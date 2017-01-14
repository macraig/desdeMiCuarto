using System;
using Assets.Scripts.Games;
using UnityEngine;
using Assets.Scripts.Common;
using Assets.Scripts.Metrics.Model;

namespace Assets.Scripts.Games.HouseActivity {
	public class HouseActivityModel : LevelModel {
		private Difficulty currentDifficulty;
		private Randomizer sectorRandomizer;
		private Sprite[] boards;

		private int currentSector, stage;

		public HouseActivityModel(){
			MetricsController.GetController().GameStart();
			currentDifficulty = Difficulty.EASY;
			SetBoards();
			sectorRandomizer = Randomizer.New(boards.Length - 1);
			stage = 0;
		}

		public void SetSector() {
			currentSector = sectorRandomizer.Next();
		}

		void SetBoards() {
			boards = Resources.LoadAll<Sprite>("Sprites/HouseActivity/consignas" + (currentDifficulty == Difficulty.EASY ? "1" : currentDifficulty == Difficulty.MEDIUM ? "2" : "3"));
		}

		public AudioClip BoardClip() {
			return Resources.Load<AudioClip>("Audio/HouseActivity/Consignas/" + (currentDifficulty == Difficulty.EASY ? "Easy" : currentDifficulty == Difficulty.MEDIUM ? "Medium" : "Hard") + "/" + currentSector);
		}

		public Sprite BoardSprite() {
			return boards[currentSector];
		}

		public void NextStage(){
			stage++;
			CheckStage();
		}

		public int GetStage() {
			return stage;
		}

		public bool IsFinished(){
			return stage == 6;
		}

		void CheckStage() {
			if(stage == 2){
				currentDifficulty = Difficulty.MEDIUM;
				SetBoards();
			} else if(stage == 4){
				currentDifficulty = Difficulty.HARD;
				SetBoards();
			}
		}

		public bool IsCorrectSector(int selected) {
			return selected == currentSector;
		}

		#region implemented abstract members of LevelModel
		public override void NextChallenge() { }
		public override void InitGame() { }
		public override void RestartGame() { }
		#endregion
	}
}