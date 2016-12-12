using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace Assets.Scripts.App {
	public class GameController : MonoBehaviour {
		private static GameController controller;
		private ClickGamesIndex gamesIndex;

		void Awake() {
			if (controller == null) controller = this;
			else if (controller != this) Destroy(gameObject);
			DontDestroyOnLoad(this);
		}

		void Start() {
			gamesIndex = ClickGamesIndex.LoadIndex();

			CheckIndexPlus();
		}

		// Check which games have folders, add them to the index, change their questions.gd in case they have file paths.
		void CheckIndexPlus() {
			if(!File.Exists(Common.INDEX_PLUS_FILE)) return;

			ClickGamesIndex plusIndex = (ClickGamesIndex) Common.Load(Common.INDEX_PLUS_FILE);

			plusIndex.GetGames().ForEach((ClickGameInfo info) => {
				if(Directory.Exists(info.GetPath())){
					gamesIndex.AddGame(info);

					string questionsPath = info.GetPath() + "/" + Common.QUESTIONS_FILE;
					Debug.Log("Loading questions from " + questionsPath);
					List<Question> q = (List<Question>) Common.Load(questionsPath);

					List<Question> newQuestions = q.ConvertAll((Question input) => {
						string question = ReplaceWithNewPath(info, input.question);
						string secondQuestion = ReplaceWithNewPath(info, input.secondQuestion);
						List<string> answers = input.answers.ConvertAll((ans) => ReplaceWithNewPath(info, ans));

						return new Question(input.title, question, secondQuestion, answers, input.correctAnswer).SetQuestionNumber(input.number);
					});

					Common.Save(questionsPath, newQuestions);
				}
			});

			File.Delete(Common.INDEX_PLUS_FILE);
		}

		string ReplaceWithNewPath(ClickGameInfo info, string s) {
			int index = s.IndexOf("/" + Common.GAME_NAME + "/" + info.id);
			if(index == -1) return s;
			string oldDataPersistentPath = s.Substring(0, index);
			return s.Replace(oldDataPersistentPath, Common.DocumentsPath());
		}

		// Add Game ************************************************************************************************************

		public ClickGameInfo AddGame(string title, string author, QuestionAnswerType questionType, QuestionAnswerType answerType, List<Question> questions) {
			string id = GenerateId(title, author);
			Debug.Log("Add game with id " + id);
			ClickGameInfo gameInfo = new ClickGameInfo(id, author, title, questionType, answerType);
			SaveQuestions(gameInfo, questions);
			gamesIndex.AddGame(gameInfo);

			return gameInfo;
		}

		private void SaveQuestions(ClickGameInfo gameInfo, List<Question> questions) {
			List<Question> questionsToSave = new List<Question>();
			string gamePath = gameInfo.GetPath();
			Debug.Log("Saving game to path " + gamePath);
			if(!Directory.Exists(gamePath)) Directory.CreateDirectory(gamePath);

			foreach(Question question in questions) {
				questionsToSave.Add(DoStuffWithQuestionAndGetNew(gameInfo, question));
			}

			Common.Save(gamePath + "/" + Common.QUESTIONS_FILE, questionsToSave);
		}

		//Very important method in which we copy new stuff into persistent data path.
		private Question DoStuffWithQuestionAndGetNew(ClickGameInfo gameInfo, Question question) {
			string q = question.question;
			string sq = question.secondQuestion;
			List<string> answers = question.answers;

			if(gameInfo.questionType == QuestionAnswerType.AUDIO || gameInfo.questionType == QuestionAnswerType.IMG || gameInfo.questionType == QuestionAnswerType.AUDIO_IMG){
				string fileName = question.number.ToString() + Path.GetExtension(question.question);
				q = CopyQuestion(gameInfo, fileName, question.question);
			}

			if(gameInfo.questionType == QuestionAnswerType.AUDIO_IMG){
				string fileName = question.number.ToString() + Path.GetExtension(question.secondQuestion);
				sq = CopyQuestion(gameInfo, fileName, question.secondQuestion);
			}

			if(gameInfo.answerType == QuestionAnswerType.AUDIO || gameInfo.answerType == QuestionAnswerType.IMG){
				answers = CopyAnswers(gameInfo, question);
			}

			return new Question(question.title, q, sq, answers, question.correctAnswer).SetQuestionNumber(question.number);
		}

		private List<string> CopyAnswers(ClickGameInfo gameInfo, Question question) {
			List<string> answers = new List<string>();

			for(int i = 0; i < question.answers.Count; i++) {
				string answer = question.answers[i];
				string fileName = question.number.ToString() + "-" + i + Path.GetExtension(answer);
				answers.Add(CopyQuestion(gameInfo, fileName, answer));
			}

			return answers;
		}

		private string CopyQuestion(ClickGameInfo gameInfo, string fileName, string from) {
			string newQuestion = gameInfo.GetPath() + "/" + fileName;
			File.Copy(from, newQuestion);
			return newQuestion;
		}

		// End Add Game ************************************************************************************************************

		// Edit Game ***************************************************************************************************************

		public ClickGameInfo EditGame(ClickGameInfo oldGame, string title, string author, QuestionAnswerType questionType, QuestionAnswerType answerType, List<Question> questions) {
			Debug.Log("Edit game with id " + oldGame.id);
			ClickGameInfo gameInfo = new ClickGameInfo(GenerateId(title, author), author, title, questionType, answerType);
			string oldPath = oldGame.GetPath();
			string editPath = MoveToEditPath(oldPath);
			List<Question> newQuestions = QuestionsToEditPath(oldPath, editPath, questions);

			SaveQuestions(gameInfo, newQuestions);

			Directory.Delete(editPath, true);

			gamesIndex.EditGame(oldGame, gameInfo);

			return gameInfo;
		}

		private List<Question> QuestionsToEditPath(string oldPath, string editPath, List<Question> questions) {
			return questions.ConvertAll((Question q) => {
				string question = q.question.Replace(oldPath, editPath);
				string secondQuestion = q.secondQuestion.Replace(oldPath, editPath);

				List<string> answers = q.answers.ConvertAll((string answer) => answer.Replace(oldPath, editPath));

				return new Question(q.title, question, secondQuestion, answers, q.correctAnswer).SetQuestionNumber(q.number);
			});
		}

		private string MoveToEditPath(string pathToMove) {
			string editPath = pathToMove + Common.EDIT_SUFFIX;
			if(Directory.Exists(editPath)) Directory.Delete(editPath, true);
			Directory.Move(pathToMove, editPath);

			if(Directory.Exists(pathToMove)) Debug.Log("Path to move should not exist......");

			return editPath;
		}

		// End Edit Game ************************************************************************************************************

		public void DeleteGame(ClickGameInfo game) {
			Directory.Delete(game.GetPath(), true);
			gamesIndex.DeleteGame(game);
		}

		public List<ClickGameInfo> GetGames() {
			return gamesIndex.GetGames();
		}

		private string GenerateId(string title, string author) {
			return title + "-" + author + "-" + DateTime.Now.Ticks;
		}

		public static GameController GetController() { return controller; }
	}
}