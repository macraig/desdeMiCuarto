using System;
using UnityEngine.UI;

namespace Assets.Scripts.Editors.QuestionBoxes {
	public class TextQuestionBox : QuestionBox {
		public Text questionText;

		public override void SetQuestion(Question q) {
			base.SetQuestion(q);
			questionText.text = q.question;
		}
	}
}