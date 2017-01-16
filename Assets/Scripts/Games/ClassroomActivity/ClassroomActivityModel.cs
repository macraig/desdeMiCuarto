using System;
using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Metrics.Model;

namespace Assets.Scripts.Games.ClassroomActivity {
	public class ClassroomActivityModel : LevelModel {
		

		private int currentLvl;
		private List<ClassroomLevel> lvls;

		public void NextLvl(){
			currentLvl++;
		}

		public bool GameEnded(){
			return currentLvl == lvls.Count;
		}

		public int CurrentLvl(){
			return currentLvl;
		}

		public void SetCurrentLevel(ClassroomActivityView view, bool enabled) {
			lvls[currentLvl].Set(view, enabled);
		}

		private ClassroomActivityModel(List<ClassroomLevel> lvls) {
			this.lvls = lvls;
			currentLvl = 0;
			MetricsController.GetController().GameStart();

		}

		public static ClassroomActivityModel StartFromJson(JSONArray lvls){
			List<ClassroomLevel> classLevels = new List<ClassroomLevel>();
			foreach(JSONClass lvl in lvls) {
				GameObject correct = OneGameObject(lvl["correct"]);
				List<GameObject> wrong = GameObjects(lvl["wrong"].AsArray);

				classLevels.Add(new ClassroomLevel(correct, wrong));
			}

			Debug.Log(classLevels.Count + " Levels");

			return new ClassroomActivityModel(classLevels);
		}

		static List<GameObject> GameObjects(JSONArray lvl) {
			return new List<JSONNode>(lvl.Childs).ConvertAll(OneGameObject);
		}

		static GameObject OneGameObject(JSONNode lvl) {
			return GameObject.Find(lvl.Value);
		}

		public void Correct() {
			LogAnswer(true);
		}

		public void Wrong(){
			LogAnswer(false);
		}
	}
}