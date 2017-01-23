using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Games.TreasureActivity {
	public class TreasureSlot : MonoBehaviour, IDropHandler {
		public TreasureActivityView view;

		public void OnDrop(PointerEventData eventData) {
			TreasureDragger target = TreasureDragger.itemBeingDragged;
			if(target != null) {
				GetComponent<Image>().sprite = target.GetComponent<Image>().sprite;
				view.Dropped();
			}
		}
	}
}