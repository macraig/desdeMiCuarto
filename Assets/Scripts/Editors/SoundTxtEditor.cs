using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.App;
using I18N;

namespace Assets.Scripts.Editors {
	public class SoundTxtEditor : EditorScreen {
		private string questionPath;
		public Button soundBtn;
		private AudioClip questionAudio;
		public List<InputField> answerTexts;
		public InputField titleText;
		public Text titleMaxChars;

		public void Start(){
			SetTexts();
			if(!IsEdit()){
				soundBtn.interactable = false;
				titleMaxChars.text = Common.TITLE_MAX_CHARS.ToString();
				CheckTicButton();
			}
		}
		
		public override List<string> GetQuestionAnswers() {
			return answerTexts.ConvertAll((InputField input) => input.text);
		}

		public override string GetQuestionText() {
			return questionPath;
		}

		public override string GetSecondQuestionText() {
			return "";
		}

		public override string GetQuestionTitle() {
			return titleText.text;
		}

		public override bool CanSubmitQuestion() {
			foreach(InputField answerText in answerTexts) {
				if(answerText.text.Trim().Length == 0) return false;
			}
			return questionAudio != null && titleText.text.Length != 0 && GetCorrectQuestion() != -1;
		}

		public override void BaseSetQuestion(Question q) {
			questionPath = q.question;
			questionAudio = Common.LoadSound(questionPath);
			titleText.text = q.title;

			for(int i = 0; i < answerTexts.Count; i++) {
				answerTexts[i].text = q.answers[i];
			}

			CheckTicButton();
		}

		public void CheckTitleLength(){
			int charsLeft = Common.TITLE_MAX_CHARS - titleText.text.Length;
			if(charsLeft < 0){
				titleText.text = titleText.text.Substring(0, Common.TITLE_MAX_CHARS);
				charsLeft = 0;
			}
			titleMaxChars.text = charsLeft.ToString();
		}

		public void PlaySound(){
			if(questionAudio != null)
				SoundController.GetController().PlayClip(questionAudio);
		}

		public void SoundClick(){
			soundFileBrowser.gameObject.SetActive(true);
		}

		public void SoundSubmit(){
			string path = soundFileBrowser.AddressPath + soundFileBrowser.FileName;
			if(path != null){
				questionAudio = Common.LoadSound(path);
				questionPath = path;
				soundBtn.interactable = true;
				CheckTicButton();
			}
		}

		// I18N **************************************************************************************

		void SetTexts() {
			SetWarningTexts();
			titleText.placeholder.GetComponentInChildren<Text>().text = I18n.Msg("editors.enterTitle");
			foreach(InputField ans in answerTexts) {
				ans.placeholder.GetComponentInChildren<Text>().text = I18n.Msg("editors.enterText");
			}
		}
	}
}