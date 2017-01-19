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
		public Button soundBtn, muebleButton, carpetPanelOkButton, carpet, photoButton;
		public GameObject carpetPanel, photoPanel;

		private Sprite[] boards;
		private BedroomActivityModel model;

		public void Start(){
			model = BedroomActivityModel.StartFromJson(JSON.Parse(Resources.Load<TextAsset>("BedroomActivity/bedroom").text).AsObject["levels"].AsArray);
			boards = Resources.LoadAll<Sprite>("Sprites/BedroomActivity/consignas");
			Begin();
		}

		public void Begin(){
			ShowRightAnswerAnimation ();
			//ShowExplanation();
			Next(true);
		}

		public void Next(bool first = false){
			if(!first){
				SetCurrentLevel(false);
				model.NextLvl();
			}

			if(model.GameEnded()) ShowPhotoButton();
			else SetCurrentLevel(true);
		}

		public void SelectCarpet(){
			//PlaySoundSwitch();
			carpetPanelOkButton.interactable = true;
		}

		public void CarpetOk(){
			if(GameObject.Find("blueToggle").GetComponent<Toggle>().isOn){
				model.Correct();
				PlayRightSound();

				carpet.image.sprite = Resources.LoadAll<Sprite>("Sprites/BedroomActivity/alfombra")[4];

				Next();
			} else {
				PlayWrongSound();
				model.Wrong();
			}
		}

		private void ShowPhotoButton() {
			Debug.Log("End game");
			photoButton.gameObject.SetActive(true);
			upperBoard.sprite = boards[boards.Length - 1];
		}

		public void PhotoClick(){
			photoButton.gameObject.SetActive(false);
			photoPanel.SetActive(true);
			//playPhotoSound();
		}

		public void OnPhotoPanelClick(){
//			5 ESTRELLAS: 0 ERRORES
//			4 ESTRELLAS: 1 ERROR
//			3 ESTRELLAS: 2 O 3 ERRORES
//			2 ESTRELLAS: 4 ERRORES
//			1 ESTRELLA: MAS DE 4 ERRORES
			EndGame (6000,0,1600);
		}

		private void SetCurrentLevel(bool enabled) {
			if(!model.CurrentCarpet())
				model.SetCurrentLevel(enabled);
			else
				carpetPanel.SetActive(enabled);

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


	}
}