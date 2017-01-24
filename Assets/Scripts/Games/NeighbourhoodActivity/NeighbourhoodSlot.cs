using System;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Games.NeighbourhoodActivity {
	public class NeighbourhoodSlot : MonoBehaviour, IDropHandler {
		public NeighbourhoodActivityView view;
		public int row, column;

		public void OnDrop(PointerEventData eventData) {
			NeighbourhoodDragger target = NeighbourhoodDragger.itemBeingDragged;
			if(target != null) {
				GetComponent<Image>().sprite = target.GetComponent<Image>().sprite;
				view.Dropped(target, this, row, column);
			}
		}
	}
}