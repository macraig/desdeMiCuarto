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
		public List<Button> controlButtons;
		public Button soundButton,menuButton;
		public Image board;
		public Player santi;
		List<Direction> instructions;

		[HideInInspector] public bool playersTurn = true;


		public List<SchoolTile> allTiles;
		private List<List<SchoolTile>> viewGrid;


		private SchoolActivityModel model;
		private List<Direction> currentInstructions;

		public void Start(){
			ShowExplanation();
			instructions = new List<Direction> ();
			model = new SchoolActivityModel();
			CreateViewGrid ();
			ShowSanti ();

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

//			okBtn.interactable = false;

			model.SetSector();
			board.sprite = model.BoardSprite();
			SoundClick();
		}

	


		public void SoundClick(){
			SoundController.GetController().PlayClip(model.BoardClip());
		}

		void CreateViewGrid ()
		{
			viewGrid = new List<List<SchoolTile>> ();
			int k = 0;
			for (int i = 0; i< model.GetRows (); i++) {
				List<SchoolTile> rowList = new List<SchoolTile> ();
				for (int j = 0; j < model.GetCols (); j++) {
					
					rowList.Add (allTiles[k]);
					k++;
				}
				viewGrid.Add (rowList);
			}
		}

		void ShowSanti ()
		{
			Vector2 santiPos = model.GetSantiPos ();
			viewGrid[(int)santiPos.x] [(int)santiPos.y].ShowSanti(true);
		}

		public void OkClick(){
			GetInstructions ();
			//todo: bloquear acciones del user hasta que termine de moverse
			currentInstructions = GetInstructions ();
			DisableAllButtons ();
			NextMove ();
		}

		private void NextMove(){
			if (currentInstructions.Count > 0) {
				Vector2 instructionVector = model.ParseInstruction(currentInstructions[0]);

				bool canMove = model.AnalizeMovement (instructionVector);
				if (canMove) {
					MoveSanti (currentInstructions[0]);
				} else {
					ShowWrongAnswerAnimation ();						
				}
				currentInstructions.RemoveAt (0);
			}else{
				CleanUI ();
				EnableAllButtons ();
			}
				 
		}

		void EnableAllButtons ()
		{
			controlButtons.ForEach((Button b) => b.interactable=true);
			directionsListBtns.ForEach((GameObject g) => g.GetComponent<Button>().interactable=true);
			soundButton.interactable = true;
			menuButton.interactable = true;
			okBtn.interactable = true;

		}

		void DisableAllButtons ()
		{
			controlButtons.ForEach((Button b) => b.interactable=false);
			directionsListBtns.ForEach((GameObject g) => g.GetComponent<Button>().interactable=false);
			soundButton.interactable = false;
			okBtn.interactable = false;
			menuButton.interactable = false;

		}


		private void MoveSanti(Direction moveTo){
			SchoolTile currentTile = viewGrid [(int)model.GetSantiPos ().x] [(int)model.GetSantiPos ().y];
			switch (moveTo) {
			case Direction.DOWN:
				currentTile.MoveSantiDown ();
				break;
			case Direction.UP:
				currentTile.MoveSantiUp ();
				break;
			case Direction.LEFT:
				currentTile.MoveSantiLeft ();
				break;
			case Direction.RIGHT:
				currentTile.MoveSantiRight ();
				break;
			}

		}

		public void EndMove(){
			Vector2 newPosition = model.GetNewPosition ();
//			Vector2 santiPos = new Vector2 ((int)model.GetSantiPos ().x, (int) model.GetSantiPos ().y);
//			viewGrid[(int)santiPos.x][(int)santiPos.y].ShowSanti(false);
			viewGrid[(int)newPosition.x][(int)newPosition.y].ShowSanti(true);
			model.UpdateSantiPosition (newPosition);
			if (model.OnTrigger ()) {
				if (model.IsCorrectSector ()) {
					ShowRightAnswerAnimation ();
				} else {
					ShowWrongAnswerAnimation ();
				}
			} else {
				NextMove ();
			}
		

		}



		private List<Direction> GetInstructions(){
			instructions.Clear ();

			foreach (GameObject dirImg in directionsListBtns) {
				if (dirImg.activeSelf) {
					instructions.Add(model.ParseFromSprite(dirImg.GetComponent<Image>().sprite));
				}
			}
			return instructions;
		}

		List<Vector2> ParseInstructions (List<Direction> instructions)
		{
			List<Vector2> vectorArray = new List<Vector2> ();
			for (int i = 0; i < instructions.Count; i++) {
				vectorArray.Add(model.ParseInstruction (instructions [i])); 
			}
			return vectorArray;
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
			foreach (GameObject dirImg in directionsListBtns) {
				if (!dirImg.activeSelf) {
					dirImg.SetActive (true);
					dirImg.GetComponent<Image>().sprite = model.DirectionSprite (model.ParseToDirection(dir));
					break;
				}
			}
		}

		override public void OnWrongAnimationEnd(){
			CleanUI ();
			EnableAllButtons ();
		}

		override public void OnRightAnimationEnd(){
			CleanUI ();
			EnableAllButtons ();
			Next ();
		}



	}
}