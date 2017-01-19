using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Sound;
using SimpleJSON;

namespace Assets.Scripts.Games.PatternsActivity {
	public class PatternsActivityView : LevelView {
		public List<GameObject> leftGrids, rightGrids;

		override public void Next(bool first = false){
			
		}

		public void ExchangeClick(){
			
		}

		public void OkClick(){
			
		}

		//Kjakljdklasjdlkajsdlkasjdklajsdklajsd

		public Image upperBoard;
		public Button soundBtn;

		private Sprite[] boards;
		private PatternsActivityModel model;

		public void Start(){
			model = PatternsActivityModel.StartFromJson(JSON.Parse(Resources.Load<TextAsset>("Jsons/classroom").text).AsObject["levels"].AsArray);
			boards = Resources.LoadAll<Sprite>("Sprites/ClassroomActivity/consignas");
			Begin();
		}

		public void Begin(){
			ShowExplanation();

		}

		public void SoundClick(){
			SoundController.GetController().PlayClip(Resources.Load<AudioClip>("Audio/ClassroomActivity/Consignas/" + model.CurrentLvl()));
		}

		public void Nextt(bool first = false){
			if(!first){
				SetCurrentLevel(false);
				model.NextLvl();
			}

			if(model.GameEnded()) EndGame(60,0,1250);
			else SetCurrentLevel(true);

			SoundClick();
		}



		private void SetCurrentLevel(bool enabled) {
			model.SetCurrentLevel(this, enabled);

			upperBoard.sprite = boards[model.CurrentLvl()];
		}

		public void ClickCorrect(){
			ShowRightAnswerAnimation ();
			model.Correct();
			//			Next();
		}

		public void ClickWrong(){
			ShowWrongAnswerAnimation ();
			model.Wrong();
		}
	}
}