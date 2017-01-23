using System;
using UnityEngine.UI;

namespace Assets.Scripts.Games.NeighbourhoodActivity {
	public class NeighbourhoodActivityView : LevelView {
		public Image upperBoard;
		public Button soundBtn;

		private NeighbourhoodActivityModel model;

		public void Start(){
			model = new NeighbourhoodActivityModel();
			Begin();
		}

		public void Begin(){
			ShowExplanation();

		}

		public void SoundClick(){
			
		}

		override public void Next(bool first = false){
			if(!first){
				SetCurrentLevel(false);
				model.NextLvl();
			}

			if(model.GameEnded()) EndGame(60,0,1250);
			else SetCurrentLevel(true);

			SoundClick();
		}



		private void SetCurrentLevel(bool enabled) {
			
		}

		public void ClickCorrect(){
			ShowRightAnswerAnimation ();
			model.Correct();
		}

		public void ClickWrong(){
			ShowWrongAnswerAnimation ();
			model.Wrong();
		}
	}
}