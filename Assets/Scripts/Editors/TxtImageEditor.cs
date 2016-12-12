using System;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using I18N;

namespace Assets.Scripts.Editors {
	public class TxtImageEditor : EditorScreen {
		public InputField questionText;
		public List<Image> answerImgs;
		public List<Text> answerPlaceholders;
		private List<string> answerPaths;

		public void Start(){
			SetTexts();
			if(!IsEdit()) {
				answerPaths = new List<string>();
				for(int i = 0; i < answerImgs.Count; i++)
					answerPaths.Add("");
				ticButton.interactable = false;
			}
		}
		
		public override List<string> GetQuestionAnswers() {
			return answerPaths;
		}
		public override string GetQuestionText() {
			return questionText.text;
		}

		public override string GetSecondQuestionText() {
			return "";
		}

		public override string GetQuestionTitle() {
			return "";
		}

		public override bool CanSubmitQuestion() {
			foreach(string answerPath in answerPaths) {
				if(!File.Exists(answerPath)) return false;
			}
			return questionText.text.Trim().Length != 0 && GetCorrectQuestion() != -1;
		}

		public override void BaseSetQuestion(Question q) {
			answerPaths = q.answers;
			questionText.text = q.question;

			LoadImages();

			CheckTicButton();
		}

		private void LoadImages() {
			for(int i = 0; i < answerPaths.Count; i++) {
				answerImgs[i].sprite = Common.LoadImage(answerPaths[i]);
				answerPlaceholders[i].gameObject.SetActive(false);
			}
		}

		public void ImageClick(int index){
			currentIndex = index;
			imageFileBrowser.gameObject.SetActive(true);
		}

		public void ImageSubmit(){
			string path = imageFileBrowser.AddressPath + imageFileBrowser.FileName;
			if(path != null){
				answerImgs[currentIndex].sprite = Common.LoadImage(path);
				answerPaths[currentIndex] = path;
				answerPlaceholders[currentIndex].gameObject.SetActive(false);
				CheckTicButton();
			}
		}

		// I18N **************************************************************************************

		void SetTexts() {
			SetWarningTexts();
			questionText.placeholder.GetComponentInChildren<Text>().text = I18n.Msg("editors.enterQuestion");
			foreach(Text ans in answerPlaceholders) {
				ans.text = I18n.Msg("editors.loadImage");
			}
		}
	}
}