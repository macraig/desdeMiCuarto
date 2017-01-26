using System;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Sound;

namespace Assets.Scripts.Games.NeighbourhoodActivity {
	public class NeighbourhoodActivityView : LevelView {
		public Text upperBoard;
		public Button soundBtn, okButton;
		public List<Text> refTexts;
		private NeighbourhoodSlot takenSlot;
		private NeighbourhoodDragger takenDragger;
		public List<NeighbourhoodDragger> viewChoices;
		public List<Image> viewGrid, refImages;
		public Sprite baseTileSprite;

		private NeighbourhoodActivityModel model;

		public void Start(){
			model = new NeighbourhoodActivityModel();
			Begin();
		}

		public void Begin(){
			ShowExplanation();
			SetGrid(model.GetGrid());

			//create levels after setting the initial grid.
			model.CreateLevels();
		}

		public void SoundClick(){
			soundBtn.interactable = false;
			SoundController.GetController().ConcatenateAudios(model.GetAudios(), EndSoundMethod);
		}

		public void EndSoundMethod(){
			soundBtn.interactable = true;
		}

		override public void Next(bool first = false){
			if(!first) model.NextLvl();

//			ActivateDraggers (true);
		

			if (model.GameEnded ()) {
				EndGame (60, 0, 1250);

			}else{
				SetCurrentLevel();
				SoundClick();
			}


		}

		private void SetCurrentLevel() {
			List<Building> choices = model.GetChoices ();
			SetChoices(choices);
			SetReferences (choices);
			upperBoard.text = model.GetText();
		}

		void SetChoices(List<Building> choices) {
			for(int i = 0; i < choices.Count; i++) {
				viewChoices[i].GetComponent<Image>().sprite = model.GetSprite(choices[i]);
			}
		}

		void SetReferences(List<Building> choices) {
			for(int i = 0; i < choices.Count; i++) {
				refImages[i].sprite = model.GetReferenceSprite(choices[i]);
				refTexts [i].text = model.GetBuildingTextName (choices[i]);
			}
		}

		void SetGrid(List<Building> grid) {
			for(int i = 0; i < grid.Count; i++) {
				if(grid[i] != null && !grid[i].IsStreet()) {
					viewGrid[i].sprite = model.GetSprite(grid[i]);
					DisableSlot (viewGrid[i].GetComponent<NeighbourhoodSlot>());

				}
			}
		}

		//Le saca los componentes de interactividad a los slots que ya tienen edificio
		void DisableSlot (NeighbourhoodSlot slot)
		{
			slot.GetComponent<NeighbourhoodSlot> ().enabled=false;

		}

		//ESTO SOLO ES CUANDO CAES EN UN SLOT, NO AFUERA
		public void Dropped(NeighbourhoodDragger dragger, NeighbourhoodSlot slot, int row, int column) {
			
			if(IsCorrect(dragger, slot, row, column)){
				model.SetCorrect (true);
			} else {
				model.SetCorrect (false);
			}
			slot.GetComponent<Image>().sprite = dragger.GetComponent<Image>().sprite;
			dragger.SetPosition (slot.transform.position);
			dragger.GetComponent<Button> ().interactable = true;
			takenDragger = dragger;
			if (takenSlot) {
					ClearTakenSlot ();
			}else{
				ActivateDraggers (dragger,false);
			}
			takenSlot = slot;
			okButton.interactable = true;

		}

		bool IsCorrect(NeighbourhoodDragger dragger, NeighbourhoodSlot slot, int row, int column) {
			if(!model.IsCorrectDragger(dragger.GetComponent<Image>().sprite))
				return false;

			if(!model.IsCorrectSlot(row, column))
				return false;

			return true;
		}

		public void OkClick(){
			
			if (model.IsCorrect ()) {
				SoundController.GetController ().SetConcatenatingAudios (false);
				soundBtn.interactable = true;
				ShowRightAnswerAnimation ();
				model.Correct();
				DisableSlot (takenSlot);
				takenSlot = null;

			} else {
				SoundController.GetController ().SetConcatenatingAudios (false);
				soundBtn.interactable = true;
				ShowWrongAnswerAnimation ();
				model.Wrong();
				ActivateDraggers (takenDragger,true);
				ClearTakenSlot ();
			}

			okButton.interactable = false;
		}

		public void ClearTakenSlot(){
			if(takenSlot)
				takenSlot.GetComponent<Image> ().sprite = baseTileSprite;
		}

		public void OnSelectedSlotClick(NeighbourhoodDragger dragger){
			SoundController.GetController ().PlayDropSound ();
			ClearTakenSlot ();
			dragger.ReturnToOriginalPosition ();
			ActivateDraggers (takenDragger,true);
		}

		public void ActivateDraggers(NeighbourhoodDragger dragger, bool activate){
			for(int i = 0; i < viewChoices.Count; i++) {
				if (dragger != viewChoices [i]) {
					viewChoices [i].GetComponent<NeighbourhoodDragger> ().enabled = activate;
					viewChoices [i].GetComponent<Button> ().interactable = activate;
				}
			}
		}
	}
}