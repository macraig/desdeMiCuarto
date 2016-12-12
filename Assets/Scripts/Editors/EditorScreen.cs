using System;
using UnityEngine;
using Assets.Scripts.Menus;
using System.Collections.Generic;
using UnityEngine.UI;
using uFileBrowser;
using I18N;

namespace Assets.Scripts.Editors {
	public abstract class EditorScreen : MonoBehaviour {
		public FileBrowser soundFileBrowser, imageFileBrowser;

		protected GeneralOverview parent;
		protected Question question;

		public Button ticButton;
		public List<Toggle> correctToggles;

		protected int currentIndex;

		public void CheckTicButton(){
			ticButton.interactable = CanSubmitQuestion();
		}

		public void BackClick(){
			warningPanel.SetActive(true);
		}

		public bool IsEdit(){
			return question != null;
		}

		public Question GetQuestion() {
			return new Question(GetQuestionTitle(), GetQuestionText(), GetSecondQuestionText(), GetQuestionAnswers(), GetCorrectQuestion());
		}

		public abstract string GetSecondQuestionText();

		public abstract string GetQuestionTitle();

		public abstract List<string> GetQuestionAnswers();

		public abstract string GetQuestionText();

		public void TicClick(){
			if(IsEdit()) parent.EditQuestion(GetQuestion().SetQuestionNumber(question.number));
			else parent.AddQuestion(GetQuestion());

			Destroy(gameObject);
		}

		protected int GetCorrectQuestion() {
			return correctToggles.FindIndex((Toggle t) => t.isOn);
		}

		public abstract bool CanSubmitQuestion();

		public void SetQuestion(Question q, GeneralOverview generalOverview) {
			SetParent(generalOverview);
			question = q;

			BaseSetQuestion(q);

			correctToggles[q.correctAnswer].isOn = true;
		}

		public abstract void BaseSetQuestion(Question q);

		public void SetParent(GeneralOverview parent){ this.parent = parent; }

		// Warning panel *************************************************************************************

		public GameObject warningPanel;
		public Text warningQuestion, warningInstructions, warningYes, warningNo;

		public void WarningYes(){
			Destroy(gameObject);
		}

		public void WarningNo(){
			warningPanel.SetActive(false);
		}

		// I18N **********************************************************************************************

		public void SetWarningTexts(){
			warningQuestion.text = I18n.Msg("warningPanel.leaveQuestion");
			warningInstructions.text = I18n.Msg("warningPanel.instructions");
			warningYes.text = I18n.Msg("common.yes");
			warningNo.text = I18n.Msg("common.no");
		}
	}
}