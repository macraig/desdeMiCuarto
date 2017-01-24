using System;
using Assets.Scripts.Sound;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Games.TreasureActivity {
	public class TreasureDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
		public static TreasureDragger itemBeingDragged;
		private Vector3 startPosition;
        public GameObject ParentGameObject;
		public bool active;
		public GameObject OnFrontTheScene;
	    private int order;


        public void SetActive(bool isActive){
			active = isActive;
		}

		public void OnBeginDrag(PointerEventData eventData) {
			if (active)
			{
			    order = this.transform.GetSiblingIndex();
                ParentGameObject.GetComponent<GridLayoutGroup>().enabled = false;
                this.transform.SetParent(OnFrontTheScene.transform);
				SoundController.GetController().PlayDragSound();
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
                SoundController.GetController().PlayDropSound();
                itemBeingDragged = null;
				GetComponent<CanvasGroup> ().blocksRaycasts = true;
			    this.transform.SetParent(ParentGameObject.transform);
                this.transform.SetSiblingIndex(order);
                ParentGameObject.GetComponent<GridLayoutGroup>().enabled = true;
            }
        }
	}
}