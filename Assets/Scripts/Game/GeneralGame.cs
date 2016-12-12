using System;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.App;

namespace Assets.Scripts.Game {
	public class GeneralGame : ClickGame {
		public Image questionImage;
		public List<Image> imageAnswers;
		public List<Text> textAnswers;

		private AudioClip questionClip;

		private QuestionAnswerType questionType;
		private QuestionAnswerType answerType;

		public override void SetQuestion(Question question) {
			if(questionType == QuestionAnswerType.IMG){
				questionImage.sprite = Common.LoadImage(question.question);
			} else if(questionType == QuestionAnswerType.AUDIO){
				questionClip = Common.LoadSound(question.question);
			} else if(questionType == QuestionAnswerType.AUDIO_IMG){
				questionImage.sprite = Common.LoadImage(question.question);
				questionClip = Common.LoadSound(question.secondQuestion);
			}

			List<string> orderedAnswers = question.answers;
			List<string> unorderedAnswers = Randomizer.RandomizeList(orderedAnswers);
			if(answerType == QuestionAnswerType.TEXT){
				for(int i = 0; i < textAnswers.Count; i++)
					textAnswers[i].text = GetText(unorderedAnswers[i]);
			} else {
				for(int i = 0; i < imageAnswers.Count; i++) imageAnswers[i].sprite = Common.LoadImage(unorderedAnswers[i]);
			}

			SetCorrectAnswer(unorderedAnswers.IndexOf(orderedAnswers[question.correctAnswer]));
		}

		public override void ShowQuestionType(QuestionAnswerType questionType) {
			this.questionType = questionType;
		}

		public override void ShowAnswerType(QuestionAnswerType answerType) {
			this.answerType = answerType;
			if(answerType == QuestionAnswerType.TEXT){
				foreach(var ans in imageAnswers) ans.gameObject.SetActive(false);
			} else {
				foreach(var ans in textAnswers) ans.gameObject.SetActive(false);
			}
		}

		public void PlayQuestion(){
			if(questionClip != null) SoundController.GetController().PlayClip(questionClip);
		}
	}
}