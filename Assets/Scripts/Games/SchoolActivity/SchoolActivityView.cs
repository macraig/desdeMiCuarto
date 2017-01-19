using System;
using System.Collections.Generic;
using UnityEngine.UI;

using UnityEngine;
using Assets.Scripts.Common;
using Assets.Scripts.Sound;

namespace Assets.Scripts.Games.SchoolActivity {
	public class SchoolActivityView : LevelView {
		public List<Toggle> rooms;
		public Button okBtn;
		public List<GameObject> directionsListBtns;
		public Image board;


		private SchoolActivityModel model;
		private int currentSlot;

		public void Start(){
			ShowExplanation();
			model = new SchoolActivityModel();

			Next();
		}

		override public void Next(bool first = false) {
			if(model.IsFinished()) {
				//				5 ESTRELLAS: 0 ERRORES
				//				4 ESTRELLAS: 1ERROR
				//				3 ESTRELLAS: 2 o 3 ERRORES
				//				2 ESTRELLAS: 4 o 5 ERRORES
				//				1 ESTRELLA: MAS DE 5 ERRORES
				EndGame(60,0,1250);
				return;
			}
			CleanUI ();
			okBtn.interactable = false;

			model.SetSector();
			board.sprite = model.BoardSprite();
			SoundClick();
		}



		public void SoundClick(){
			SoundController.GetController().PlayClip(model.BoardClip());
		}

		public void HouseSectorSelected(){
			okBtn.interactable = true;
		}

		public void OkClick(){
			bool correct = model.IsCorrectSector(rooms.FindIndex((Toggle t) => t.isOn));
			model.LogAnswer(correct);
			if(correct) {
				PlayRightSound();
				StartPhantomScreen();
			} else {
				PlayWrongSound();
			}
		}

		// PHANTOM SCREEN ***************************************************************************************************

		public Toggle left, right, up, down;

		private bool cleaning;

		void StartPhantomScreen() {
//			currentSlot = 4;

//			correctGhost = Randomizer.New(8).ExcludeNumbers(new List<int> { 4 }).Next();
//			ghosts[correctGhost].SetActive(true);
//			crosshairs[currentSlot].SetActive(true);

//			shootToggle.isOn = false;
		}

		public void ShootToggle(){
//			if(!shootToggle.isOn) return;

//			bool correct = currentSlot == correctGhost;
//			model.LogAnswer(correct);

//			lifeGhosts[model.GetStage()].gameObject.SetActive(true);
//			lifeGhosts[model.GetStage()].GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("Sprites/HouseActivity/ghost")[correct ? 2 : 1];

//			if(correct){
//				PlayRightSound();
//			} else {
//				PlayWrongSound();
//			}

			cleaning = true;
			CleanUI();
			cleaning = false;

			model.NextStage();
			Next();
		}

		void CleanUI() {
			directionsListBtns.ForEach((GameObject g) => g.SetActive(false));

		}

	
		public void OnClickDirection(string dir){
			PlaySoundClick();
			AddInstruction (dir);
		}


		public void OnClickDirectionImage(Button dirBtn){
			SoundController.GetController ().PlayDropSound ();
			dirBtn.gameObject.SetActive (false);
		}


		void AddInstruction (string dir)
		{
//			model.AddInstruction (dir);
			foreach (GameObject dirImg in directionsListBtns) {
				if (!dirImg.activeSelf) {
					dirImg.SetActive (true);
					dirImg.GetComponent<Image>().sprite = model.DirectionSprite (model.ParseToDirection(dir));
					break;
				}
			}
		}
	}
}