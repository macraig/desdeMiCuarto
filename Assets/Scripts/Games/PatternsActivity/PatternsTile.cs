using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Games.PatternsActivity {
	public class PatternsTile : MonoBehaviour {
		public bool isLeft;
		private PatternsColor color;

		public PatternsColor GetColor(){
			return color;
		}

		public void SetColor(PatternsColor name) {
			color = name;
			GetComponent<Image>().color = name.color;
		}
	}
}