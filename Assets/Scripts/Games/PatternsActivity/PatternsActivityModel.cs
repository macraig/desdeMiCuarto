using System;
using System.Collections.Generic;
using SimpleJSON;
using Assets.Scripts.Metrics.Model;
using UnityEngine;

namespace Assets.Scripts.Games.PatternsActivity {
	public class PatternsActivityModel : LevelModel {
		private int currentLvl, difficulty;
		private List<PatternsLevel> lvls;

		public void NextLvl(){
			currentLvl++;
		}

		public bool GameEnded(){
			return currentLvl == lvls.Count;
		}

		public int CurrentLvl(){
			return currentLvl;
		}

		public void SetCurrentLevel(PatternsActivityView view, bool enabled) {
			lvls[currentLvl].Set(view, enabled);
		}

		private PatternsActivityModel(List<PatternsLevel> lvls) {
			this.lvls = lvls;
			currentLvl = 0;
			MetricsController.GetController().GameStart();

		}

		public static PatternsActivityModel StartFromJson(JSONArray lvls){
			List<PatternsLevel> classLevels = new List<PatternsLevel>();
			foreach(JSONClass lvl in lvls) {
				GameObject correct = OneGameObject(lvl["correct"]);
				List<GameObject> wrong = GameObjects(lvl["wrong"].AsArray);

				classLevels.Add(new PatternsLevel(correct, wrong));
			}

			Debug.Log(classLevels.Count + " Levels");

			return new PatternsActivityModel(classLevels);
		}

		static List<GameObject> GameObjects(JSONArray lvl) {
			return new List<JSONNode>(lvl.Childs).ConvertAll(OneGameObject);
		}

		static GameObject OneGameObject(JSONNode lvl) {
			return GameObject.Find(lvl.Value);
		}

		public void Correct() {
			LogAnswer(true);

			NextDifficulty();
		}

		void NextDifficulty() {
			difficulty++;

		}

		public void Wrong(){
			LogAnswer(false);
		}
	}
}