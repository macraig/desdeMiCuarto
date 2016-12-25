using UnityEngine;
using System.Collections;
using Assets.Scripts.App;
using System;
//using Assets.Scripts.Timer;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace Assets.Scripts.App{

public class MetricsController : MonoBehaviour {

		public static MetricsController instance;
		public MetricsModel metricsModel;

		//Awake is always called before any Start functions
		void Awake()
		{
			if (instance == null)           
				instance = this;
			else if (instance != this)
				Destroy(gameObject);
			DontDestroyOnLoad(gameObject);
			metricsModel = new MetricsModel();
		}

        internal void DiscardCurrentMetrics()
        {
            metricsModel.DiscardCurrentMetrics();
        }

        public void GameStart() {
            metricsModel.GameStarted();
//            TimerImpl.instance.InitTimer();
        }

        public void GameFinished(int minSeconds, int pointsPerSecond, int pointsPerError){
//            TimerImpl.instance.FinishTimer();
//            metricsModel.GameFinished(TimerImpl.instance.GetLapsedSeconds(), minSeconds, pointsPerSecond, pointsPerError);
            saveToDisk();
        }

		public void LogAnswer(bool isCorrect){
            if (isCorrect) { metricsModel.AddRightAnswer(); } else { metricsModel.AddWrongAnswer(); }
		}

		public void LogHint(){
            metricsModel.AddHint();
		}

		public int GetLevelStars(int level){
			if (metricsModel.LevelHasMetrics (1,level)) return MetricsController.instance.metricsModel.GetBestMetric(level).stars;;		


			return 0;		
		}

        private void saveToDisk(){
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/"+ SettingsController.instance.GetUsername() + ".2dat");
            bf.Serialize(file, metricsModel);
            file.Close();
        }

        public void LoadFromDisk(){
            if(File.Exists(Application.persistentDataPath + "/" + SettingsController.instance.GetUsername() + ".2dat")){
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/" + SettingsController.instance.GetUsername() + ".2dat", FileMode.Open);
                metricsModel = (MetricsModel) bf.Deserialize(file);
                file.Close();
            } else
            {
                metricsModel = new MetricsModel();
            }
        }

        internal List<GameMetrics> GetMetricsByLevel(int level)
        {
            return metricsModel.metrics[level];
        }
    }
}