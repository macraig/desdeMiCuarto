using System;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Sound;

namespace Assets.Scripts.Games.ClassroomActivity {
	public class ClassroomActivityView : LevelView {
		#region implemented abstract members of LevelView
		public override void RestartGame() { }
		#endregion

		public Image upperBoard;
		public Button soundBtn;

		private Sprite[] boards;
		private ClassroomActivityModel model;

		public void Start(){
			model = ClassroomActivityModel.StartFromJson(JSON.Parse(Resources.Load<TextAsset>("Jsons/classroom").text).AsObject["levels"].AsArray);
			boards = Resources.LoadAll<Sprite>("Sprites/ClassroomActivity/consignas");
			Begin();
		}

		public void Begin(){
			ShowExplanation();
			Next(true);
		}

		public void SoundClick(){
			SoundController.GetController().PlayClip(Resources.Load<AudioClip>("Audio/ClassroomActivity/Consignas/" + model.CurrentLvl()));
		}

		public void Next(bool first = false){
			if(!first){
				SetCurrentLevel(false);
				model.NextLvl();
			}

			if(model.GameEnded()) EndGame();
			else SetCurrentLevel(true);

			SoundClick();
		}

		private void EndGame() {
			Debug.Log("End game");
		}

		private void SetCurrentLevel(bool enabled) {
			model.SetCurrentLevel(this, enabled);

			upperBoard.sprite = boards[model.CurrentLvl()];
		}

		public void ClickCorrect(){
			PlayRightSound();
			model.Correct();
			Next();
		}

		public void ClickWrong(){
			PlayWrongSound();
			model.Wrong();
		}
		
	}
}