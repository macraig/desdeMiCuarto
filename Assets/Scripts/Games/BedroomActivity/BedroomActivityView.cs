﻿using System;
using Assets.Scripts.App;
using Assets.Scripts.Games;
using UnityEngine.UI;
using UnityEngine;
using SimpleJSON;
using Assets.Scripts.Common.Dragger;

namespace Assets.Scripts.Games.BedroomActivity {
	public class BedroomActivityView : DraggerView {
		public Image upperBoard;
		public Button soundBtn;

		private Sprite[] boards;
		private BedroomActivityModel model;

		public void Start(){
			model = BedroomActivityModel.StartFromJson(JSON.Parse(Resources.Load<TextAsset>("BedroomActivity/bedroom").text).AsObject["levels"].AsArray);
			boards = Resources.LoadAll<Sprite>("Sprites/BedroomActivity/consignas");
			Begin();
		}

		public void Begin(){
			ShowExplanation();
			Next(true);
		}

		public void Next(bool first = false){
			if(!first){
				SetCurrentLevel(false);
				model.NextLvl();
			}

			if(model.GameEnded()) EndGame();
			else SetCurrentLevel(true);
		}

		private void EndGame() {
			
		}

		private void SetCurrentLevel(bool enabled) {
			model.SetCurrentLevel(enabled);

			upperBoard.sprite = boards[model.CurrentLvl()];
		}

		public override void Dropped(DraggerHandler dropped, DraggerSlot where) {
			//Check if it's over.
			Next();
		}

		public void MuebleEnter(){
			if(DraggerHandler.itemBeingDragged != null){
				//change mueble frame.
			}
		}

		public void MuebleLeave(){
			//Change mueble frame.
		}

		public override bool CanDropInSlot(DraggerHandler dropper, DraggerSlot slot) {
			return model.CanDropInSlot(slot);
		}

		public void ClickTarget(GameObject target){
			target.SetActive(false);
			if(CheckIfFinished()) Next();
		}

		bool CheckIfFinished() {
			return model.ClickFinished();
		}

		public void ClickCorrect(){
			Next();
		}

		public void ClickWrong(){

		}

		public override void RestartGame(){ }
	}
}