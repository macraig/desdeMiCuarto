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
		public Player santi;
		List<Direction> instructions;
		[HideInInspector] public bool playersTurn = true;


		public List<SchoolTile> allTiles;
		private List<List<SchoolTile>> viewGrid;


		private SchoolActivityModel model;
		private bool canMove;
		private int currentSlot;

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
			CleanUI ();
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
//			//todo: bloquear acciones del user hasta que termine de moverse
			MoveSequence (ParseInstructions (instructions));
		}

		private void MoveSequence(Vector2[] instructionArray){
			for (int i = 0; i < instructionArray.Length; i++) {
				 canMove = model.AnalizeMovement (instructionArray[i]);
				if (canMove) {
					MoveSanti (instructionArray [i]);
				} else {
					//ShowWrongAnimation
					break;					
				}
				// ver que hay en ese casillero:
				//si hay pared, break y chau turno
				//si hay vacio, me muevo (actualizar santi pos)
				//si hay un trigger me muevo y veo si esta bien o mal (actualizar santi pos)

			}

			//todo: desbloquear acciones del user para que siga jugando
			//borrar todas las instrucciones que ya puso el player
		}

		public void NextMove(){
			//todo
		}

		private void MoveSanti(Vector2 moveTo){
			Vector2 newPosition = model.GetSantiPos () + moveTo;
			viewGrid[(int)model.GetSantiPos().x][(int)model.GetSantiPos().y].ShowSanti(false);
			viewGrid[(int)newPosition.x][(int)newPosition.y].ShowSanti(false);
			model.UpdateSantiPosition (newPosition);
			Invoke ("OnTrigger", 1f);
		}

		private void OnTrigger(){
			Tile tile = model.GetCurrentTile ();
			if (tile == Tile.EMPTY)
				return;
			if(model.IsCorrectSector()){
				//showAnimationCorrect
				//nextTurn
			}
			canMove = false;

			
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

		Vector2[] ParseInstructions (List<Direction> instructions)
		{
			Vector2[] vectorArray = new Vector2[instructions.Count];
			for (int i = 0; i < instructions.Count; i++) {
				vectorArray [i] = model.ParseInstruction (instructions [i]); 
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


	}
}