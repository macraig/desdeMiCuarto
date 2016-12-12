using System;
using Assets.Scripts.Editors;

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using I18N;

namespace Assets.Scripts.Editors {
	public class ImgTextEditor : EditorScreen {
		public Image questionImg;
		private string questionPath;
		public Text questionPlaceholder;
		public List<InputField> answerTexts;

		public void Start(){
			SetTexts();
			imageFileBrowser.m_SubmitBtn.onClick.AddListener(() => ImageClick());
			if(!IsEdit()) {
				questionPath = "";
				CheckTicButton();
			}
		}

		public override string GetQuestionTitle() {
			return "";
		}

		public override List<string> GetQuestionAnswers() {
			return answerTexts.ConvertAll((InputField txt) => txt.text);
		}

		public override string GetSecondQuestionText() {
			return "";
		}

		public override string GetQuestionText() {
			return questionPath;
		}

		public override bool CanSubmitQuestion() {
			foreach(InputField answerText in answerTexts) {
				if(answerText.text.Trim().Length == 0) return false;
			}
			return File.Exists(questionPath) && GetCorrectQuestion() != -1;
		}

		public override void BaseSetQuestion(Question q) {
			questionPath = q.question;
			questionImg.sprite = Common.LoadImage(questionPath);
			questionPlaceholder.gameObject.SetActive(false);

			for(int i = 0; i < answerTexts.Count; i++) {
				answerTexts[i].text = q.answers[i];
			}
			CheckTicButton();
		}

		public void ImageClick(){
			imageFileBrowser.gameObject.SetActive(true);
		}

		public void ImageSubmit(){
			string path = imageFileBrowser.AddressPath + imageFileBrowser.FileName;
			Debug.Log("path: " + path);
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
				ans.placeholder.GetComponent<Text>().text = I18n.Msg("editors.enterText");
			}
		}
	}
}