using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Common.Dragger;

namespace Assets.Scripts.Games.BedroomActivity {
	public class BedroomLevel {
		List<GameObject> correct, wrong, objects;
		GameObject target;
		string sound;
		StageType type;

		private BedroomLevel(StageType type, List<GameObject> correct, List<GameObject> wrong, List<GameObject> objects, GameObject target, string sound) {
			this.correct = correct;
			this.wrong = wrong;
			this.objects = objects;
			this.target = target;
			this.sound = sound;
			this.type = type;
		}

		public static BedroomLevel FromClickOrToggle(StageType type, List<GameObject> correct, List<GameObject> wrong, string sound){
			return new BedroomLevel(type, correct, wrong, null, null, sound);
		}

		public static BedroomLevel FromDrag(StageType type, List<GameObject> objects, GameObject target, string sound){
			return new BedroomLevel(type, null, null, objects, target, sound);
		}

		public GameObject Target() {
			return target;
		}

		public List<GameObject> Correct() {
			return type == StageType.DRAG ? objects : correct;
		}

		public void Set(bool enabled) {
			switch(type) {
			case StageType.TOGGLE:
				foreach(GameObject c in correct) {
					c.GetComponent<Button>().enabled = enabled;
				}
				break;
			case StageType.CLICK:
				foreach(GameObject c in correct) {
					c.GetComponent<Button>().enabled = enabled;
				}

				foreach(GameObject w in wrong) {
					w.GetComponent<Button>().enabled = enabled;
				}
				break;
			case StageType.DRAG:
				foreach(GameObject d in objects) {
					d.GetComponent<DraggerHandler>().SetActive(enabled);
				}

				break;
			}
		}
	}
}

