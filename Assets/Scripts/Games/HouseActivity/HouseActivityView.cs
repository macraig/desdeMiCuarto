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
		public GameObject board;
		public Material[] roomTextures;

		public GameObject phantomPanel;
		private AudioClip ghostRightSound, ghostWrongSound,vacuumSound;
		private int  toggleIndex;

		private HouseActivityModel model;
		private int currentSlot;
		private bool firstPhantom = true;
		private AudioClip currentSound;

		public void Start(){
			model = new HouseActivityModel();
			roomTextures = Resources.LoadAll<Material> ("Sprites/HouseActivity/Materials");
			vacuumSprites = Resources.LoadAll<Sprite> ("Sprites/HouseActivity/aspiradora");
			ghostRightSound = Resources.Load<AudioClip> ("Audio/HouseActivity/boo");
			ghostWrongSound = Resources.Load<AudioClip> ("Audio/HouseActivity/laugh");
			vacuumSound =  Resources.Load<AudioClip> ("Audio/HouseActivity/aspiradora");

			phantomInstructionSounds = Resources.LoadAll<AudioClip> ("Audio/HouseActivity/PhantomConsignas"); 
			phantomInstructions = new string[]{"¡El fantasma está ahí! Atrápenlo con las flechas. ¡Rápido!","¡Ahí está!","¡Cuidado!","¡Atrápenlo antes de que se vaya!" };
			allToggles = new List<Toggle> (){ left,right,up,down,shootToggle};
			ShowExplanation ();
		}



		override public void Next(bool first = false) {
			if(model.IsFinished()) {
//				5 ESTRELLAS: 0 ERRORES
//				4 ESTRELLAS: 1ERROR
//				3 ESTRELLAS: 2 o 3 ERRORES
//				2 ESTRELLAS: 4 o 5 ERRORES
//				1 ESTRELLA: MAS DE 5 ERRORES
				ActivatePhantomPanel(false);
				EndGame(60,0,1250);
				return;
			}

			okBtn.interactable = false;
			shootToggle.interactable = false;
			ActivatePhantomPanel (false);
			model.SetSector();
			board.GetComponentsInChildren<Image>()[1].sprite = model.BoardSprite();
			currentSound = model.BoardClip ();
			SoundClick();
		}

		public void SoundClick(){
			SoundController.GetController().PlayClip(currentSound);
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
		private List<Toggle> allToggles;
		public Image phantomBackground, vacuumCleaner;
		private Sprite[] vacuumSprites;
		//Right and wrong animations
		public GameObject rightGhostAnimation;
		public GameObject wrongGhostAnimation;

		private string[] phantomInstructions;
		private AudioClip[] phantomInstructionSounds;

		private int correctGhost;
		private bool cleaning;

		void StartPhantomScreen(int roomIndex) {

			ActivatePhantomPanel (true);

			if (firstPhantom) {
				board.GetComponentInChildren<Text> ().text = phantomInstructions[0].ToUpper();
				currentSound = phantomInstructionSounds [0];
				firstPhantom = false;
			} else {
				int randomIndex = Randomizer.RandomInRange (phantomInstructions.Length - 1, 1);
				board.GetComponentInChildren<Text> ().text = phantomInstructions[randomIndex].ToUpper();
				currentSound = phantomInstructionSounds [randomIndex];
			}
			SoundClick ();
			phantomBackground.material = roomTextures[roomIndex];
			currentSlot = 4;

			correctGhost = Randomizer.New(8).ExcludeNumbers(new List<int> { 4 }).Next();
			ghosts[correctGhost].SetActive(true);
			crosshairs[currentSlot].SetActive(true);

			shootToggle.isOn = false;
		}

		public void ActivatePhantomPanel (bool activate)
		{
			phantomPanel.SetActive(activate);
			board.GetComponentInChildren<Text> ().enabled = activate;
			board.GetComponentsInChildren<Image>()[1].enabled = !activate;
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
			SoundController.GetController ().PlayClip (vacuumSound);
			EnableComponents (false);
		
			if(correct){
				Invoke ("ShowRightAnswerAnimation",1f);
			} else {
				Invoke ("ShowWrongAnswerAnimation",1f);
			}

		}

		override public void EnableComponents(bool enabled){
			allToggles.ForEach ((Toggle t) => t.interactable=enabled);
			menuBtn.interactable = enabled;
			soundBtn.interactable = enabled;
		}

		override public void OnWrongAnimationEnd(){
			Next ();
			cleaning = true;
			CleanUI();
			cleaning = false;
			EnableComponents (true);
		}

		override public void OnRightAnimationEnd(){
			Next ();
			cleaning = true;
			CleanUI ();
			cleaning = false;
			EnableComponents (true);
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

		public void EnableDirection(Toggle opposite,bool enable){
			if (!cleaning) {
				opposite.interactable = enable;
				shootToggle.interactable = true;
			}
		}

		public void ClickDirection(Toggle dir){
			if(cleaning) return;
			PlaySoundClick();

			Toggle opposite = GetOppositeToggle(dir);
			EnableDirection(opposite,!dir.isOn);

			int move = GetToggleMove(dir);


			if (dir.isOn) {
				ShowArrow (dir);
				MoveSlots (move);
			} else {
				RefreshArrow (dir);
				MoveSlots (move * -1);
			}
//			dir.enabled = false;
		}

		private Toggle GetOppositeToggle(Toggle dir){
			if (dir == left)
				return right;
			else if (dir == right)
				return left;
			else if (dir == up)
				return down;
			else
				return up;
			
		}

		int GetToggleMove (Toggle dir)
		{
			if (dir == left)
				return -1;
			else if (dir == right)
				return 1;
			else if (dir == up)
				return -3;
			else
				return 3;
		}

		//left -1, right +1, up -3, down +3
		public void MoveSlots(int move){
			if(cleaning) return;

			crosshairs[currentSlot].SetActive(false);

			currentSlot += move;

			crosshairs[currentSlot].SetActive(true);
		}

		//Clean all arrows, redraw the one that's left
		void RefreshArrow (Toggle dir)
		{
			arrows.ForEach((GameObject g) => g.SetActive(false));
			if (dir == left || dir == right) {
				vacuumCleaner.sprite = vacuumSprites [1];

				if (up.isOn)
					ShowArrow (up);
				else if(down.isOn)
					ShowArrow (down);
			} else {
				if (left.isOn)
					ShowArrow (left);
				else if(right.isOn)
					ShowArrow (right);
			}
		}

		public void ShowArrow(Toggle dir){
			if(cleaning) return;

			if (dir == left)
				vacuumCleaner.sprite = vacuumSprites [3];
			else if (dir == right)
				vacuumCleaner.sprite = vacuumSprites [5];
			

			string writtenDir = GetStringDirection (dir);

			switch (writtenDir) {
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

		string GetStringDirection (Toggle dir)
		{
			if (dir == left)
				return "LEFT";
			else if (dir == right)
				return "RIGHT";
			else if (dir == up)
				return "UP";
			else 
				return "DOWN";
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


		/* -------------------------RESTART GAME----------------------- */
		override public  void RestartGame(){
			HideInGameMenu ();

			first = true;
			firstPhantom = true;
			CleanUI ();
			ActivatePhantomPanel (false);
			lifeGhosts.ForEach((Image im) => im.gameObject.SetActive(false));
			model = new HouseActivityModel();

			ShowExplanation ();
		}

	}
} 