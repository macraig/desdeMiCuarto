using System;
using UnityEngine.UI;
using Assets.Scripts.App;

namespace Assets.Scripts.Editors.QuestionBoxes {
	public class SoundQuestionBox : QuestionBox {
		public Image soundBtn;
		public Text questionTitle;
		private string audioPath;

		public override void SetQuestion(Question q) {
			base.SetQuestion(q);
			questionTitle.text = q.title;

			audioPath = q.question;
		}

		public void SoundClick(){ SoundController.GetController().PlayClip(Common.LoadSound(audioPath)); }
	}
}