using System;
using Assets.Scripts.Sound;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Games.PatternsActivity {
	public class PatternsTile : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler {
		public bool isLeft;
		private PatternsColor color;

		public PatternsColor GetColor(){
			return color;
		}

		public void SetColor(PatternsColor patternColor, bool setAlsoImage = true) {
			color = patternColor;
			if(setAlsoImage) GetComponent<Image>().color = patternColor.color;
		}

	    public void OnPointerEnter(PointerEventData eventData)
	    {
	        if (Input.GetMouseButton(0))
	        {
                PatternsActivityView.GetView().ClickTile(this);
                SoundController.GetController().PlayMusicNote(this.color.name);
            }

        }

	    public void OnPointerDown(PointerEventData eventData)
	    {
            PatternsActivityView.GetView().ClickTile(this);
            SoundController.GetController().PlayMusicNote(this.color.name);

        }

    }
}