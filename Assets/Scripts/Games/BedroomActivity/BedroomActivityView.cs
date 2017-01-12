using System;
using Assets.Scripts.App;
using Assets.Scripts.Games;
using UnityEngine.UI;
using UnityEngine;
using SimpleJSON;
using Assets.Scripts.Common.Dragger;

namespace Assets.Scripts.Games.BedroomActivity {
	public class BedroomActivityView : DraggerView {
		public Image upperBoard;
		public Button soundBtn, muebleButton, carpetPanelOkButton, carpet;
		public GameObject carpetPanel;

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

		public void SelectCarpet(){
			//PlaySoundSwitch();
			carpetPanelOkButton.interactable = true;
		}

		public void CarpetOk(){
			if(GameObject.Find("blueToggle").GetComponent<Toggle>().isOn){
				model.Correct();
				carpetPanel.SetActive(false);
				PlayRightSound();

				carpet.image.sprite = Resources.LoadAll<Sprite>("Sprites/BedroomActivity/alfombra")[4];

				Next();
			} else {
				PlayWrongSound();
				model.Wrong();
			}
		}

		private void EndGame() {
			
		}

		private void SetCurrentLevel(bool enabled) {
			if(!model.CurrentCarpet())
				model.SetCurrentLevel(enabled);
			else
				carpetPanel.SetActive(true);

			upperBoard.sprite = boards[model.CurrentLvl()];
		}

		public override void Dropped(DraggerHandler dropped, DraggerSlot where) {
			model.Correct();
			if(model.IsLvlDone()) Next();
		}

		public void MuebleEnter(){
			if(DraggerHandler.itemBeingDragged != null){
				muebleButton.image.sprite = Resources.LoadAll<Sprite>("Sprites/BedroomActivity/mueble")[1];
			}
		}

		public void MuebleLeave(){
			muebleButton.image.sprite = Resources.LoadAll<Sprite>("Sprites/BedroomActivity/mueble")[0];
		}

		public override bool CanDropInSlot(DraggerHandler dropper, DraggerSlot slot) {
			return model.CanDropInSlot(slot);
		}

		public void ClickTarget(GameObject target){
			target.SetActive(false);
			model.Correct();
			if(model.IsLvlDone()) Next();
		}

		public void ClickToggle(string togglePath){
			model.Correct();
			Sprite[] spr = Resources.LoadAll<Sprite>(togglePath);

			model.SetToggle(spr);

			if(model.IsLvlDone()) Next();
		}

		public void ClickWrong(){
			model.Wrong();
		}

		public override void RestartGame(){ }
	}
}