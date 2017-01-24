using System;
using Assets.Scripts.Metrics.Model;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Common;

namespace Assets.Scripts.Games.NeighbourhoodActivity {
	public class NeighbourhoodActivityModel : LevelModel {
		public static Building STREET = Building.B("street", false, true);
		public static int SIMPLE_BUILDINGS = 13;
		private int currentLvl;

		private Dictionary<string, Sprite> buildingSprites;
		private Dictionary<string, AudioClip> audios;
		private List<List<Building>> grid;
		private List<Building> simpleBuildings, options;
		private string[] names;
		private List<NeighbourhoodLevel> lvls;

		public List<Building> GetGrid() {
			List<Building> result = new List<Building>();
			foreach(List<Building> l in grid) {
				result.AddRange(l);
			}
			return result;
		}

		public bool IsCorrectDragger(Sprite dragger) {
			return lvls[currentLvl].IsCorrectDragger(dragger, buildingSprites);
		}

		public bool IsCorrectSlot(int row, int column) {
			return lvls[currentLvl].IsCorrectSlot(row, column);
		}

		//randomizo 5 cualquiera 
		public List<Building> GetChoices() {
			return lvls[currentLvl].GetChoices(simpleBuildings);
		}

		public Sprite GetSprite(Building b){
			return b.GetSprite(buildingSprites);
		}

		public void NextLvl(){
			currentLvl++;
		}

		public bool GameEnded(){
			return currentLvl == 5;
		}

		public int CurrentLvl(){
			return currentLvl;
		}

		public NeighbourhoodActivityModel() {
			currentLvl = 0;
			//nombres de los edificios en orden de sprites.
			InitNames();
			//inicia la grilla con las calles y la escuela.
			InitGrid();
			//inicia el sprite de cada edificio con su nombre.
			InitSprites();
			//inicia los audios con sus nombres (edificios y consignas).
			InitAudios();
			//inicia los objetos de los edificios simples.
			InitSimpleBuildings();

			//randomiza las 5 respuestas correctas.
			RandomizeOptions();

			//mete la plaza y el hospital en la grilla.
			SetDoubleBuildingsInGrid();
			//mete los 5 random en la grilla y los borra de simpleBuildings.
			SetSimpleBuildingsInGrid();

			//armar cada nivel y su consigna.
			CreateLevels();

			MetricsController.GetController().GameStart();
		}

		void CreateLevels() {
			lvls = new List<NeighbourhoodLevel>();
			foreach(Building o in options) {
				lvls.Add(NeighbourhoodLevel.CreateLevel(grid, o));
			}
		}

		void RandomizeOptions() {
			options = new List<Building>();
			Randomizer r = Randomizer.New(simpleBuildings.Count - 1);

			while(options.Count < 5){
				options.Add(simpleBuildings[r.Next()]);
			}
		}

		Building GetSimpleBuilding() {
			Randomizer r = Randomizer.New(simpleBuildings.Count - 1);
			Building b = null;
			while(b == null && options.Contains(b)){
				b = simpleBuildings[r.Next()];
			}

			simpleBuildings.Remove(b);
			return b;
		}

		// Recontra mega ultra hiper cableado :)
		void SetSimpleBuildingsInGrid() {
			Building a = GetSimpleBuilding();
			bool up = Randomizer.RandomBoolean();
			Randomizer r = Randomizer.New(up ? 5 : 3, up ? 2 : 0);
			//primero: el que va con la plaza o el hospital.
			while(true){
				int spot = r.Next();

				if(grid[up ? 0 : 5][spot] == null){
					grid[up ? 0 : 5][spot] = a;
					break;
				}
			}

			//segundo: los dos de la izquierda.
			r = Randomizer.New(3);

			grid[r.Next()][0] = GetSimpleBuilding();
			grid[r.Next()][0] = GetSimpleBuilding();

			//tercero: los dos de la derecha.
			r = Randomizer.New(5, 2);

			grid[r.Next()][5] = GetSimpleBuilding();
			grid[r.Next()][5] = GetSimpleBuilding();
		}

		void SetDoubleBuildingsInGrid() {
			bool doubleRandom = Randomizer.RandomBoolean();

			Building up = doubleRandom ? Building.B("hospital", true) : Building.B("plaza", true);
			Building down = doubleRandom ? Building.B("plaza", true) : Building.B("hospital", true);

			bool middleRandom = Randomizer.RandomBoolean();
			bool left = Randomizer.RandomBoolean();

			if(middleRandom){
				grid[0][3] = up;
				grid[0][4] = up;

				grid[5][left ? 0 : 2] = down;
				grid[5][left ? 1 : 3] = down;
			} else {
				grid[0][left ? 2 : 4] = up;
				grid[0][left ? 3 : 5] = up;

				grid[5][1] = down;
				grid[5][2] = down;
			}
		}

		public void Correct() {
			LogAnswer(true);
		}

		public void Wrong(){
			LogAnswer(false);
		}

		void InitSimpleBuildings() {
			simpleBuildings = new List<Building>();
			for(int i = 0; i < SIMPLE_BUILDINGS; i++) {
				simpleBuildings.Add(Building.B(names[i]));
			}
		}

		void InitAudios() {
			audios = new Dictionary<string, AudioClip>();

			//TODO audios.
		}

		void InitNames() {
			names = new string[] {
				"heladeria",
				"banco",
				"correo",
				"floreria",
				"biblioteca",
				"policia",
				"verduleria",
				"kiosko",
				"carniceria",
				"casaBlanca",
				"estacionDeServicio",
				"casaAmarilla",
				"supermercado",
				"cine",
				"plazaLeft",
				"plazaRight",
				"hospitalLeft",
				"hospitalRight",
				"escuelaLeft",
				"escuelaRight"
			};
		}

		void InitSprites() {
			buildingSprites = new Dictionary<string, Sprite>();
			Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/NeighbourhoodActivity/edificios");

			for(int i = 0; i < names.Length; i++) {
				buildingSprites.Add(names[i], sprites[i]);
			}
		}

		void InitGrid() {
			Building escuela = Building.B("escuela", true);

			grid = new List<List<Building>>();
			grid[0] = new List<Building>{ null, STREET, null, null, null, null };
			grid[1] = new List<Building>{ null, STREET, STREET, STREET, STREET, STREET };
			grid[2] = new List<Building>{ null, STREET, null, null, STREET, null };
			grid[3] = new List<Building>{ null, STREET, escuela, escuela, STREET, null };
			grid[4] = new List<Building>{ STREET, STREET, STREET, STREET, STREET, null };
			grid[5] = new List<Building>{ null, null, null, null, STREET, null };
		}
	}
}