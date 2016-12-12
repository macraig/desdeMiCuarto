using System;
using UnityEngine;
using Assets.Scripts.App;
using Assets.Scripts.Editors;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Scripts.Editors.QuestionBoxes;
using I18N;

namespace Assets.Scripts.Menus {
	public class GeneralOverview : MonoBehaviour {
		public InputField title, author;
		public Button ticButton;
		public ScrollRect verticalScrollView, horizontalScrollView;
		public Image noGamesSign;
		public Text titleMaxChars;
		public GameObject deletePanel;

		private ClickGameInfo editGame, createdGame;
		private string creationScreen;

		private QuestionBox auxBox;
		private Question auxQuestion;

		private QuestionAnswerType questionType, answerType;
		private List<Question> questions = new List<Question>();
		private List<QuestionBox> questionBoxes = new List<QuestionBox>();

		public void Start(){
			SetTexts();
			if(!IsEdit()) {
				ticButton.interactable = false;
				noGamesSign.gameObject.SetActive(true);
				titleMaxChars.text = Common.TITLE_MAX_CHARS.ToString();
			}
		}

		public void EditGame(ClickGameInfo info) {
			Start();
			editGame = info;
			title.text = info.title;
			author.text = info.author;
			SetCreationScreen(TextFromEnum(info.questionType) + TextFromEnum(info.answerType) + "Editor");
			LoadQuestions(info);
			CheckTitleLength();
		}

		private void LoadQuestions(ClickGameInfo info) {
			string questionsPath = info.GetPath() + "/" + Common.QUESTIONS_FILE;
			Debug.Log("Loading questions from " + questionsPath);
			var aux = Common.Load(questionsPath);
			List<Question> q = (List<Question>) aux;
			q.ForEach(AddQuestion);
		}

		private string TextFromEnum(QuestionAnswerType type) {
			if(type == QuestionAnswerType.AUDIO_IMG) return "SoundImage";
			else if(type == QuestionAnswerType.TEXT) return "Txt";
			else if(type == QuestionAnswerType.AUDIO) return "Sound";
			else return "Img";
		}

		public void CheckTic(){
			SoundController.GetController ().PlayTypeSound ();
			ticButton.interactable = title.text.Length != 0 && author.text.Length != 0 && questions.Count != 0;
		}

		public void CheckTitleLength(){
			int charsLeft = Common.TITLE_MAX_CHARS - title.text.Length;
			if(charsLeft < 0){
				title.text = title.text.Substring(0, Common.TITLE_MAX_CHARS);
				charsLeft = 0;
			}
			titleMaxChars.text = charsLeft.ToString();
		}

		private bool IsEdit() { return editGame != null; }

		public void TicClick(){
			SoundController.GetController ().PlayTypeSound ();
			if(IsEdit()) createdGame = GameController.GetController().EditGame(editGame, title.text, author.text, questionType, answerType, questions);
			else createdGame = GameController.GetController().AddGame(title.text, author.text, questionType, answerType, questions);

			finishGamePanel.SetActive(true);
		}

		public void SetInfo(ClickGameInfo info){ editGame = info; }

		public void PlusClick(){
			SoundController.GetController ().PlayClickSound ();
			GameObject editor = ViewController.InstantiatePrefab("Editors/" + creationScreen);
			ViewController.FitObjectTo(editor, gameObject);
			editor.GetComponent<EditorScreen>().SetParent(this);
		}

		public void BackClick(){
			SoundController.GetController ().PlayToggleSound ();
			warningPanel.SetActive(true);
		}

		public void AddQuestion(Question q){
			q.SetQuestionNumber(questions.Count + 1);
			questions.Add(q);
			AddQuestionBox(q);
			CheckTic();
			noGamesSign.gameObject.SetActive(false);
		}

		public void EditQuestion(Question q){
			int questionIndex = q.number - 1;

			questions[questionIndex] = q;
			questionBoxes[questionIndex].SetQuestion(q);
		}

		public void EditClick(Question q){
			SoundController.GetController ().PlayClickSound ();
			GameObject editor = ViewController.InstantiatePrefab("Editors/" + creationScreen);
			ViewController.FitObjectTo(editor, gameObject);
			editor.GetComponent<EditorScreen>().SetQuestion(q, this);
		}

		public void DeleteClick(QuestionBox box, Question question) {
			SoundController.GetController ().PlayClickSound ();
			auxBox = box;
			auxQuestion = question;
			deletePanel.SetActive(true);
		}

		public void DeleteYesClick(){
			SoundController.GetController ().PlayClickSound ();
			questions.Remove(auxQuestion);
			questionBoxes.Remove(auxBox);
			Destroy(auxBox.gameObject);
			UpdateQuestionBoxes();
			CheckTic();
			if(questions.Count == 0) noGamesSign.gameObject.SetActive(true);
			deletePanel.SetActive(false);
		}

		public void DeleteNoClick(){
			SoundController.GetController ().PlayClickSound ();
			deletePanel.SetActive(false);
		}

		private void UpdateQuestionBoxes() {
			for(int i = 0; i < questionBoxes.Count; i++) {
				questionBoxes[i].UpdateNumber(i + 1);
				questions[i].SetQuestionNumber(i + 1);
			}
		}

		private void AddQuestionBox(Question q) {
			QuestionBox box;

			if(questionType == QuestionAnswerType.IMG || questionType == QuestionAnswerType.AUDIO_IMG){
				box = ViewController.QuestionBox(questionType, horizontalScrollView.transform.GetChild(0).GetChild(0));
			} else {
				box = ViewController.QuestionBox(questionType, verticalScrollView.transform.GetChild(0).GetChild(0));
			}

			box.SetOverview(this);
			box.SetQuestion(q);
			questionBoxes.Add(box);
		}

		private void SetQuestion(int number, Image questionPrefab, Question question) {
			questionPrefab.gameObject.SetActive(true);
			questionPrefab.transform.GetChild(0).GetComponent<Text>().text = number.ToString();
			questionPrefab.transform.GetChild(1).GetComponent<Text>().text = question.question;
		}

		public void SetCreationScreen(string creationScreen){
			this.creationScreen = creationScreen;
			questionType = QuestionFromString(creationScreen);
			answerType = AnswerFromString(creationScreen);
		}

		private QuestionAnswerType AnswerFromString(string s) {
			if(s.Contains("SoundImageTxt"))
				return QuestionAnswerType.TEXT;
			else if(s.Contains("SoundImg") || s.Contains("TxtImg"))
				return QuestionAnswerType.IMG;
			else
				return QuestionAnswerType.TEXT;
		}

		private QuestionAnswerType QuestionFromString(string s) {
			if(s.Contains("SoundImageTxt")) {
				horizontalScrollView.gameObject.SetActive(true);
				verticalScrollView.gameObject.SetActive(false);
				return QuestionAnswerType.AUDIO_IMG;
			}
			else if(s.Contains("SoundImg") || s.Contains("SoundTxt")) {
				horizontalScrollView.gameObject.SetActive(false);
				verticalScrollView.gameObject.SetActive(true);
				return QuestionAnswerType.AUDIO;
			}
			else if(s.Contains("ImgTxt")) {
				horizontalScrollView.gameObject.SetActive(true);
				verticalScrollView.gameObject.SetActive(false);
				return QuestionAnswerType.IMG;
			}
			else {
				horizontalScrollView.gameObject.SetActive(false);
				verticalScrollView.gameObject.SetActive(true);
				return QuestionAnswerType.TEXT;
			}
		}

		// FinishGame panel **********************************************************************************

		public GameObject finishGamePanel;
		public Text finishBackToMenu, finishPlay;

		public void FinishBackToMenu(){
			SoundController.GetController ().PlayClickSound ();
			ViewController.GetController().LoadGamesMenu(true);
		}

		public void FinishPlay(){
			SoundController.GetController ().PlayClickSound ();
			ViewController.GetController().LoadPlayersMenu(createdGame, true);
		}

		// Warning panel *************************************************************************************

		public GameObject warningPanel;
		public Text warningQuestion, warningInstructions, warningYes, warningNo;

		public void WarningYes(){
			SoundController.GetController ().PlayClickSound ();
			if(IsEdit()) ViewController.GetController().LoadGamesMenu(true);
			else ViewController.GetController().LoadChooseStyle();
		}

		public void WarningNo(){
			SoundController.GetController ().PlayClickSound ();
			warningPanel.SetActive(false);
		}

		// I18N **********************************************************************************************

		public Text addBtnText, noQuestionsSignText,noQuestionsSignWarningText;
		public Text deleteText, deleteYes, deleteNo;

		public void SetTexts(){
			title.placeholder.GetComponentInChildren<Text>().text = I18n.Msg("generalOverview.enterTitle");
			author.placeholder.GetComponentInChildren<Text>().text = I18n.Msg("generalOverview.enterAuthor");
			addBtnText.text = I18n.Msg("generalOverview.addBtn");
			noQuestionsSignText.text = I18n.Msg("generalOverview.noQuestionsSign");
			noQuestionsSignWarningText.text = I18n.Msg("generalOverview.noQuestionsWarning");
			deleteText.text = I18n.Msg("generalOverview.delete");

			deleteYes.text = I18n.Msg("common.yes");
			deleteNo.text = I18n.Msg("common.no");

			warningQuestion.text = I18n.Msg("warningPanel.leaveGame");
			warningInstructions.text = I18n.Msg("warningPanel.instructions");
			warningYes.text = I18n.Msg("common.yes");
			warningNo.text = I18n.Msg("common.no");

			finishBackToMenu.text = I18n.Msg("generalOverview.finish.back");
			finishPlay.text = I18n.Msg("generalOverview.finish.play");
		}
	}
}