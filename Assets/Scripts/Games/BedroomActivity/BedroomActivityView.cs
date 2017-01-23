using System;
using Assets.Scripts.App;
using Assets.Scripts.Games;
using Assets.Scripts.Sound;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using SimpleJSON;
using Assets.Scripts.Common.Dragger;

namespace Assets.Scripts.Games.BedroomActivity {
	public class BedroomActivityView : DraggerView {
		public Image upperBoard;
		public Button soundBtn, muebleButton, carpetPanelOkButton, carpet, photoButton;
		public GameObject carpetPanel, photoPanel;

		private Sprite[] boards;
		private AudioClip[] instructions;
		private BedroomActivityModel model;

		public void Start(){
			model = BedroomActivityModel.StartFromJson(JSON.Parse(Resources.Load<TextAsset>("BedroomActivity/bedroom").text).AsObject["levels"].AsArray);
			boards = Resources.LoadAll<Sprite>("Sprites/BedroomActivity/consignas");
			instructions = Resources.LoadAll<AudioClip>("Audio/BedroomActivity/Consignas");
			Begin();
		}

		public void Begin(){
			ShowExplanation();

		}

		override public void Next(bool first = false){
			if(!first){
				SetCurrentLevel(false);
				if(model.CurrentLvl()==5) GameObject.Find("baulToggle").GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("Sprites/BedroomActivity/baul")[0];
				model.NextLvl();
			}

			if(model.GameEnded()) ShowPhotoButton();
			else SetCurrentLevel(true);
		}

		public void SelectCarpet(){
			SoundController.GetController ().PlaySwitchSound ();
			carpetPanelOkButton.interactable = true;
		}

		public void CarpetOk(){
			//Desactivar el eventTrigger del mueble. Lo pongo acá porque no sé donde más ponerlo :)
			GameObject.Find("muebleButton").GetComponent<EventTrigger>().enabled = false;

			if(GameObject.Find("blueToggle").GetComponent<Toggle>().isOn){
				model.Correct();
				carpet.image.sprite = Resources.LoadAll<Sprite>("Sprites/BedroomActivity/alfombra")[4];

				ShowRightAnswerAnimation ();
			} else {
				ShowWrongAnswerAnimation ();
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
			SoundController.GetController ().PlayClip (Resources.Load<AudioClip> ("Audio/BedroomActivity/SFX/takePicture"));
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
			SoundController.GetController().PlayClip(instructions[model.CurrentLvl()]);

		}

		public void OnSoundButtonClick(){
			SoundController.GetController().PlayClip(instructions[model.CurrentLvl()]);
		}

		public override void Dropped(DraggerHandler dropped, DraggerSlot where) {
			if (model.TargetIsCorrect (where)) {
				model.Correct ();
				if(!dropped.ActiveOnDrop()) dropped.gameObject.SetActive (false);
				if (model.GetLevelSound () != null)
					SoundController.GetController ().PlayClip (model.GetLevelSound ());
				if (model.IsLvlDone ()) {
					Invoke ("ShowRightAnswerAnimation", 1f);
				} 
			} else {
				model.Wrong ();
				ShowWrongAnswerAnimation ();
			}
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
			if(model.GetLevelSound()!=null) SoundController.GetController ().PlayClip (model.GetLevelSound());
			if (model.IsLvlDone ()) {
				Invoke("ShowRightAnswerAnimation",1f);
			}
				
		}

		public void ClickToggle(string togglePath){
			model.Correct();
			Sprite[] spr = Resources.LoadAll<Sprite>(togglePath);

			model.SetToggle(spr);

			if (model.IsLvlDone ()) {
				if(model.GetLevelSound()!=null) SoundController.GetController ().PlayClip (model.GetLevelSound());
				Invoke("ShowRightAnswerAnimation",1f);
			}
		}

		public void ClickWrong(){
			ShowWrongAnswerAnimation ();
			model.Wrong();
		}


	}
}