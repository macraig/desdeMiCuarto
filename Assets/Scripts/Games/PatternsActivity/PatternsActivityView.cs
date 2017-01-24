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
		public Button OkButton, NextButton;
		public Button ExchangeButton;

		private PatternsColor currentColor;

		private static PatternsActivityView view;

		override public void Next(bool first = false){
            model.StartFromJson();
            ChangeGrid(model.currentGridIndex());


            ExchangeButton.enabled = true;

            NextButton.gameObject.SetActive(false);
            OkButton.gameObject.SetActive(true);
		    OkButton.enabled = true;

            
            SetAndShowDrawing(currentLeft, null, false);
            SetAndShowDrawing(currentRight, null, false);
            PatternsTile[] left = GetTiles(currentLeft);
			PatternsTile[] right = GetTiles(currentRight);

			for(int i = 0; i < left.Length; i++) {
				left[i].SetColor(colors[0]);
				right[i].SetColor(colors[0]);
			}

			colors.ForEach((PatternsColor c) => c.GetComponent<Toggle>().isOn = false);
			currentColor = null;
		    SetCurrentLevel();
            EnableGridButtons(currentRight, !model.CanPaintLeft());
            EnableGridButtons(currentLeft, model.CanPaintLeft());
        }

		public void ExchangeClick(){
            SoundController.GetController().PlayClickSound();
            model.ExchangeClick();
            
		    PatternsTile[] leftTiles = currentLeft.GetComponentsInChildren<PatternsTile>();
            // guardo los de la izquierda en una lista auxilar
            List<PatternsTile> auxList = new List<PatternsTile>(leftTiles.Length);
		    for (int i = 0; i < leftTiles.Length; i++)
		    {
		        PatternsTile patternsTile = new PatternsTile();
                patternsTile.SetColor(leftTiles[i].GetColor(), false);
		        auxList.Add(patternsTile);
		    }
            // pongo los de la derecha en la de la izquierda
            PatternsTile[] rightTiles = currentRight.GetComponentsInChildren<PatternsTile>();

            for (int i = 0; i < rightTiles.Length; i++)
            {      
                leftTiles[i].SetColor(rightTiles[i].GetColor());
            }

            // pongo los de la lista aux (copia de la izq original) en la derecha
            for (int i = 0; i < auxList.Count; i++)
            {
                rightTiles[i].SetColor(auxList[i].GetColor());
            }
            EnableGridButtons(currentRight, !model.CanPaintLeft());
            EnableGridButtons(currentLeft, model.CanPaintLeft());
        }

        public void OkClick()
        {
            ExchangeButton.enabled = false;
            OkButton.enabled = false;
            if (IsCorrect()){
				ShowRightAnswerAnimation ();
                EnableGridButtons(currentLeft, false);
                EnableGridButtons(currentRight, false);
				model.Correct();
			    ShowCorrectImage();

            }
            else {
				ShowWrongAnswerAnimation();
				model.Wrong();
			}
		}

	    private void ShowCorrectImage()
	    {
            Sprite sprite = Resources.LoadAll<Sprite>("Sprites/PatternsActivity/grilla" + model.GetCurrentGridSize())[
                model.GetCurrentLevel().GetImageIndex()];
            SetAndShowDrawing(model.CanPaintLeft() ? currentLeft : currentRight, sprite, true);

        }

        private void SetAndShowDrawing(GameObject grid, Sprite sprite, bool show)
	    {
        
	        Image[] children = grid.GetComponentsInChildren<Image>(true);
	        Image drawing = children[children.Length - 1];
	        drawing.sprite = sprite;
            drawing.color = Color.white;

            drawing.gameObject.SetActive(show);
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

	    void Awake()
	    {
	        if (view == null) view = this;
            else if(view != this) Destroy(this);
	    }

		public void Start(){
			model = new PatternsActivityModel();
			currentLeft = leftGrids[0];
			currentRight = rightGrids[0];
		    EnableGridButtons(currentRight, false);
		    EnableGridButtons(currentLeft, true);
			Begin();
		}

	    private void EnableGridButtons(GameObject grid, bool enabledButtons)
	    {
	        foreach (Button componentsInChild in grid.GetComponentsInChildren<Button>())
	        {
	            componentsInChild.enabled = enabledButtons;
	            componentsInChild.gameObject.GetComponent<PatternsTile>().enabled = enabledButtons;
	        }
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
            SoundController.GetController().PlaySwitchSound();
			if(c.GetComponent<Toggle>().isOn) {
				currentColor = c;
			} else
				currentColor = null;
		}

        public static PatternsActivityView GetView()
        {
            return view;
        }

	    public override void OnRightAnimationEnd()
	    {
            model.NextLvl();
	        if (model.GameEnded()) EndGame(60, 0, 1250);
	        else
	        {
                NextButton.gameObject.SetActive(true);
                OkButton.gameObject.SetActive(false);
            }

            

        }

        public override void OnWrongAnimationEnd()
	    {
            ExchangeButton.enabled = true;
            OkButton.enabled = true;
        }
	}

    
}