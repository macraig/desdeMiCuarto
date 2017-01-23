using System;
using Assets.Scripts.Games;
using UnityEngine;
using Assets.Scripts.Common;
using System.Collections.Generic;
using Assets.Scripts.Metrics.Model;

namespace Assets.Scripts.Games.SchoolActivity {
	public class SchoolActivityModel : LevelModel {
		
		public const int MAX_INSTRUCTIONS = 7;
		private Difficulty currentDifficulty;
		private Randomizer sectorRandomizer;
		private Sprite[] boards,currentBoards,easyBoards,directionSprites;
		private Sprite[][] mediumBoards;
		private AudioClip[] boardAudios, currentAudios, easyAudios;
		private AudioClip[][] mediumAudios;
		private List<string> instructions;

		private const int ROOMS = 6;
		//Current sector index
		private int currentSector,stage, streak;

		public SchoolActivityModel(){
			MetricsController.GetController().GameStart();
			currentDifficulty = Difficulty.EASY;
			boards = Resources.LoadAll<Sprite>("Sprites/SchoolActivity/consignas");
			directionSprites = Resources.LoadAll<Sprite>("Sprites/SchoolActivity/direcciones");
			boardAudios = Resources.LoadAll<AudioClip>("Audio/SchoolActivity/consignas");
			InitBoards ();
			SetCurrentBoards();
			sectorRandomizer = Randomizer.New(ROOMS - 1);
			stage = 0;
			streak = 0;
			instructions = new List<string> ();
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

		public Sprite DirectionSprite(Direction dir){
			switch (dir) {
			case Direction.LEFT:
				return directionSprites [0];
			case Direction.UP:
				return directionSprites [1];
			case Direction.RIGHT:
				return directionSprites [2];
			case Direction.DOWN:
				return directionSprites [3];
			}
			return null;
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


		public void AddInstruction (string dir)
		{
			instructions.Add (dir);

		}

		public bool ReachedMaxInstructions(){
			return instructions.Count >= MAX_INSTRUCTIONS;
		}

		public Direction ParseToDirection(string dir){
			switch (dir) {
			case "UP":
				return Direction.UP;
			case "DOWN":
				return Direction.DOWN;
			case "LEFT":
				return Direction.LEFT;
			case "RIGHT":
				return Direction.RIGHT;
			}
			return Direction.LEFT;
		}

		public Direction ParseFromSprite (Sprite sprite)
		{
			if (sprite == directionSprites [0])
				return Direction.LEFT;
			else if (sprite == directionSprites [1])
				return Direction.UP;
			else if (sprite == directionSprites [2])
				return Direction.RIGHT;
			else
				return Direction.DOWN;
			

		}


		public Vector2 ParseInstruction (Direction direction)
		{
			switch (direction) {
				case Direction.UP:
					return new Vector2(0,-1);
				case Direction.DOWN:
					return new Vector2(0,1);
				case Direction.LEFT:
					return new Vector2(-1,0);
				case Direction.RIGHT:
					return new Vector2(1,0);
			}
			return new Vector2(0,0);

		}

	}



}