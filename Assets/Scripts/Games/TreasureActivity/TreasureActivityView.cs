using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Games.TreasureActivity {
	public class TreasureActivityView : LevelView {
		public List<Image> pattern;
		public Button soundBtn, okBtn;
		public List<Button> droppers;
		public List<Image> draggers;

		private Sprite[] figureSprites;
		private TreasureActivityModel model;

		override public void Next(bool first = false){
			if(!first) model.NextLvl();
			model.SetLevel();

			if(model.GameEnded()) EndGame(60,0,1250);
			else SetCurrentLevel();
		}

		public void OkClick(){
			if(IsCorrect()){
				ShowRightAnswerAnimation ();
				model.Correct();
			} else {
				ShowWrongAnswerAnimation();
				model.Wrong();
			}
		}

		public void SoundClick(){
			
		}

		bool IsCorrect() {
			for(int i = 0; i < pattern.Count; i++) {
				if(droppers[i].image.sprite != pattern[i].sprite) return false;
				if(droppers[i + 4].image.sprite != pattern[i].sprite) return false;
			}

			return true;
		}

		public void EraseDropper(Button dropper){
			dropper.image.sprite = null;
			okBtn.interactable = false;
		}

		public void Start(){
			model = new TreasureActivityModel();
			figureSprites = Resources.LoadAll<Sprite>("Sprites/TreasureActivity/figuras");
			Begin();
		}

		public void Begin(){
			ShowExplanation();
		}

		private void SetCurrentLevel() {
			CleanDroppers();

			SetPattern(model.GetPattern());
			SetOptions(model.GetOptions());

			CheckOk();
		}

		void SetOptions(List<Figure> o) {
			for(int i = 0; i < draggers.Count; i++) {
				draggers[i].sprite = GetFigure(o[i]);
			}
		}

		void SetPattern(List<Figure> p) {
			for(int i = 0; i < pattern.Count; i++) {
				pattern[i].sprite = GetFigure(p[i]);
			}
		}

		Sprite GetFigure(Figure figure) {
			return figureSprites[figure.SpriteNumber()];
		}

		void CleanDroppers() {
			droppers.ForEach(EraseDropper);
		}

		public void Dropped() { CheckOk(); }

		void CheckOk() {
			okBtn.interactable = CanSubmit();
		}

		bool CanSubmit() {
			foreach(Button dropper in droppers) {
				if(dropper.image.sprite == null) return false;
			}
			return true;
		}
	}
}