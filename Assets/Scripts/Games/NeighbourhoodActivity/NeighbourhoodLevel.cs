using System;
using Assets.Scripts.Games.NeighbourhoodActivity;
using System.Collections.Generic;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Games.NeighbourhoodActivity {
	public class NeighbourhoodLevel {
		Building correct;
		int row;
		int column;

		private NeighbourhoodLevel(Building correct, int row, int column){
			this.column = column;
			this.row = row;
			this.correct = correct;
			
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

		public static NeighbourhoodLevel CreateLevel(List<List<Building>> grid, Building correct){
			return null;
		}
	}
}