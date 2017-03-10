using System;
using Assets.Scripts.App;
using Assets.Scripts.Games;
using Assets.Scripts.Sound;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;
using Assets.Scripts.Common.Dragger;

namespace Assets.Scripts.Games.BedroomActivity {
	public class BedroomActivityView : DraggerView {
		public Image upperBoard;
		public Button muebleButton, carpetPanelOkButton, carpet, photoButton;
		public GameObject carpetPanel, photoPanel;
		public ToggleGroup carpetToggleGroup;

		private Sprite[] boards;
		private AudioClip[] instructions;
		private BedroomActivityModel model;

		public List<GameObject> nonDraggables;
		public List<GameObject> draggables;
		private List<Vector2> draggablesPositions;

		public void Start(){
			model = BedroomActivityModel.StartFromJson(JSON.Parse(Resources.Load<TextAsset>("BedroomActivity/bedroom").text).AsObject["levels"].AsArray);
			boards = Resources.LoadAll<Sprite>("Sprites/BedroomActivity/consignas");
			instructions = Resources.LoadAll<AudioClip>("Audio/BedroomActivity/Consignas");
			draggablesPositions = new List<Vector2> ();

			foreach(GameObject draggable in draggables){
				draggablesPositions.Add (new Vector2 (draggable.transform.position.x,draggable.transform.position.y));
			}

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
			carpetPanelOkButton.interactable = carpetToggleGroup.AnyTogglesOn();
			SoundController.GetController ().PlaySwitchSound ();

		}

		public void CarpetOk(){
			GameObject.Find("muebleButton").GetComponent<EventTrigger>().enabled = false;

			if(GameObject.Find("blueToggle").GetComponent<Toggle>().isOn){
				carpetPanelOkButton.interactable = false;
				carpetToggleGroup.SetAllTogglesOff ();
				model.Correct();
				carpet.image.sprite = Resources.LoadAll<Sprite>("Sprites/BedroomActivity/alfombra")[4];
				EnableComponents (false);
				ShowRightAnswerAnimation ();
			} else {
				EnableComponents (false);
				ShowWrongAnswerAnimation ();
				model.Wrong();
			}
		}

		private void ShowPhotoButton() {
			photoButton.gameObject.SetActive(true);
			upperBoard.sprite = boards[boards.Length - 1];
			SoundController.GetController().PlayClip(instructions[instructions.Length - 1]);
		}

		public void PhotoClick(){
			photoButton.gameObject.SetActive(false);
			photoPanel.transform.SetAsLastSibling ();
			photoPanel.SetActive(true);
			SoundController.GetController ().PlayClip (Resources.Load<AudioClip> ("Audio/BedroomActivity/SFX/takePicture"));
		}

		public void OnPhotoPanelClick(){
//			5 ESTRELLAS: 0 ERRORES
//			4 ESTRELLAS: 1 ERROR
//			3 ESTRELLAS: 2 O 3 ERRORES
//			2 ESTRELLAS: 4 ERRORES
//			1 ESTRELLA: MAS DE 4 ERRORES
			EndGame (0,0,800);
		}

		private void SetCurrentLevel(bool enabled) {
			if (!model.CurrentCarpet ()) {
				model.SetCurrentLevel (enabled);
			} else {
				carpetPanel.transform.SetAsLastSibling ();
				carpetPanel.SetActive (enabled);
			}

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
					EnableComponents (false);
					Invoke ("ShowRightAnswerAnimation", 1f);
				} 
			} else {
				model.Wrong ();
				EnableComponents (false);
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
				EnableComponents (false);
				Invoke("ShowRightAnswerAnimation",1f);
			}
				
		}

		public void ClickToggle(string togglePath){
			model.Correct();
			Sprite[] spr = Resources.LoadAll<Sprite>(togglePath);

			model.SetToggle(spr);

			if (model.IsLvlDone ()) {
				EnableComponents (false);
				if(model.GetLevelSound()!=null) SoundController.GetController ().PlayClip (model.GetLevelSound());
				Invoke("ShowRightAnswerAnimation",1f);
			}
		}

		public void ClickWrong(){
			EnableComponents (false);
			ShowWrongAnswerAnimation ();
			model.Wrong();
		}

		override public void EnableComponents(bool enable){
			base.EnableComponents (enable);
//			carpetPanelOkButton.interactable = enable;
			model.EnableLevelComponents (enable);

		}

		override public void RestartGame(){
			HideInGameMenu ();

			GameObject.Find ("alfombraButton").GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("Sprites/BedroomActivity/alfombra")[6];
			GameObject.Find("lampToggle").GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("Sprites/BedroomActivity/lamp")[0];

			GameObject mueble = GameObject.Find("muebleButton");
			mueble.GetComponent<EventTrigger> ().enabled = true;
			mueble.GetComponent<Button> ().enabled = false;
			

			GameObject door = GameObject.Find ("doorToggle");
			door.GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("Sprites/BedroomActivity/door")[0];
			door.GetComponent<Button> ().enabled = false;

			GameObject baul = GameObject.Find ("baulToggle");
			baul.GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("Sprites/BedroomActivity/baul")[0];
			baul.GetComponent<Button> ().enabled = false;

			GameObject window = GameObject.Find ("windowToggle");
			window.GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("Sprites/BedroomActivity/window")[0];
			window.GetComponent<Button> ().enabled = false;

			foreach (GameObject go in nonDraggables) {
				go.SetActive (true);
				go.GetComponent<Button> ().enabled = false;
			}

			for (int i=0;i<draggables.Count;i++) {
				draggables[i].SetActive (true);
				draggables[i].transform.position = draggablesPositions[i];
			}
//			model.RestartGame ();
			first = true;
			Start ();

		}


	}
}