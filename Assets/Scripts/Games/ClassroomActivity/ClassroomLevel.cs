﻿using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Assets.Scripts.Games.ClassroomActivity {
	public class ClassroomLevel {
		GameObject correct;
		List<GameObject> wrong;

		public ClassroomLevel(GameObject correct, List<GameObject> wrong){
			this.wrong = wrong;
			this.correct = correct;
		}

		public void Set(ClassroomActivityView view, bool enabled) {
			Button c = correct.GetComponent<Button>();
			c.enabled = enabled;
			if(enabled) c.onClick.AddListener(view.ClickCorrect);
			else c.onClick.RemoveAllListeners();

			foreach(GameObject w in wrong) {
				Button wb = w.GetComponent<Button>();
				wb.enabled = enabled;

				if(enabled) wb.onClick.AddListener(view.ClickWrong);
				else wb.onClick.RemoveAllListeners();
			}
		}
	}
}