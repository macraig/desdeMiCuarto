using UnityEngine;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.App{

public class AppController : AppElement {

	public static AppController instance;
	public AppModel appModel;
    
	

	void Awake()
	{
		if (instance == null)           
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
			
		appModel = new AppModel ();
		DontDestroyOnLoad(gameObject);
		
	}

        internal void MainMenu(){
			DestroyLvl (appModel.CurrentLevel);
			ViewController.instance.LoadMainMenu();
        }

		public void StartLevel(){			
			SoundController.instance.StopMusic ();
			ViewController.instance.LoadLevel (appModel.CurrentLevel);
		}




        private string GetLevelName(int level)
        {
			
			if (level < 6) {
				switch (level) {
				case 1:
					return "Vowels";
				case 2:
					return "VowelsOral";
				case 3:
					return "StartWithVowel";
				case 4:
					return "CompleteVowel"; 
				case 5:
					return "OracionesPictogramas";
				}
			} else if (level < 12) {
				switch (level) {
				case 6:
					return "Consonants";
				case 7:
					return "ConsonantsOral"; 
				case 8:
					return "ABCWords";
				case 9:
					return "CompleteConsonant";
				case 10:
					return "ABCOrder";
				case 11:
					return "CountLetters";
					
				}
			} else if (level < 17) {
				switch (level) {
				case 12:
					return "CombineSounds"; 
				case 13:
					return "IdentifyInitialSound";
				case 14: return "MemotestEnding";
				case 15: return "SoundsInWords";
				case 16: return  "LettersComposeWords";
					
				
				}
			} else {
				switch (level) {
				case 17:return "OrderLetters";
				case 18:return "ListenAndWrite";
				case 19:return "FindError";
				case 20:return "OrderWordsDictionary";
				case 21:return "CreateSentence";
				}
			}
				
            return "Error";
        }

        internal void RestartLvl(){
			ViewController.instance.CurrentGameObject.GetComponent<LevelController>().RestartGame();
        }

		internal void RestartCompletedLvl(){
			StartLevel ();
		}

        internal void NextLvl(){
	       appModel.NextLvl();
            StartLevel();
        }

        private void DestroyLvl(int lvl)
        {
            Destroy(GameObject.Find(GetLevelName(lvl)));
        }

        internal void SetCurrentLevel(int v) {
            appModel.SetCurrentLevel(v);
        }

        internal bool IsMaxLevelPossible() {
            return appModel.CurrentLevel == appModel.MaxLevelPossible;
        }

		public int GetMaxLevelPossible(){
			return appModel.MaxLevelPossible;
		}

		public int GetCurrentLevel(){
			return appModel.CurrentLevel;
		}

		public void SetMaxLevelPossible(int level){
			appModel.SetMaxLevelPossible (level);
		}
			
       

    }

}