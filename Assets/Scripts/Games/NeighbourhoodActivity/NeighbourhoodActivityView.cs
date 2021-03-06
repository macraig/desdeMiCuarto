﻿using System;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Sound;

namespace Assets.Scripts.Games.NeighbourhoodActivity {
	public class NeighbourhoodActivityView : LevelView {
		public Text upperBoard;
		public Button okButton;
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
			okButton.interactable = false;
			//create levels after setting the initial grid.
			model.CreateLevels();
		}

		override public  void RestartGame(){
			base.RestartGame ();
			ResetGrid (model.GetGrid());
			Start ();

		}

		public void SoundClick(){
			soundBtn.interactable = false;
			SoundController.GetController().ConcatenateAudios(model.GetAudios(), EndSoundMethod);
		}

		override public void ShowInGameMenu(){
			base.ShowInGameMenu ();
			SoundController.GetController ().SetConcatenatingAudios (false);
			soundBtn.interactable = true;
		}

		public void EndSoundMethod(){
			soundBtn.interactable = true;
		}

		override public void Next(bool first = false){
			if(!first) model.NextLvl();
		

			if (model.GameEnded ()) {
				EndGame (0, 0, 800);

			} else {
				
				SetCurrentLevel ();
				SoundClick ();
				ActivateDraggers (takenDragger,true);
				if (takenDragger) {
					takenDragger.ReturnToOriginalPosition ();
					takenDragger = null;
				}
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
					EnableSlot (viewGrid[i].GetComponent<NeighbourhoodSlot>(),false);

				}
			}
		}

		void ResetGrid(List<Building> grid) {
			for(int i = 0; i < grid.Count; i++) {
				if(grid[i] != null && !grid[i].IsStreet() && grid[i].GetName()!="escuela") {
					viewGrid[i].sprite = baseTileSprite;
					EnableSlot (viewGrid[i].GetComponent<NeighbourhoodSlot>(),true);

				}
			}
		}


		//Le saca los componentes de interactividad a los slots que ya tienen edificio
		void EnableSlot (NeighbourhoodSlot slot,bool enable)
		{
			slot.GetComponent<NeighbourhoodSlot> ().enabled=enable;
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
				EnableComponents (false);
				EnableSlot (takenSlot,false);
				ShowRightAnswerAnimation ();
				model.Correct();
				takenSlot = null;

			} else {
				SoundController.GetController ().SetConcatenatingAudios (false);
				soundBtn.interactable = true;
				EnableComponents (false);
				ShowWrongAnswerAnimation ();
				model.Wrong();
			}

			okButton.interactable = false;
		}

		public void ClearTakenSlot(){
			if(takenSlot)
				takenSlot.GetComponent<Image> ().sprite = baseTileSprite;
		}

		public void OnSelectedSlotClick(NeighbourhoodDragger dragger){
			if (dragger.WasDragged ()) {
				SoundController.GetController ().SetConcatenatingAudios (false);
				soundBtn.interactable = true;
				SoundController.GetController ().PlayDropSound ();
				ClearTakenSlot ();
				dragger.ReturnToOriginalPosition ();
				ActivateDraggers (takenDragger,true);
				takenSlot = null;
				okButton.interactable = false;
			}

		}

		public void ActivateDraggers(NeighbourhoodDragger dragger, bool activate){
			if (dragger) {
				for(int i = 0; i < viewChoices.Count; i++) {
					if (dragger != viewChoices [i]) {
						viewChoices [i].GetComponent<NeighbourhoodDragger> ().enabled = activate;
						viewChoices [i].GetComponent<Button> ().interactable = activate;
					}
				}
			}

		}
	}
}