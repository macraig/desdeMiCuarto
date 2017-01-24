using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Assets.Scripts.Games.ClassroomActivity {
	public class ClassroomLevel {
		GameObject correct;
		List<GameObject> wrong;
		AudioClip sound;

		public ClassroomLevel(GameObject correct, List<GameObject> wrong,string soundName){
			this.wrong = wrong;
			this.correct = correct;
			sound = Resources.Load<AudioClip> ("Audio/ClassroomActivity/SFX/" + soundName);
		}

		public AudioClip GetAudioClip ()
		{
			return sound;
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