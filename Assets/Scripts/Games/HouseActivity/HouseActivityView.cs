using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Assets.Scripts.Common;
using Assets.Scripts.Sound;

namespace Assets.Scripts.Games.HouseActivity {
	public class HouseActivityView : LevelView {
		public List<Toggle> rooms;
		public Button okBtn;
		public Image board;

		public GameObject phantomPanel;

		private HouseActivityModel model;
		private int currentSlot;

		public void Start(){
			model = new HouseActivityModel();

			Next();
		}

		void Next() {
			if(model.IsFinished()) {
				EndGame();
				return;
			}

			okBtn.interactable = false;
			phantomPanel.SetActive(false);
			model.SetSector();
			board.sprite = model.BoardSprite();
			SoundClick();
		}

		void EndGame() {
			Debug.Log("End game.");
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

		public List<GameObject> arrows, ghosts, crosshairs;
		public List<Image> lifeGhosts;
		public Toggle shootToggle, left, right, up, down;

		private int correctGhost;

		void StartPhantomScreen() {
			phantomPanel.SetActive(true);
			currentSlot = 4;

			correctGhost = Randomizer.New(8).ExcludeNumbers(new List<int> { 4 }).Next();
			ghosts[correctGhost].SetActive(true);

			shootToggle.isOn = false;
		}

		public void ShootToggle(){
			if(!shootToggle.isOn) return;

			bool correct = currentSlot == correctGhost;
			model.LogAnswer(correct);

			lifeGhosts[model.GetStage()].gameObject.SetActive(true);
			lifeGhosts[model.GetStage()].GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("Sprites/HouseActivity/ghost")[correct ? 2 : 1];

			if(correct){
				PlayRightSound();
			} else {
				PlayWrongSound();
			}

			model.NextStage();
			Next();
		}

		public void DisableDirection(Toggle disable){
			disable.interactable = false;
		}

		public void ClickDirection(Toggle dir){
			PlaySoundClick();
			dir.enabled = false;
		}

		//left -1, right +1, up -3, down +3
		public void MoveSlots(int move){
			crosshairs[currentSlot].SetActive(false);

			currentSlot += move;

			crosshairs[currentSlot].SetActive(true);
		}

		#region implemented abstract members of LevelView
		public override void RestartGame() { }
		#endregion
	}
}