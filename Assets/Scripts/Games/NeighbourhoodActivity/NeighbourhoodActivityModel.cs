using System;
using Assets.Scripts.Metrics.Model;

namespace Assets.Scripts.Games.NeighbourhoodActivity {
	public class NeighbourhoodActivityModel : LevelModel {
		private int currentLvl;

		public void NextLvl(){
			currentLvl++;
		}

		public bool GameEnded(){
			return currentLvl == 9;
		}

		public int CurrentLvl(){
			return currentLvl;
		}

		public NeighbourhoodActivityModel() {
			currentLvl = 0;
			MetricsController.GetController().GameStart();

		}

		public void Correct() {
			LogAnswer(true);
		}

		public void Wrong(){
			LogAnswer(false);
		}
	}
}