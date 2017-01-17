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


		private const int ROOMS = 5;
		//Current sector index
		private int currentSector,stage, streak;

		public SchoolActivityModel(){
			MetricsController.GetController().GameStart();
			currentDifficulty = Difficulty.EASY;
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
			mediumBoards = new Sprite[ROOMS][];
			mediumAudios = new AudioClip[ROOMS][];

			for (i=0; i < ROOMS; i++) {
				easyBoards [i] = boards [i];
				easyAudios [i] = boardAudios [i];
				mediumBoards [i] = new Sprite[2];
				mediumBoards [i] [0] = boards [i + ROOMS];
				mediumBoards [i] [1] = boards [i + ROOMS*2];
				mediumAudios [i] = new AudioClip[2];
				mediumAudios [i] [0] = boardAudios [i + ROOMS];
				mediumAudios [i] [1] = boardAudios [i + ROOMS*2];
			}

		}

		void SetCurrentBoards() {
			if (currentDifficulty == Difficulty.EASY) {
				currentBoards = easyBoards;
				currentAudios = easyAudios;
			} else {
				bool randomBool = Randomizer.RandomBoolean ();
				currentBoards = randomBool ? mediumBoards[0] : mediumBoards[1];
				currentAudios = randomBool ? mediumAudios [0] : mediumAudios [1];
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