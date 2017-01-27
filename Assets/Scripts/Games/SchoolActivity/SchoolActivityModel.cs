using System;
using Assets.Scripts.Games;
using UnityEngine;
using Assets.Scripts.Common;
using System.Collections.Generic;
using Assets.Scripts.Metrics.Model;

namespace Assets.Scripts.Games.SchoolActivity {
	public class SchoolActivityModel : LevelModel {
		
		public static int MAX_INSTRUCTIONS = 7;
		public const int ROWS = 8, COLS = 10;


		private Difficulty currentDifficulty;
		private Randomizer sectorRandomizer;
		private Sprite[] boards,currentBoards,easyBoards,directionSprites;
		private Sprite[][] mediumBoards;
		private AudioClip[] boardAudios, currentAudios, easyAudios;
		private AudioClip[][] mediumAudios;
		private List<string> instructions;
		private Tile currentTile;

		private int[][] schoolGrid;
		private Vector2 santiPos = new Vector2(3,4);

		private const int ROOMS = 6;
		//Current sector index
		private int currentSector,stage, streak;

		public SchoolActivityModel(){
			MetricsController.GetController().GameStart();
			currentDifficulty = Difficulty.EASY;

			boards = Resources.LoadAll<Sprite>("Sprites/SchoolActivity/consignas");
			directionSprites = Resources.LoadAll<Sprite>("Sprites/SchoolActivity/direcciones");
			boardAudios = Resources.LoadAll<AudioClip>("Audio/SchoolActivity/consignas");

			InitGrid ();
			InitBoards ();
			SetCurrentBoards();
			santiPos = new Vector2 (3,4);

			sectorRandomizer = Randomizer.New(7,2);
			stage = 0;
			streak = 0;
			instructions = new List<string> ();
		}

		public bool AnalizeMovement (Vector2 moveVector){
			Vector2 newPosition = santiPos + moveVector;
			//Check if newPosition in within the grid
			if (newPosition.x < 0 || newPosition.y < 0 || newPosition.x >= ROWS || newPosition.y >= COLS)
				return false;
			
			int tile = schoolGrid [(int)newPosition.x] [(int)newPosition.y];

			SetCurrentTile (tile);

			//Is not a wall
			if (tile != 1) {
				return true;
			}
			return false;
		}

		public void UpdateSantiPosition(Vector2 newPosition){
			santiPos = newPosition;
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

		public Vector2 GetSantiPos ()
		{
			return santiPos;
		}


		public AudioClip BoardClip() {
			return currentAudios [currentSector-2];

		}

		public Sprite BoardSprite() {
			return currentBoards[currentSector-2];
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

		public bool IsCorrectSector() {
			return currentTile == ParseTile(currentSector);
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
					return new Vector2(-1,0);
				case Direction.DOWN:
					return new Vector2(1,0);
				case Direction.LEFT:
					return new Vector2(0,-1);
				case Direction.RIGHT:
					return new Vector2(0,1);
			}
			return new Vector2(0,0);

		}



		void InitGrid ()
		{
			/*
			GRID TILES:
				0: empty
				1: wall
				2: classroom 3:lab 4:library 5:playground 6:entrance 7:lunch 8:bathroom
			*/
			schoolGrid = new int[ROWS][];
			schoolGrid[0] = new int[COLS]{0,0,0,1,0,1,0,0,1,0};
			schoolGrid[1] = new int[COLS]{0,0,0,2,0,3,0,0,1,0};
			schoolGrid[2] = new int[COLS]{1,1,1,1,0,1,1,1,1,1};
			schoolGrid[3] = new int[COLS]{0,0,0,0,0,0,0,0,0,5};
			schoolGrid[4] = new int[COLS]{1,4,1,1,0,1,8,1,1,1};
			schoolGrid[5] = new int[COLS]{0,0,0,1,0,7,0,0,0,0};
			schoolGrid[6] = new int[COLS]{0,0,0,1,0,1,0,0,0,0};
			schoolGrid[7] = new int[COLS]{0,0,1,6,0,1,0,0,0,0};

		}

		public Tile ParseTile (int tile)
		{
			switch (tile) {
			case 0:
				return Tile.EMPTY;
			case 1:
				return Tile.WALL;
			case 2:
				return Tile.CLASS;
			case 3:
				return Tile.LAB;
			case 4:
				return Tile.LIBRARY;
			case 5:
				return Tile.PLAYGROUND;
			case 6:
				return Tile.ENTRANCE;
			case 8:
				return Tile.BATHROOM;
			}
			Debug.Log ("Error: Check ParseTile in SchoolActivityModel");
			return Tile.EMPTY;
		}

		public void SetCurrentTile(int tile){
			currentTile = ParseTile (tile);
		}

		public Tile GetCurrentTile(){
			return currentTile;
		}

		public int GetRows(){
			return ROWS;
		}

		public int GetCols(){
			return COLS;
		}
	
		

	}




}