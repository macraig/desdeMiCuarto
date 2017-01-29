using System;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
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
		public Image santi;
		List<Direction> instructions;
		public List<GameObject> allTiles;
		private List<List<GameObject>> viewGrid;
		private SchoolActivityModel model;
		private List<Direction> currentInstructions;

		public void Start(){
			ShowExplanation();
			instructions = new List<Direction> ();
			model = new SchoolActivityModel();
			CreateViewGrid ();
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
			viewGrid = new List<List<GameObject>> ();
			int k = 0;
			for (int i = 0; i< model.GetRows (); i++) {
				List<GameObject> rowList = new List<GameObject> ();
				for (int j = 0; j < model.GetCols (); j++) {
					
					rowList.Add (allTiles[k]);
					k++;
				}
				viewGrid.Add (rowList);
			}
		}
			
		public void OkClick(){
			
			GetInstructions ();
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
//					FailMoveSanti (currentInstructions [0]);
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
			GameObject currentTile = viewGrid [(int)model.GetNewPosition ().x] [(int)model.GetNewPosition ().y];
			switch (moveTo) {
			case Direction.DOWN:
			case Direction.UP:
				santi.transform.DOMoveY(currentTile.transform.position.y,1).OnComplete(EndMove);
				break;
			case Direction.LEFT:
			case Direction.RIGHT:
				santi.transform.DOMoveX(currentTile.transform.position.x,1).OnComplete(EndMove);
				break;
			}

		}

		private void FailMoveSanti(Direction moveTo){
			GameObject currentTile = viewGrid [(int)model.GetNewPosition ().x] [(int)model.GetNewPosition ().y];
			RectTransform rt = (RectTransform)currentTile.transform;
			switch (moveTo) {
			case Direction.DOWN:
			case Direction.UP:
				santi.transform.DOMoveY((currentTile.transform.position.y)-rt.rect.height*0.3f,1).OnComplete(ShowWrongAnswerAnimation);
				break;
			case Direction.LEFT:
			case Direction.RIGHT:
				santi.transform.DOMoveX((currentTile.transform.position.x)/3,1).OnComplete(ShowWrongAnswerAnimation);
				break;
			}

		}

		public void EndMove(){
			Vector2 newPosition = model.GetNewPosition ();
			model.UpdateSantiPosition (newPosition);
			if (model.OnTrigger ()) {
				if (model.IsCorrectSector ()) {
					model.AddStreak (true);
					ShowRightAnswerAnimation ();
				} else {
					model.AddStreak (true);
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
			model.NextStage ();
			Next ();
		}



	}
}