using Assets.Scripts.Sound;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Games.TreasureActivity {
	public class TreasureSlot : MonoBehaviour, IDropHandler {
		public TreasureActivityView view;

		public void OnDrop(PointerEventData eventData) {
			TreasureDragger target = TreasureDragger.itemBeingDragged;
			if(target != null) {
                SoundController.GetController().PlayDropSound();
			    Image image = GetComponent<Image>();
			    image.color = Color.white;
			    image.sprite = target.GetComponent<Image>().sprite;
				view.Dropped();
			    GetComponent<Button>().enabled = false;
                Invoke("EnableButton", 1);
			}


		}

	    private void EnableButton()
	    {
            GetComponent<Button>().enabled = true;

        }

    }
}