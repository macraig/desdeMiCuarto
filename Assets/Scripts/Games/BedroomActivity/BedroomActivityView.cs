using System;
using Assets.Scripts.App;
using UnityEngine.UI;
using UnityEngine;
using SimpleJSON;

namespace Assets.Scripts.Games.BedroomActivity {
	public class BedroomActivityView : LevelView {
		public Image upperBoard;
		public Button soundBtn;

		private Sprite[] boards, carpets;
		private JSONArray lvls;
		private int currentLvl;

		public void Go(){
			RemovePreviousLevel();
			currentLvl++;
			if(currentLvl == lvls.Count - 1) EndGame();
			else SetCurrentLevel();
		}

		private void EndGame() {
			
		}

		private void RemovePreviousLevel() {
			
		}

		private void SetCurrentLevel() {
			JSONClass lvl = lvls[currentLvl].AsObject;

			switch((StageType) Enum.Parse(typeof(StageType), lvl["type"].Value)){
			case StageType.CLICK:
				break;
			}
		}
	}
}