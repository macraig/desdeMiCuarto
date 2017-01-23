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




		private SchoolActivityModel model;
		private int currentSlot;

		public void Start(){
			ShowExplanation();
			instructions = new List<Direction> ();
			model = new SchoolActivityModel();
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
			GetInstructions ();
			santi.MoveSequence (ParseInstructions (instructions));
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