using System;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Assets.Scripts.Games.NeighbourhoodActivity {
	public class NeighbourhoodDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
		public static NeighbourhoodDragger itemBeingDragged;
		private Vector3 startPosition;
		public bool active;

		public void SetActive(bool isActive){
			active = isActive;
		}

		public void OnBeginDrag(PointerEventData eventData) {
			if (active) {
				//				SoundManager.instance.PlayClickSound ();
				itemBeingDragged = this;
				startPosition = transform.position;
				GetComponent<CanvasGroup> ().blocksRaycasts = false;
			}
		}

		public void OnDrag(PointerEventData eventData) {
			if (active) 
				transform.position = Input.mousePosition;
		}

		public void OnEndDrag(PointerEventData eventData = null) {
			if (active) {
				//SoundController.GetController().PlayClickSound ();
				itemBeingDragged = null;
				GetComponent<CanvasGroup> ().blocksRaycasts = true;

				transform.position = startPosition;
			}
		}
	}
}