using System;
using Assets.Scripts.Games.NeighbourhoodActivity;
using System.Collections.Generic;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Games.NeighbourhoodActivity {
	public class NeighbourhoodLevel {
		Building correct;
		int row, column;
		Possibilities p;
		List<AudioClip> audios;

		private NeighbourhoodLevel(Building correct, int row, int column, Possibilities p){
			this.column = column;
			this.row = row;
			this.correct = correct;
			this.p = p;
		}

		public List<Building> GetChoices(List<Building> simpleBuildings) {
			Randomizer r = Randomizer.New(simpleBuildings.Count - 1);
			List<Building> choices = new List<Building>();
			choices.Add(correct);
			while(choices.Count < 5){
				Building b = simpleBuildings[r.Next()];
				if(!choices.Contains(b)) choices.Add(b);
			}
			return choices;
		}

		public bool IsCorrectDragger(Sprite dragger, Dictionary<string, Sprite> buildingSprites) {
			return correct.GetSprite(buildingSprites) == dragger;
		}

		public bool IsCorrectSlot(int row, int column) {
			return row == this.row && column == this.column;
		}

		// TEXT CREATION *******************************************************************************************************************

		public string GetText(List<List<Building>> grid){
			string result = correct.GetName();

			switch(p) {
			case Possibilities.BEHIND:
				result = result + " atras de " + grid[row + 1][column].GetName();
				break;
			case Possibilities.BEHIND_SCHOOL:
				//no puede ir ninguno aca porque es ambiguo. Si estoy atras de la escuela puedo estar en cualquiera de los dos lugares libres.
				break;
			case Possibilities.BEHIND_STREET:
				result = result + " atras de " + grid[row + 2][column].GetName() + ", cruzando la calle";
				break;
			case Possibilities.BETWEEN_VERTICAL:
				Building upperB = grid[row - 1][column];
				Building downB = grid[row + 1][column];
				result = result + " entre " + upperB.GetName() + " y " + downB.GetName();
				break;
			case Possibilities.BETWEEN_HORIZONTAL:
				Building leftB = grid[row][column - 1];
				Building rightB = grid[row][column + 1];
				result = result + " entre " + leftB.GetName() + " y " + rightB.GetName();
				break;
			case Possibilities.IN_FRONT:
				result = result + " frente a " + grid[row - 1][column].GetName();
				break;
			case Possibilities.IN_FRONT_SCHOOL_STREET:
				result = result + " frente a la escuela, cruzando la calle";
				break;
			case Possibilities.IN_FRONT_STREET:
				result = result + " frente a " + grid[row - 2][column].GetName() + ", cruzando la calle";
				break;
			case Possibilities.LEFT:
				result = result + " a la izquierda de " + grid[row][column + 1].GetName();
				break;
			case Possibilities.LEFT_SCHOOL_STREET:
				result = result + " a la izquierda de la escuela";
				break;
			case Possibilities.LEFT_STREET:
				result = result + " a la izquierda de " + grid[row][column + 2].GetName();
				break;
			case Possibilities.RIGHT:
				result = result + " a la derecha de " + grid[row][column - 1].GetName();
				break;
			case Possibilities.RIGHT_SCHOOL_STREET:
				result = result + " a la derecha de la escuela";
				break;
			case Possibilities.RIGHT_STREET:
				result = result + " a la derecha de " + grid[row][column - 2].GetName() + ", cruzando la calle";
				break;
			}

			return result;
		}

		// LEVEL CREATION ******************************************************************************************************************

		public static NeighbourhoodLevel CreateLevel(List<List<Building>> grid, Building correct){
			Possibilities p = Possibilities.EMPTY;
			Array values = Enum.GetValues(typeof(Possibilities));
			List<Vector2> freeSpaces = FreeSpaces(grid);
			Randomizer r = Randomizer.New(freeSpaces.Count - 1);

			while(true){
				Vector2 randomFreeSpace = freeSpaces[r.Next()];
				int row = (int) randomFreeSpace.x, column = (int) randomFreeSpace.y;
				Randomizer enumRandomizer = Randomizer.New(values.Length - 1);
				for(int i = 0; i < values.Length; i++) {
					Possibilities value = (Possibilities) values.GetValue(enumRandomizer.Next());
					if(PossibilityApplies(row, column, grid, value)){
						p = value;
						break;
					}
				}

				if(p != Possibilities.EMPTY) {
					grid[row][column] = correct;
					return new NeighbourhoodLevel(correct, row, column, p);
				}
			}
		}

		static bool PossibilityApplies(int row, int column, List<List<Building>> grid, Possibilities p) {
			Building spot, streetSpot;
			switch(p) {
			case Possibilities.BEHIND:
				if(row == 5)
					return false;
				spot = grid[row + 1][column];
				if(spot != null && !spot.IsStreet() && spot.GetName() != "escuela")
					return true;
				break;
			case Possibilities.BEHIND_SCHOOL:
				//no puede ir ninguno aca porque es ambiguo. Si estoy atras de la escuela puedo estar en cualquiera de los dos lugares libres.
				return false;
			case Possibilities.BEHIND_STREET:
				if(row >= 4)
					return false;
				streetSpot = grid[row + 1][column];
				spot = grid[row + 2][column];
				if(spot != null && streetSpot != null && !spot.IsStreet() && streetSpot.IsStreet())
					return true;
				break;
			case Possibilities.BETWEEN_VERTICAL:
				if(row >= 1 && row <= 4) {
					Building upperB = grid[row - 1][column];
					Building downB = grid[row + 1][column];
					if(upperB != null && !upperB.IsStreet() && downB != null && !downB.IsStreet())
						return true;
				}
				break;
			case Possibilities.BETWEEN_HORIZONTAL:
				if(column >= 1 && column <= 4){
					Building leftB = grid[row][column - 1];
					Building rightB = grid[row][column + 1];
					if(leftB != null && !leftB.IsStreet() && rightB != null && !rightB.IsStreet()) return true;
				}
				break;
			case Possibilities.IN_FRONT:
				if(row == 0)
					return false;
				spot = grid[row - 1][column];
				if(spot != null && !spot.IsStreet())
					return true;
				break;
			case Possibilities.IN_FRONT_SCHOOL_STREET:
				//si estoy en frente de la escuela y el del costado mio no esta vacio.
				if(row == 5 && (column == 2 || column == 3)) {
					return grid[5][column == 2 ? 3 : 2] != null;
				}
				break;
			case Possibilities.IN_FRONT_STREET:
				if(row <= 1)
					return false;
				streetSpot = grid[row - 1][column];
				spot = grid[row - 2][column];
				if(spot != null && streetSpot != null && !spot.IsStreet() && streetSpot.IsStreet())
					return true;
				break;
			case Possibilities.LEFT:
				//this means i'm left of something
				if(column == 5)
					return false;
				spot = grid[row][column + 1];
				if(column < 5 && spot != null && !spot.IsStreet())
					return true;
				break;
			case Possibilities.LEFT_SCHOOL_STREET:
				return row == 3 && column == 0;
			case Possibilities.LEFT_STREET:
				if(column >= 4)
					return false;
				streetSpot = grid[row][column + 1];
				spot = grid[row][column + 2];
				if(spot != null && streetSpot != null && !spot.IsStreet() && streetSpot.IsStreet())
					return true;
				break;
			case Possibilities.RIGHT:
				if(column == 0)
					return false;
				spot = grid[row][column - 1];
				if(spot != null && !spot.IsStreet())
					return true;
				break;
			case Possibilities.RIGHT_SCHOOL_STREET:
				return row == 3 && column == 5;
			case Possibilities.RIGHT_STREET:
				if(column <= 1)
					return false;
				streetSpot = grid[row][column - 1];
				spot = grid[row][column - 2];
				if(spot != null && streetSpot != null && !spot.IsStreet() && streetSpot.IsStreet())
					return true;
				break;
			}

			return false;
		}

		static List<Vector2> FreeSpaces(List<List<Building>> grid) {
			List<Vector2> result = new List<Vector2>();
			for(int row = 0; row < grid.Count; row++) {
				List<Building> r = grid[row];
				for(int column = 0; column < r.Count; column++) {
					if(r[column] == null)
						result.Add(new Vector2(row, column));
				}
			}

			return result;
		}
	}
}