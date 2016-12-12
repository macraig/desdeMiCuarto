using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Scripts.App;
using I18N;
using Assets.Scripts.Settings;

namespace Assets.Scripts.Game {
	public abstract class ClickGame : MonoBehaviour, Internationalizable {
		public Image currentPlayerImage;
		public Button ticBtn;
		public List<Button> answerButtons;

		protected List<Question> questions;
		private List<int> spriteNumbers;
		private List<string> playerNames;
		private List<int> scores;
		private List<Color> originalColors;

		private ClickGameInfo info;
		private bool isRandom;

		private int currentPlayer = 0;
		private int currentQuestion = 0;
		private int correctsInARow = 0;
		private int currentRound = 0;

		private int correctAnswer;

		private int totalRounds;

		private Sprite[] characterSprites, characterButtonSprites, characterWinSprites;

		public void StartGame(ClickGameInfo info, bool isRandom, List<int> spriteNumbers, List<string> playerNames){
			this.spriteNumbers = spriteNumbers;
			this.playerNames = playerNames;
			this.info = info;
			this.isRandom = isRandom;
			SetTexts();

			characterSprites = Resources.LoadAll<Sprite>("Sprites/characters");
			characterButtonSprites = Resources.LoadAll<Sprite>("Sprites/charactersTurnBtn");
			characterWinSprites = Resources.LoadAll<Sprite>("Sprites/charactersWin");

			ShowQuestionType(info.questionType);
			ShowAnswerType(info.answerType);

			questions = (List<Question>) Common.Load(info.GetPath() + "/" + Common.QUESTIONS_FILE);
			if(isRandom) questions = Randomizer.RandomizeList(questions);

			ticBtn.gameObject.SetActive(false);

			currentPlayerImage.sprite = characterButtonSprites[spriteNumbers[currentPlayer]];

			totalRounds = questions.Count / (playerNames.Count * 3);
			if(!IsOnePlayer() && totalRounds == 0) Debug.Log("This game doesn't even have one round.....");

			SetOriginalColors();

			StartScorePanel();
			StartScores();
			FirstRound();
		}

		protected void SetCorrectAnswer(int correctAnswer){
			this.correctAnswer = correctAnswer;
		}

		private void SetOriginalColors() {
			if(originalColors == null){
				originalColors = new List<Color>();
				foreach(var ans in answerButtons) {
					originalColors.Add(ans.image.color);
				}
			}

			for(int i = 0; i < answerButtons.Count; i++) {
				answerButtons[i].image.color = originalColors[i];
			}
		}

		public abstract void ShowQuestionType(QuestionAnswerType questionType);

		public abstract void ShowAnswerType(QuestionAnswerType answerType);

		public void Answer(int index){
			ticBtn.gameObject.SetActive(true);

			foreach(Button ans in answerButtons) ans.enabled = false;

			Question q = questions[currentQuestion];

			if(index == correctAnswer)
				Correct(answerButtons[index]);
			else
				Wrong(answerButtons[q.correctAnswer], answerButtons[index]);
		}

		public void Correct(Button correct){
			correctsInARow++;
			correct.image.color = Common.CORRECT_COLOR;

			scores[currentPlayer]++;
		}

		public void Wrong(Button correct, Button wrong){
			correct.image.color = Common.CORRECT_COLOR;
			wrong.image.color = Common.WRONG_COLOR;

			correctsInARow = -1;
		}

		private void StartScores() {
			scores = new List<int>();
			for(int i = 0; i < playerNames.Count; i++) {
				scores.Add(0);
			}
		}

		protected void FirstRound() {
			ShowTurnPanelWithCurrentPlayer();
			SetQuestion(questions[currentQuestion]);
		}

		public abstract void SetQuestion(Question question);

		public void NextClick(){
			ticBtn.gameObject.SetActive(false);
			SetOriginalColors();
			foreach(Button ans in answerButtons) ans.enabled = true;

			if(correctsInARow == -1 && IsOnePlayer()) {
				EndGame();
				return;
			}

			if(correctsInARow == 3 || correctsInARow == -1){
				correctsInARow = 0;
				if(NextPlayer()) return;
				ShowTurnPanelWithCurrentPlayer();
			}
			NextQuestion();
		}

		//Returns if game ended.
		protected bool NextPlayer() {
			currentPlayer++;
			if(currentPlayer == playerNames.Count) {
				currentPlayer = 0;
				currentRound++;
			}

			if(currentRound == totalRounds && !IsOnePlayer()){
				EndGame();
				return true;
			}

			currentPlayerImage.sprite = characterButtonSprites[spriteNumbers[currentPlayer]];
			return false;
		}

		protected void NextQuestion(){
			currentQuestion++;
			if(currentQuestion == questions.Count)
				EndGame();
			else
				SetQuestion(questions[currentQuestion]);
		}

		bool IsOnePlayer() { return playerNames.Count == 1; }

		protected static string GetText(string txt) {
			return Settings.Settings.Instance().UppercaseOn() ? txt.ToUpper() : txt;
		}

		// Score Panel ******************************************************************************************

		public GameObject scorePanel;
		public Button endGameBtn;
		public GameObject playersScoresPanel;
		private List<GameObject> scoreBoxes;
		private List<Text> scoreTexts;
		public Text scorePanelTitleText;

		private void EndGame() {
			turnPanel.gameObject.SetActive(false);
			ShowScorePanel(true);
		}

		private void StartScorePanel(){
			scoreBoxes = new List<GameObject>();
			scoreTexts = new List<Text>();
			for(int i = 0; i < playerNames.Count; i++) {
				GameObject box = ViewController.PlayerScoreBox(playersScoresPanel.transform);
				scoreBoxes.Add(box);

				box.transform.GetChild(0).GetComponent<Image>().sprite = characterSprites[spriteNumbers[i]];
				box.transform.GetChild(2).GetComponent<Text>().text = playerNames[i];
				scoreTexts.Add(box.transform.GetChild(3).GetComponentInChildren<Text>());
			}
		}

		public void ScorePanelClick(){
			if(!endGameBtn.IsActive()) scorePanel.SetActive(false);
		}

		public void ShowScorePanel(bool endGame){
			int maxScoreIndex = 0;
			for(int i = 0; i < scoreTexts.Count; i++) {
				scoreTexts[i].text = scores[i].ToString();

				if(scores[i] > scores[maxScoreIndex]) maxScoreIndex = i;
			}

			scorePanel.SetActive(true);
			endGameBtn.gameObject.SetActive(endGame);

			if(endGame) {
				if(IsOnePlayer()){
					scorePanelTitleText.text = I18n.Msg("clickGame.scorePanel.onePlayerMessage");
				} else {
					List<int> maxScores = new List<int>();
					for(int i = 0; i < scores.Count; i++) {
						if(scores[i] == scores[maxScoreIndex]) {
							maxScores.Add(i);
							scoreBoxes[i].transform.GetChild(0).GetComponent<Image>().sprite = characterWinSprites[spriteNumbers[i]];
						}
					}
					if(maxScores.Count == 1) {
						scorePanelTitleText.text = I18n.Msg("clickGame.scorePanel.multiplayerMessage", playerNames[maxScoreIndex]);
					} else {
						string s = string.Join(", ", maxScores.ConvertAll((int input) => playerNames[input]).ToArray());
						scorePanelTitleText.text = I18n.Msg("clickGame.scorePanel.tieMessage", s);
					}
				}
			}
		}

		public void EndGameClick(){
			ViewController.GetController().LoadGamesMenu(false);
		}

		// Turn Panel ********************************************************************************************

		public GameObject turnPanel;
		public Image turnPanelCharacter;
		public Text turnPanelCharacterName;
		public Text turnPanelTurnOfText;

		public void ShowTurnPanelWithCurrentPlayer(){
			if(!IsOnePlayer()) {
				turnPanel.SetActive(true);
				turnPanelCharacter.sprite = characterSprites[spriteNumbers[currentPlayer]];
				turnPanelCharacterName.text = playerNames[currentPlayer];
			}
		}

		public void TurnPanelClick(){
			turnPanel.SetActive(false);
		}

		// InGame Menu ******************************************************************************************

		public GameObject menuPanel;
		public Text menuText;

		public void MenuClick(){
			menuText.text = "";
			menuPanel.SetActive(true);
		}

		public void BackToGame(){
			menuPanel.SetActive(false);
		}

		public void Restart(){
			ViewController.GetController().LoadGameScreen(spriteNumbers, playerNames, info, isRandom);
		}

		public void SettingsClick(){
			ViewController.GetController().ShowSettingsScreen(gameObject);
		}

		public void BackToMenu(){
			ViewController.GetController().LoadGamesMenu(false);
		}

		public void MenuButtonHover(string textId){
			menuText.text = I18n.Msg(textId);
		}

		// I18N ***************************************************************************************************

		public Text endGameText;

		public void SetTexts(){
			scorePanelTitleText.text = I18n.Msg("clickGame.scorePanel.title");
			turnPanelTurnOfText.text = I18n.Msg("clickGame.turnPanel.turnOf");
			endGameText.text = I18n.Msg("clickGame.scorePanel.endGame");
		}
	}
}