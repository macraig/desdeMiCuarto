using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;
using System;

namespace Assets.Scripts.App{

	public class AppModel  {

		private const int TOTAL_LEVELS = 21;

		private int currentLevel;
		private int maxLevelPossible;
		private List<string> gameTitlesSpanish;
		private List<string> gameTitlesEnglish;
		private List<string> gameInfoEnglish;
		private List<string> gameInfoSpanish;
		private List<string> instructionsSpanish;
		private List<string> instructionsEnglish;


		public AppModel(){
			currentLevel = 1;
			maxLevelPossible = 1;
			gameTitlesSpanish = new List<string> ();
			gameTitlesEnglish = new List<string> ();
			gameInfoEnglish = new List<string> ();
			gameInfoSpanish = new List<string> ();

			instructionsEnglish = new List<string> ();
			instructionsSpanish = new List<string> ();

			TextAsset JSONstring = Resources.Load("activities") as TextAsset;

			JSONNode data = JSON.Parse(JSONstring.text);

			for (int i = 0; i < TOTAL_LEVELS; i++) {
				gameTitlesSpanish.Add (data["activities"][i]["nameSpanish"]);
				gameTitlesEnglish.Add (data["activities"][i]["nameEnglish"]);
				gameInfoSpanish.Add (data["activities"][i]["infoSpanish"]);
				gameInfoEnglish.Add (data["activities"][i]["infoEnglish"]);
				instructionsEnglish.Add (data["activities"][i]["insEnglish"]);
				instructionsSpanish.Add (data["activities"][i]["insSpanish"]);

			}

		}

        internal void SetCurrentLevel(int v)
        {
            currentLevel = v;
        }

		public void SetMaxLevelPossible(int level){
			maxLevelPossible = level;
		}

        public List<string> GameInfoSpanish {
			get {
				return gameInfoSpanish;
			}
		}

		public List<string> GameInfoEnglish {
			get {
				return gameInfoEnglish;
			}
		}

		public List<string> GameTitlesEnglish {
			get {
				return gameTitlesEnglish;
			}
		}

        internal void NextLvl()
        {
			currentLevel += currentLevel == TOTAL_LEVELS ? 0 : 1;
        }

        public List<string> GameTitlesSpanish {
			get {
				return gameTitlesSpanish;
			}
		}

		public List<string> InstructionsSpanish {
			get {
				return instructionsSpanish;
			}
		}

		public List<string> InstructionsEnglish {
			get {
				return instructionsEnglish;
			}
		}

	


		public int MaxLevelPossible {
			get {
				return maxLevelPossible;
			}
		}

		public int CurrentLevel {
			get {
				return currentLevel;
			}
		}

        internal string GetTitleFromIndex(int activity)
        {
            switch (SettingsController.instance.GetLanguage())
            {
                case 0:
                    return gameTitlesSpanish[activity];
                case 1:
                    return GameTitlesEnglish[activity];
                default:
                    return "Error";
            }
        }
    }

}
