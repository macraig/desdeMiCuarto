using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Games.TreasureActivity {
	public class TreasureActivityView : LevelView {
		public List<Image> pattern;
		public Button okBtn, NextButton;
		public List<Image> DropBackgroundImages;
		public List<Image> draggers;
	    public GameObject[] DropperTables;

		private Sprite[] figureSprites;
		private TreasureActivityModel model;
		public AudioClip ingameExplanationSound;

		override public void Next(bool first = false){
            EnableDropers(true);

            if (!first) PlaySoundClick();
			model.SetLevel();
		    ShowCurrentDropTable();
		    okBtn.enabled = true;
            NextButton.gameObject.SetActive(false);
            okBtn.gameObject.SetActive(true);
            okBtn.enabled = true;
			SetCurrentLevel();
		}

	    private void ShowCurrentDropTable()
	    {
            DropperTables[0].gameObject.SetActive(model.TableSize == 4);
            DropperTables[1].gameObject.SetActive(model.TableSize == 8);            
	    }

	    private GameObject GetCurrentDropperTable()
	    {
	        return DropperTables[model.TableSize == 4 ? 0 : 1];
	    }

	    public void OkClick()
		{
            EnableDropers(false);
            okBtn.enabled = false;
            if (IsCorrect()){
                model.Correct();
                ShowRightAnswerAnimation();
			} else {
				ShowWrongAnswerAnimation();
				model.Wrong();
			}
		}

		public void OnSoundButtonClick(){
			SoundController.GetController ().PlayClip (ingameExplanationSound);
		}

		bool IsCorrect() {
			for(int i = 0; i < pattern.Count; i++) {
				if(GetDroppers()[i].image.sprite != pattern[i].sprite) return false;
				if(model.TableSize == 8 && GetDroppers()[i + 4].image.sprite != pattern[i].sprite) return false;
			}

			return true;
		}

	    private List<Button> GetDroppers()
	    {
	        return GetCurrentDropperTable().GetComponentsInChildren<Button>().ToList();
	    }

	    public void EraseDropper(Button dropper){
			dropper.image.sprite = null;
            dropper.image.color = new Color32(0,0,0,1);

            okBtn.interactable = false;
            SoundController.GetController().PlayClickSound();
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

		void SetOptions(List<Figure> o)
		{
		    int i = 0;
            for (; i < o.Count; i++) {
                draggers[i].gameObject.SetActive(true);

                draggers[i].sprite = GetFigure(o[i]);
			}
            for (; i < draggers.Count; i++)
            {
                draggers[i].gameObject.SetActive(false);
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
            GetDroppers().ForEach(EraseDropper);
		}

		public void Dropped() { CheckOk(); }

		void CheckOk() {
			okBtn.interactable = CanSubmit();
		}

		bool CanSubmit() {
			foreach(Button dropper in GetDroppers()) {
				if(dropper.image.sprite == null) return false;
			}
			return true;
		}

	    public override void OnRightAnimationEnd()
	    {

	        if (model.LastCorrect)
	        {
	            model.NextLvl();
	        }
	        else
	        {
                model.LastCorrect = true;
	        }
            if (model.GameEnded()) EndGame(60, 0, 1250);
            else
            {
                NextButton.gameObject.SetActive(true);
                okBtn.gameObject.SetActive(false);
            }
        }

	    public override void OnWrongAnimationEnd()
	    {
            model.LastCorrect = false;
            EnableDropers(true);

            okBtn.enabled = true;

	    }

	    private void EnableDropers(bool value)
	    {
            foreach (Button dropper in GetDroppers())
            {
                dropper.enabled = value;
            }
	        foreach (Image dragger in draggers)
	        {
	            dragger.gameObject.GetComponent<TreasureDragger>().enabled = value;
	        }
        }
	}
}