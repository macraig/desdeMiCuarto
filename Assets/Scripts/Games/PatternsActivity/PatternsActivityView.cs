using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Sound;
using SimpleJSON;

namespace Assets.Scripts.Games.PatternsActivity {
	public class PatternsActivityView : LevelView {
		public List<GameObject> leftGrids, rightGrids;
		public List<PatternsColor> colors;

		private GameObject currentLeft, currentRight;

		private PatternsColor currentColor;

		override public void Next(bool first = false){
			if(!first) model.NextLvl();

			PatternsTile[] left = GetTiles(currentLeft);
			PatternsTile[] right = GetTiles(currentRight);

			for(int i = 0; i < left.Length; i++) {
				left[i].SetColor(colors[0]);
				right[i].SetColor(colors[0]);
			}

			colors.ForEach((PatternsColor c) => c.GetComponent<Toggle>().isOn = false);
			currentColor = null;

			if(model.GameEnded()) EndGame(60,0,1250);
			else SetCurrentLevel();
		}

		public void ExchangeClick(){
			//model.ExchangeClick();
			//TODO exchange left and right.
		}

		public void OkClick(){
			if(IsCorrect()){
				ShowRightAnswerAnimation ();
				model.Correct();
				ChangeGrid(model.currentGridIndex());
			} else {
				ShowWrongAnswerAnimation();
				model.Wrong();
			}
		}

		bool IsCorrect() {
			PatternsTile[] left = GetTiles(currentLeft);
			PatternsTile[] right = GetTiles(currentRight);

			for(int i = 0; i < left.Length; i++) {
				if(left[i].GetColor().name != right[i].GetColor().name) return false;
			}
			return true;
		}

		void ChangeGrid(int gridIndex) {
			currentLeft.SetActive(false);
			currentLeft = leftGrids[gridIndex];
			currentLeft.SetActive(true);

			currentRight.SetActive(false);
			currentRight = rightGrids[gridIndex];
			currentRight.SetActive(true);
		}

		private PatternsActivityModel model;

		public void Start(){
			model = new PatternsActivityModel();
			currentLeft = leftGrids[0];
			currentRight = rightGrids[0];
			Begin();
		}

		public void Begin(){
			ShowExplanation();
		}

		private void SetCurrentLevel() {
			model.RandomizeLvl();
			SetColors(model.GetColors(AllColors()));
			SetPattern(model.GetColorArray(), model.CanPaintLeft() ? currentRight : currentLeft);
		}

		void SetPattern(List<string> colorArray, GameObject grid) {
			PatternsTile[] tiles = GetTiles(grid);

			for(int i = 0; i < colorArray.Count; i++) {
				tiles[i].SetColor(FindColor(colorArray[i]));
			}
		}

		PatternsColor FindColor(string name) {
			return colors.Find((PatternsColor c) => c.name == name);
		}

		static PatternsTile[] GetTiles(GameObject grid) {
			return grid.transform.FindChild("gridTiles").GetComponentsInChildren<PatternsTile>();
		}

		public void ClickTile(PatternsTile tile){
			if((tile.isLeft && !model.CanPaintLeft()) || (!tile.isLeft && model.CanPaintLeft()) || currentColor == null) return;

			tile.SetColor(currentColor);
			tile.GetComponent<Image>().color = currentColor.color;
		}

		List<string> AllColors() {
			return colors.ConvertAll((PatternsColor c) => c.name);
		}

		void SetColors(List<string> colors) {
			this.colors.ForEach((PatternsColor toggle) => toggle.gameObject.SetActive(colors.Contains(toggle.name)));
		}

		public void ColorChange(PatternsColor c){
			if(c.GetComponent<Toggle>().isOn) {
				currentColor = c;
			} else
				currentColor = null;
		}
	}
}