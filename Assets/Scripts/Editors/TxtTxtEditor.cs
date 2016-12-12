using System;
using System.Collections.Generic;
using UnityEngine.UI;
using I18N;

namespace Assets.Scripts.Editors {
	public class TxtTxtEditor : EditorScreen {
		public InputField questionText;
		public List<InputField> answerTexts;

		public void Start(){
			SetTexts();
			if(!IsEdit()) {
				CheckTicButton();
			}
		}

		public override bool CanSubmitQuestion() {
			foreach(InputField answerText in answerTexts) {
				if(answerText.text.Trim().Length == 0) return false;
			}
			return questionText.text.Trim().Length != 0 && GetCorrectQuestion() != -1;
		}

		public override string GetQuestionText() {
			return questionText.text;
		}

		public override string GetSecondQuestionText() {
			return "";
		}

		public override List<string> GetQuestionAnswers() {
			return answerTexts.ConvertAll((InputField txt) => txt.text);
		}

		public override string GetQuestionTitle() {
			return "";
		}

		public override void BaseSetQuestion(Question q) {
			questionText.text = q.question;

			for(int i = 0; i < answerTexts.Count; i++) {
				answerTexts[i].text = q.answers[i];
			}

			CheckTicButton();
		}

		// I18N **************************************************************************************

		void SetTexts() {
			SetWarningTexts();
			questionText.placeholder.GetComponentInChildren<Text>().text = I18n.Msg("editors.enterQuestion");
			foreach(InputField ans in answerTexts) {
				ans.placeholder.GetComponentInChildren<Text>().text = I18n.Msg("editors.enterText");
			}
		}
	}
}