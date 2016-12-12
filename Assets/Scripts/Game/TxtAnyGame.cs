using System;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Assets.Scripts.Game {
	public class TxtAnyGame : ClickGame {
		public Text questionText;
		public List<Image> imageAnswers;
		public List<Text> textAnswers;

		private QuestionAnswerType answerType;

		public override void SetQuestion(Question question) {
			questionText.text = GetText(question.question);

			List<string> orderedAnswers = question.answers;
			List<string> unorderedAnswers = Randomizer.RandomizeList(orderedAnswers);
			if(answerType == QuestionAnswerType.TEXT){
				for(int i = 0; i < textAnswers.Count; i++) {
					textAnswers[i].text = GetText(unorderedAnswers[i]);
				}
			} else {
				for(int i = 0; i < imageAnswers.Count; i++) {
					imageAnswers[i].sprite = Common.LoadImage(unorderedAnswers[i]);
				}
			}

			SetCorrectAnswer(unorderedAnswers.IndexOf(orderedAnswers[question.correctAnswer]));
		}

		public override void ShowQuestionType(QuestionAnswerType questionType) { }

		public override void ShowAnswerType(QuestionAnswerType answerType) {
			this.answerType = answerType;
			if(answerType == QuestionAnswerType.TEXT){
				foreach(var ans in imageAnswers) ans.gameObject.SetActive(false);
			} else {
				foreach(var ans in textAnswers) ans.gameObject.SetActive(false);
			}
		}
	}
}