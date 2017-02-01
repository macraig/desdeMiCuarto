using System;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Sound;

namespace Assets.Scripts.Games.ClassroomActivity {
	public class ClassroomActivityView : LevelView {
		

		public Image upperBoard;

		private Sprite[] boards;
		private ClassroomActivityModel model;

		public void Start(){
			model = ClassroomActivityModel.StartFromJson(JSON.Parse(Resources.Load<TextAsset>("Jsons/classroom").text).AsObject["levels"].AsArray);
			boards = Resources.LoadAll<Sprite>("Sprites/ClassroomActivity/consignas");
			ShowExplanation ();
		}



		override public void RestartGame(){
			base.RestartGame ();
			SetCurrentLevel (false);
			model = ClassroomActivityModel.StartFromJson(JSON.Parse(Resources.Load<TextAsset>("Jsons/classroom").text).AsObject["levels"].AsArray);
			ShowExplanation ();
		}

		public void SoundClick(){
			SoundController.GetController().PlayClip(Resources.Load<AudioClip>("Audio/ClassroomActivity/Consignas/" + model.CurrentLvl()));
		}

		override public void Next(bool first = false){
			if(!first){
				model.NextLvl();
			}

			if (model.GameEnded ()) {
				EndGame (60, 0, 1250);
			} else {
				SetCurrentLevel (true);
				SoundClick();
			}

		}

		private void SetCurrentLevel(bool enabled) {
			model.SetCurrentLevel(this, enabled);

			upperBoard.sprite = boards[model.CurrentLvl()];
		}

		public void ClickCorrect(){
			SoundController.GetController ().PlayClip (model.GetLevelSound ());
			SetCurrentLevel(false);
			EnableComponents (false);
			Invoke ("ShowRightAnswerAnimation", 0.5f);
			model.Correct();
		}

		public void ClickWrong(){
			SetCurrentLevel(false);
			EnableComponents (false);
			ShowWrongAnswerAnimation ();
			model.Wrong();
		}

		override public void OnWrongAnimationEnd(){
			base.OnWrongAnimationEnd ();
			SetCurrentLevel(true);
		}
		
	}
}