using System;

//It's a dragger lie because it's dragging, but then it returns to the old position magically setting text in the dropped slot.
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Sound;
using Assets.Scripts.App;
using Assets.Scripts.Sound;


namespace Assets.Scripts.Common.Dragger {
	public class DraggerHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
		public static DraggerHandler itemBeingDragged;
		public bool activeOnDrop;
		private Vector3 startPosition;
		private bool dropped;
		public bool active;

		public void SetActive(bool isActive){
			active = isActive;
		}

		public void OnBeginDrag(PointerEventData eventData) {
			if (active) {
				SoundController.GetController().PlayDragSound ();
				transform.SetAsLastSibling ();
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
//				SoundController.GetController().PlayDropSound ();
				itemBeingDragged = null;
				GetComponent<CanvasGroup> ().blocksRaycasts = true;

//				if (!dropped) {
					//Uncomment to send object back to starting point
//					transform.position = startPosition;
//				} else {
//					SetActive (false);
//				}
				if (dropped && !activeOnDrop) {
					dropped = false;
//					gameObject.SetActive (false);
				}
			}
		}

		public bool ActiveOnDrop(){
			return activeOnDrop;
		}

		public void Dropped() {
			dropped = true;
		}
	}
}