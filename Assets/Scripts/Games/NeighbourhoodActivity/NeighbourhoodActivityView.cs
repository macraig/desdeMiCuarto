﻿using System;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Sound;

namespace Assets.Scripts.Games.NeighbourhoodActivity {
	public class NeighbourhoodActivityView : LevelView {
		public Text upperBoard;
		public Button soundBtn;
		public List<Text> refTexts;
		public List<Image> viewGrid, viewChoices,refImages;

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
			SoundController.GetController().ConcatenateAudios(model.GetAudios(), EndSoundMethod);
		}

		public void EndSoundMethod(){
			
		}

		override public void Next(bool first = false){
			if(!first) model.NextLvl();

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
				viewChoices[i].sprite = model.GetSprite(choices[i]);
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
					Debug.Log("BUILDING: " + grid[i].GetName());
				}
			}
		}

		//validate on drop
		public void Dropped(NeighbourhoodDragger dragger, NeighbourhoodSlot slot, int row, int column) {
			if(IsCorrect(dragger, slot, row, column)){
				slot.GetComponent<Image>().sprite = dragger.GetComponent<Image>().sprite;
				SoundController.GetController ().SetConcatenatingAudios (false);
				ShowRightAnswerAnimation ();
				model.Correct();
			} else {
				SoundController.GetController ().SetConcatenatingAudios (false);
				ShowWrongAnswerAnimation ();
				model.Wrong();
			}
		}

		bool IsCorrect(NeighbourhoodDragger dragger, NeighbourhoodSlot slot, int row, int column) {
			if(!model.IsCorrectDragger(dragger.GetComponent<Image>().sprite))
				return false;

			if(!model.IsCorrectSlot(row, column))
				return false;

			return true;
		}
	}
}