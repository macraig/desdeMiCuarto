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
		public Material[] roomTextures;

		public GameObject phantomPanel;
		private AudioClip ghostRightSound, ghostWrongSound;
		private int  toggleIndex;

		private HouseActivityModel model;
		private int currentSlot;

		public void Start(){
			model = new HouseActivityModel();
			roomTextures = Resources.LoadAll<Material> ("Sprites/HouseActivity/Materials");
			vacuumSprites = Resources.LoadAll<Sprite> ("Sprites/HouseActivity/aspiradora");
			ghostRightSound = Resources.Load<AudioClip> ("Audio/HouseActivity/boo");
			ghostWrongSound = Resources.Load<AudioClip> ("Audio/HouseActivity/laugh");
			ShowExplanation ();
		}

		override public void Next(bool first = false) {
			if(model.IsFinished()) {
//				5 ESTRELLAS: 0 ERRORES
//				4 ESTRELLAS: 1ERROR
//				3 ESTRELLAS: 2 o 3 ERRORES
//				2 ESTRELLAS: 4 o 5 ERRORES
//				1 ESTRELLA: MAS DE 5 ERRORES
				phantomPanel.SetActive(false);
				EndGame(60,0,1250);
				return;
			}

			okBtn.interactable = false;
			shootToggle.interactable = false;
			phantomPanel.SetActive(false);
			model.SetSector();
			board.sprite = model.BoardSprite();
			SoundClick();
		}

		public void SoundClick(){
			SoundController.GetController().PlayClip(model.BoardClip());
		}

		public void HouseSectorSelected(){
			SoundController.GetController ().PlayDropSound ();
			okBtn.interactable = true;
		}

		public void OkClick(){
			toggleIndex = rooms.FindIndex ((Toggle t) => t.isOn);
			bool correct = model.IsCorrectSector(toggleIndex);

			model.LogAnswer(correct);
			rooms.ForEach((Toggle t) => {if(t.isOn) t.isOn=false;});
			if(correct) {
				ShowRightGhostAnimation ();

			} else {
				ShowWrongGhostAnimation ();

			}

		}

		// PHANTOM SCREEN ***************************************************************************************************

		public List<GameObject> arrows, ghosts, crosshairs;
		public List<Image> lifeGhosts;
		public Toggle shootToggle, left, right, up, down;
		public Image phantomBackground, vacuumCleaner;
		private Sprite[] vacuumSprites;
		//Right and wrong animations
		public GameObject rightGhostAnimation;
		public GameObject wrongGhostAnimation;


		private int correctGhost;
		private bool cleaning;

		void StartPhantomScreen(int roomIndex) {
			phantomPanel.SetActive(true);
			phantomBackground.material = roomTextures[roomIndex];
			currentSlot = 4;

			correctGhost = Randomizer.New(8).ExcludeNumbers(new List<int> { 4 }).Next();
			ghosts[correctGhost].SetActive(true);
			crosshairs[currentSlot].SetActive(true);

			shootToggle.isOn = false;
		}



		public void ShootToggle(){
			if(!shootToggle.isOn) return;
			shootToggle.interactable = false;
			if (vacuumCleaner.sprite == vacuumSprites [1])
				vacuumCleaner.sprite = vacuumSprites [0];
			else if(vacuumCleaner.sprite == vacuumSprites[3])
				vacuumCleaner.sprite = vacuumSprites [2];
			else if(vacuumCleaner.sprite == vacuumSprites[5])
				vacuumCleaner.sprite = vacuumSprites [4];

			bool correct = currentSlot == correctGhost;
			model.LogAnswer(correct);

			lifeGhosts[model.GetStage()].gameObject.SetActive(true);
			lifeGhosts[model.GetStage()].GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("Sprites/HouseActivity/ghost")[correct ? 2 : 1];

			model.NextStage();
			if(correct){
				ShowRightAnswerAnimation ();
			} else {
				ShowWrongAnswerAnimation ();
			}

		}

		override public void OnWrongAnimationEnd(){
			Next ();
			cleaning = true;
			CleanUI();
			cleaning = false;
			shootToggle.interactable = true;
		}



		override public void OnRightAnimationEnd(){
			Next ();
			cleaning = true;
			CleanUI ();
			cleaning = false;
			shootToggle.interactable = true;

		}

		void CleanUI() {
			vacuumCleaner.sprite = vacuumSprites [1];
			ghosts.ForEach((GameObject g) => {if(g != null) g.SetActive(false);});
			arrows.ForEach((GameObject g) => g.SetActive(false));
			crosshairs.ForEach((GameObject g) => g.SetActive(false));
			shootToggle.isOn = false;
			shootToggle.interactable = false;
			CleanToggles(left, right, up, down);
		}

		public void CleanToggles(params Toggle[] objects){
			foreach(var o in objects) {
				o.enabled = true;
				o.interactable = true;
				o.isOn = false;
			}
		}

		public void DisableDirection(Toggle disable){
			if (!cleaning) {
				disable.interactable = false;
				shootToggle.interactable = true;
			}
		}

		public void ClickDirection(Toggle dir){
			if(cleaning) return;

			PlaySoundClick();
			dir.enabled = false;
		}

		//left -1, right +1, up -3, down +3
		public void MoveSlots(int move){
			if(cleaning) return;
			crosshairs[currentSlot].SetActive(false);

			currentSlot += move;

			crosshairs[currentSlot].SetActive(true);
		}

		public void ShowArrow(string dir){
			if(cleaning) return;

			if (dir == "LEFT")
				vacuumCleaner.sprite = vacuumSprites [3];
			else if (dir == "RIGHT")
				vacuumCleaner.sprite = vacuumSprites [5];


			switch (dir) {
			case "UP":
				if (left.IsInteractable () && right.IsInteractable ())
					arrows [0].SetActive (true);
				else if (left.isOn)
					arrows [7].SetActive (true);
				else
					arrows [10].SetActive (true);
				break;
			case "DOWN":
				if (left.IsInteractable () && right.IsInteractable ())
					arrows [3].SetActive (true);
				else if (left.isOn)
					arrows [8].SetActive (true);
				else
					arrows [11].SetActive (true);
				break;
			case "LEFT":
				if (up.IsInteractable () && down.IsInteractable ())
					arrows [6].SetActive (true);
				else if (up.isOn)
					arrows [1].SetActive (true);
				else
					arrows [4].SetActive (true);
				break;
			case "RIGHT":
				if (up.IsInteractable () && down.IsInteractable ())
					arrows [9].SetActive (true);
				else if (up.isOn)
					arrows [2].SetActive (true);
				else
					arrows [5].SetActive (true);
				break;
			}

		}

		internal void ShowRightGhostAnimation(){
			SoundController.GetController ().PlayClip (ghostRightSound);
			rightGhostAnimation.transform.SetAsLastSibling ();
			rightGhostAnimation.GetComponent<GhostAnimationScript>().ShowAnimation();

		}

		internal void ShowWrongGhostAnimation(){
			SoundController.GetController ().PlayClip (ghostWrongSound);
			wrongGhostAnimation.transform.SetAsLastSibling ();
			wrongGhostAnimation.GetComponent<GhostAnimationScript>().ShowAnimation();

		}


		 public void OnRightGhostAnimationEnd(){
			StartPhantomScreen(toggleIndex);
		}

		 public void OnWrongGhostAnimationEnd(){
			Next ();
		}

	}
}