using System;
using UnityEngine.UI;

namespace Assets.Scripts.Editors.QuestionBoxes {
	public class ImageQuestionBox : QuestionBox {
		public Image questionImage;

		public override void SetQuestion(Question q) {
			base.SetQuestion(q);

			questionImage.sprite = Common.LoadImage(q.question);
			questionImage.GetComponentInChildren<Text>().gameObject.SetActive(false);
		}
	}
}