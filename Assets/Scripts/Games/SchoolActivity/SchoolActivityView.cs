using System;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using Assets.Scripts.Common;
using Assets.Scripts.Sound;

namespace Assets.Scripts.Games.SchoolActivity {
	public class SchoolActivityView : LevelView {
		private static Color32 SELECTED_COLOR = new Color32 (238,212,63,255);
		public  int BOUNCE = 20;

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
		private int directionCounter;
		private Vector3 santiInitPos;
		private GameObject currentTile;


		public void Start(){
				
			ShowExplanation ();
			instructions = new List<Direction> ();
			model = new SchoolActivityModel ();
			CreateViewGrid ();
			santiInitPos = santi.transform.position;
		}

		override public void Next(bool first = false) {
			
			if(model.IsFinished()) {
				//				5 ESTRELLAS: 0 ERRORES
				//				4 ESTRELLAS: 1ERROR
				//				3 ESTRELLAS: 2 o 3 ERRORES
				//				2 ESTRELLAS: 4 o 5 ERRORES
				//				1 ESTRELLA: MAS DE 5 ERRORES
				EndGame(0,0,800);
				return;
			}

//			okBtn.interactable = false;

			model.SetSector();
			board.sprite = model.BoardSprite();
			SoundClick();
		}


		override public  void RestartGame(){
			base.RestartGame ();
			CleanUI ();
			santi.transform.position  = santiInitPos;
			Start ();


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
			directionCounter = 0;
			NextMove ();
		}

		private void NextMove(){
			if (currentInstructions.Count > 0) {
				Vector2 instructionVector = model.ParseInstruction(currentInstructions[0]);

				directionsListBtns [directionCounter].GetComponent<Image> ().color = SELECTED_COLOR;
				directionsListBtns [directionCounter].GetComponent<Button> ().interactable = true;
				directionsListBtns [directionCounter].GetComponent<Button> ().enabled = false;
				directionCounter++;

				bool canMove = model.AnalizeMovement (instructionVector);
				if (canMove) {
					MoveSanti (currentInstructions[0]);
				} else {
					if (!model.MovementEscapesGrid ())
						FailMoveSanti (currentInstructions [0]);
					else
						OutOfGridMove ();
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
			 currentTile = viewGrid [(int)model.GetNewPosition ().x] [(int)model.GetNewPosition ().y];
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
			currentTile = viewGrid [(int)model.GetNewPosition ().x] [(int)model.GetNewPosition ().y];
			RectTransform rt = (RectTransform)currentTile.transform;
			switch (moveTo) {
			case Direction.DOWN:
				santi.transform.DOMoveY ((currentTile.transform.position.y) + BOUNCE, 0.5f).OnComplete (EndFailMove);
				break;
			case Direction.UP:
				santi.transform.DOMoveY ((currentTile.transform.position.y) - BOUNCE, 0.5f).OnComplete (EndFailMove);
				break;
			case Direction.LEFT:
				santi.transform.DOMoveX((currentTile.transform.position.x) + BOUNCE, 0.5f).OnComplete(EndFailMove);
				break;
			case Direction.RIGHT:
				santi.transform.DOMoveX((currentTile.transform.position.x) - BOUNCE, 0.5f).OnComplete(EndFailMove);
				break;
			}

		}



		private void EndFailMove(){
			currentTile = viewGrid [(int)model.GetSantiPos ().x] [(int)model.GetSantiPos ().y];
			santi.transform.DOMoveY(currentTile.transform.position.y,1);
			santi.transform.DOMoveX(currentTile.transform.position.x,1).OnComplete(ShowWrongAnswerAnimation);
		}

		private void OutOfGridMove(){
			santi.transform.DOShakeRotation (0.5f).OnComplete (EndOutOfGridMove);
		}

		private void EndOutOfGridMove(){
			currentTile = viewGrid [(int)model.GetSantiPos ().x] [(int)model.GetSantiPos ().y];
			ShowWrongAnswerAnimation ();
		}



		public void EndMove(){
			Vector2 newPosition = model.GetNewPosition ();
			model.UpdateSantiPosition (newPosition);
			if (model.OnTrigger ()) {
				if (model.IsCorrectSector ()) {
					model.AddStreak (true);
					ShowRightAnswerAnimation ();
				} else {
					model.AddStreak (false);
					ShowWrongAnswerAnimation ();
				}
			} else {
				directionsListBtns [directionCounter-1].GetComponent<Image> ().color = Color.white;
				directionsListBtns [directionCounter-1].GetComponent<Button> ().enabled = true;
				directionsListBtns [directionCounter-1].GetComponent<Button> ().interactable = false;
				Invoke ("NextMove", 0.1f);
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
			foreach (GameObject button in directionsListBtns) {
				button.GetComponent<Image> ().color = Color.white;
				button.SetActive (false);
			}
//			directionsListBtns.ForEach((GameObject g) => g.SetActive(false));
		}
			
		public void OnClickDirection(string dir){
			PlaySoundClick();
			AddInstruction (dir);
		}
			
		public void OnClickDirectionImage(Button dirBtn){
			SoundController.GetController ().PlayDropSound ();
			Debug.Log ("clicking!");
			int btnIndex = directionsListBtns.IndexOf (dirBtn.gameObject);
			int instructionEndIndex = GetInstructionEndIndex ();

			for (int i = btnIndex; i < instructionEndIndex; i++) {
				directionsListBtns [i].GetComponent<Image> ().sprite = directionsListBtns [i+1].GetComponent<Image> ().sprite;
			}
			directionsListBtns[instructionEndIndex].gameObject.SetActive (false);

		}

		int GetInstructionEndIndex(){
			for (int i=0;i<directionsListBtns.Count;i++) {
				if (!directionsListBtns[i].activeSelf) {
					return i-1;
				}
			}
			return -1;
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