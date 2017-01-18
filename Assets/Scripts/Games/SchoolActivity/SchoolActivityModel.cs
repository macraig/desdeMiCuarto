using System;
using Assets.Scripts.Games;
using UnityEngine;
using Assets.Scripts.Common;
using System.Collections.Generic;
using Assets.Scripts.Metrics.Model;

namespace Assets.Scripts.Games.SchoolActivity {
	public class SchoolActivityModel : LevelModel {
		private Difficulty currentDifficulty;
		private Randomizer sectorRandomizer;
		private Sprite[] boards,currentBoards,easyBoards;
		private Sprite[][] mediumBoards;
		private AudioClip[] boardAudios, currentAudios, easyAudios;
		private AudioClip[][] mediumAudios;


		private const int ROOMS = 6;
		//Current sector index
		private int currentSector,stage, streak;

		public SchoolActivityModel(){
			MetricsController.GetController().GameStart();
			currentDifficulty = Difficulty.HARD;
			boards = Resources.LoadAll<Sprite>("Sprites/SchoolActivity/consignas");
			boardAudios = Resources.LoadAll<AudioClip>("Audio/SchoolActivity/consignas");
			InitBoards ();
			SetCurrentBoards();
			sectorRandomizer = Randomizer.New(ROOMS - 1);
			stage = 0;
			streak = 0;
		}

		void InitBoards(){
			int i;
			easyBoards = new Sprite[ROOMS];
			easyAudios = new AudioClip[ROOMS];
			mediumBoards = new Sprite[2][];
			mediumAudios = new AudioClip[2][];
			mediumBoards [0] = new Sprite[ROOMS];
			mediumBoards [1] = new Sprite[ROOMS];
			mediumAudios [0] = new AudioClip[ROOMS];
			mediumAudios [1] = new AudioClip[ROOMS];

			for (i=0; i < ROOMS; i++) {
				easyBoards [i] = boards [i];
				easyAudios [i] = boardAudios [i];
				mediumBoards [0] [i] = boards [i + ROOMS];
				mediumBoards [1] [i] = boards [i + ROOMS*2];
				mediumAudios [0] [i] = boardAudios [i + ROOMS];
				mediumAudios [1] [i] = boardAudios [i + ROOMS*2];
			}

		}

		void SetCurrentBoards() {
			if (currentDifficulty == Difficulty.EASY) {
				currentBoards = easyBoards;
				currentAudios = easyAudios;
			} else {
				bool randomBool = Randomizer.RandomBoolean ();
				currentAudios = randomBool ? mediumAudios [0] : mediumAudios [1];
				currentBoards = randomBool ? mediumBoards[0] : mediumBoards[1];
			}
		}

		public void SetSector() {
			currentSector = sectorRandomizer.Next();
		}



		public AudioClip BoardClip() {
			return currentAudios [currentSector];

		}

		public Sprite BoardSprite() {
			return currentBoards[currentSector];
		}

		public void NextStage(){
			stage++;
			CheckDifficulty();
		}

		public int GetStage() {
			return stage;
		}

		public bool IsFinished(){
			return stage == 6;
		}

		void CheckDifficulty() {
			if(streak > 1){
				currentDifficulty = Difficulty.HARD;
				SetCurrentBoards();
			} else {
				currentDifficulty = Difficulty.EASY;
				SetCurrentBoards();
			}
		}

		public bool IsCorrectSector(int selected) {
			return selected == currentSector;
		}


	}
}