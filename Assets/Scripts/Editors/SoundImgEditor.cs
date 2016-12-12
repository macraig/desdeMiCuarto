using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using Assets.Scripts.App;
using I18N;

namespace Assets.Scripts.Editors {
	public class SoundImgEditor : EditorScreen {
		private string questionPath;
		private AudioClip questionAudio;
		public Button soundBtn;
		public List<Image> answerImgs;
		public InputField titleText;
		public List<Text> answerPlaceholders;
		private List<string> answerPaths;
		public Text titleMaxChars;

		public void Start(){
			SetTexts();
			if(!IsEdit()) {
				soundBtn.interactable = false;
				titleMaxChars.text = Common.TITLE_MAX_CHARS.ToString();
				answerPaths = new List<string>();
				for(int i = 0; i < answerImgs.Count; i++)
					answerPaths.Add("");
				CheckTicButton();
			}
		}
		
		public override List<string> GetQuestionAnswers() {
			return answerPaths;
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

		public void CheckTitleLength(){
			int charsLeft = Common.TITLE_MAX_CHARS - titleText.text.Length;
			if(charsLeft < 0){
				titleText.text = titleText.text.Substring(0, Common.TITLE_MAX_CHARS);
				charsLeft = 0;
			}
			titleMaxChars.text = charsLeft.ToString();
		}

		public override bool CanSubmitQuestion() {
			foreach(string answerPath in answerPaths) {
				if(!File.Exists(answerPath)) return false;
			}
			return questionAudio != null && titleText.text.Length != 0 && GetCorrectQuestion() != -1;
		}

		public override void BaseSetQuestion(Question q) {
			questionPath = q.question;
			answerPaths = q.answers;
			questionAudio = Common.LoadSound(questionPath);

			LoadImages();

			titleText.text = q.title;

			CheckTicButton();
		}

		private void LoadImages() {
			for(int i = 0; i < answerPaths.Count; i++) {
				answerImgs[i].sprite = Common.LoadImage(answerPaths[i]);
				answerPlaceholders[i].gameObject.SetActive(false);
			}
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
			titleText.placeholder.GetComponentInChildren<Text>().text = I18n.Msg("editors.enterTitle");
			foreach(Text ans in answerPlaceholders) {
				ans.text = I18n.Msg("editors.loadImage");
			}
		}
	}
}