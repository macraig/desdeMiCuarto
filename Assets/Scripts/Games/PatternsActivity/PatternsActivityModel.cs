using System;
using System.Collections.Generic;
using SimpleJSON;
using Assets.Scripts.Metrics.Model;
using UnityEngine;
using Assets.Scripts.Common;

namespace Assets.Scripts.Games.PatternsActivity {
	public class PatternsActivityModel : LevelModel {
		private int currentLvl, difficulty, currentGrid, maxColors;
		private List<PatternsLevel> lvls;
		private PatternsLevel lvl;
		private Randomizer lvlRandomizer;
		private bool isCurrentLeft;

		public bool CanPaintLeft(){
			return isCurrentLeft;
		}

		public int currentGridIndex(){
			return currentGrid == 3 ? 0 : currentGrid == 4 ? 1 : 2;
		}

		public bool GameEnded(){
			return currentLvl == 5;
		}

		public int CurrentLvl(){
			return currentLvl;
		}

		public void NextLvl(){
			currentLvl++;
		}

		public PatternsActivityModel() {
			difficulty = 0;
			currentLvl = 0;
			isCurrentLeft = true;
			StartFromJson();
			MetricsController.GetController().GameStart();

		}

		public void StartFromJson(){
			JSONClass lvl = JSON.Parse(Resources.Load<TextAsset>("PatternsActivity/" + difficulty).text).AsObject;

			currentGrid = lvl["grid"].AsInt;
			maxColors = lvl["maxColors"].AsInt;

			JSONArray patterns = lvl["patterns"].AsArray;

			lvls = new List<PatternsLevel>();
			foreach(JSONClass pattern in patterns) {
				int imageIndex = pattern["imageIndex"].AsInt;
				List<string> usedColors = new List<JSONNode>(pattern["usedColors"].AsArray.Childs).ConvertAll((JSONNode node) => node.Value);
				List<string> colorArray = new List<JSONNode>(pattern["colorArray"].AsArray.Childs).ConvertAll((JSONNode node) => node.Value);

				lvls.Add(new PatternsLevel(imageIndex, usedColors, colorArray));
			}

			lvlRandomizer = Randomizer.New(lvls.Count - 1);
		}

		public List<string> GetColors(List<string> colors){
			return lvl.GetColors(colors, maxColors);
		}

		public List<string> GetColorArray(){
			return lvl.GetColorArray();
		}

		public void Correct() {
			LogAnswer(true);

			NextDifficulty();
		}

		void NextDifficulty() {
			difficulty++;
			StartFromJson();
		}

		public void Wrong(){
			LogAnswer(false);
		}

		public void ExchangeClick() {
			isCurrentLeft = !isCurrentLeft;
		}

		public void RandomizeLvl() {
			lvl = lvls[lvlRandomizer.Next()];
		}
	}
}