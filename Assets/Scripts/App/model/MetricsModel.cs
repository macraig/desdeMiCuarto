using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.App{
    [Serializable]
	public class MetricsModel 
	{
        private const int MAX_SCORE = 10000;
        private const int MIN_SCORE = 500;
		private const int TOTAL_LEVELS = 21;
		public List<List<GameMetrics>> metrics;

		public MetricsModel(){
			CreateLevelMetrics ();
		}

		private void CreateLevelMetrics(){
			
			metrics = new  List<List<GameMetrics>> ();
			//TODO: Check why using GetLevelAmount throws error
			for (int i = 0; i < TOTAL_LEVELS; i++) {
                metrics.Add (new List<GameMetrics>());
			}
		}

		public bool LevelHasMetrics (int mode, int level){
			List<List<GameMetrics>> filteredList = GetMetricsByMode (mode);
			return filteredList [level].Count > 0;
		}

        internal void DiscardCurrentMetrics()
        {
            metrics[AppController.instance.appModel.CurrentLevel - 1].RemoveAt(metrics[AppController.instance.appModel.CurrentLevel - 1].Count - 1);
        }

        internal void GameFinished(int lapsedSeconds, int minSeconds, int pointsPerSecond, int pointsPerError){
            CalculateFinalScore(lapsedSeconds, minSeconds, pointsPerSecond, pointsPerError);
            GetCurrentMetrics().stars = CalculateStars();
        }

        internal void AddWrongAnswer() {
            GetCurrentMetrics().wrongAnswers++;
        }

        internal void AddRightAnswer()
        {
            GetCurrentMetrics().rightAnswers++;
        }

        internal void AddHint()
        {
            GetCurrentMetrics().hints++;
        }

        internal GameMetrics GetCurrentMetrics()
        {
			Debug.Log (AppController.instance.appModel.CurrentLevel - 1);
			Debug.Log(metrics[AppController.instance.appModel.CurrentLevel - 1].Count - 1);
			return metrics[AppController.instance.appModel.CurrentLevel - 1][metrics[AppController.instance.appModel.CurrentLevel - 1].Count - 1];
        }

        internal List<int> GetLevelIndexes(int currentPage, int maxRows)
        {
            List<int> myList = new List<int>(maxRows);
            int initIndex = currentPage * maxRows;
            for (int i = initIndex; i < initIndex + maxRows && i < metrics.Count && initIndex >= 0; i++)
            {
                myList.Add(i);
            }
            return myList;
        }

        internal GameMetrics GetBestMetric(int level)
        {
            if (metrics[level].Count == 0) return null;
            GameMetrics max = metrics[level][0];
            for (int i = 1; i < metrics[level].Count; i++){
                if (metrics[level][i].score > max.score){max = metrics[level][i];}
            }
            Debug.Log(level + ": max score -> " + max.score);
            return max;
        }
      
        internal void GameStarted(){
            metrics[AppController.instance.appModel.CurrentLevel - 1].Add(new GameMetrics(AppController.instance.appModel.CurrentLevel - 1, SettingsController.instance.GetMode()));
        }       

        private void CalculateFinalScore(int lapsedSeconds, int minSeconds, int pointsPerSecond, int pointsPerError){
            
			GetCurrentMetrics ().score = MAX_SCORE;
			if (SettingsController.instance.GetMode () == 1) {
				GetCurrentMetrics().lapsedTime = lapsedSeconds;
				GetCurrentMetrics ().score -= pointsPerSecond * (lapsedSeconds < minSeconds ? 0 : lapsedSeconds - minSeconds);
			}
			GetCurrentMetrics().score -= GetCurrentMetrics().wrongAnswers * pointsPerError;

            if (GetCurrentMetrics().score < MIN_SCORE) GetCurrentMetrics().score = MIN_SCORE;
           
        }

        public int GetFinalScore(){
            return GetCurrentMetrics().score;
        }
     
        private int CalculateStars(){
            float percentage = (GetCurrentMetrics().score + 0f) / (MAX_SCORE + 0f);

            if (percentage > 0.75)
            {
                return 3;
            } else if(percentage > 0.5)
            {
                return 2;
            } else if(percentage > 0.25)
            {
                return 1;
            } else
            {
                return 0;
            }
        }

        public List<List<GameMetrics>> GetMetricsByMode(int mode)
        {
            List<List<GameMetrics>> filteredList = new List<List<GameMetrics>>();
            for(int i = 0; i < metrics.Count; i++)
            {
				filteredList.Add(new List<GameMetrics>());
                for(int j = 0; j < metrics[i].Count; j++) {
                    if (metrics[i][j].IsMode(mode)) filteredList[i].Add(metrics[i][j]);
                }
            }
            return filteredList;
        }
    }
}

