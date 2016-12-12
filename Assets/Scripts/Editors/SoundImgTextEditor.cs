using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Assets.Scripts.App;
using System.IO;
using I18N;

namespace Assets.Scripts.Editors {
	public class SoundImgTextEditor : EditorScreen {
		public Image questionImg;
		private string questionPath;
		public Text questionPlaceholder;

		private string secondQuestionPath;
		public Button soundBtn;
		private AudioClip secondQuestionAudio;

		public List<InputField> answerTexts;

		public void Start(){
			SetTexts();
			if(!IsEdit()){
				soundBtn.interactable = false;
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
			return secondQuestionPath;
		}

		public override string GetQuestionTitle() {
			return "";
		}

		public override bool CanSubmitQuestion() {
			foreach(InputField answerText in answerTexts) {
				if(answerText.text.Trim().Length == 0) return false;
			}
			return secondQuestionAudio != null && GetCorrectQuestion() != -1 && File.Exists(questionPath);
		}

		public override void BaseSetQuestion(Question q) {
			questionPath = q.question;
			questionImg.sprite = Common.LoadImage(questionPath);
			questionPlaceholder.gameObject.SetActive(false);

			secondQuestionPath = q.secondQuestion;
			secondQuestionAudio = Common.LoadSound(secondQuestionPath);

			for(int i = 0; i < answerTexts.Count; i++) {
				answerTexts[i].text = q.answers[i];
			}

			CheckTicButton();
		}

		public void PlaySound(){
			if(secondQuestionAudio != null)
				SoundController.GetController().PlayClip(secondQuestionAudio);
		}

		public void SoundClick(){
			soundFileBrowser.gameObject.SetActive(true);
		}

		public void SoundSubmit(){
			string path = soundFileBrowser.AddressPath + soundFileBrowser.FileName;
			if(path != null){
				secondQuestionAudio = Common.LoadSound(path);
				secondQuestionPath = path;
				soundBtn.interactable = true;
				CheckTicButton();
			}
		}

		public void ImageClick(){
			imageFileBrowser.gameObject.SetActive(true);
		}

		public void ImageSubmit(){
			string path = imageFileBrowser.AddressPath + imageFileBrowser.FileName;
			if(path != null){
				questionImg.sprite = Common.LoadImage(path);
				questionPath = path;
				questionPlaceholder.gameObject.SetActive(false);
				CheckTicButton();
			}
		}

		// I18N **************************************************************************************

		void SetTexts() {
			SetWarningTexts();
			questionPlaceholder.text = I18n.Msg("editors.loadImage");
			foreach(InputField ans in answerTexts) {
				ans.placeholder.GetComponentInChildren<Text>().text = I18n.Msg("editors.enterText");
			}
		}
	}
}