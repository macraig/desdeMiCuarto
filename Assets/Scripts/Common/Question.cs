using System;
using System.Collections.Generic;

[System.Serializable]
public class Question {
	//title is only for sound questions.
	public string title;
	public int number;
	public string question;
	//secondQuestion is only for sound+image questions. Second question is sound path.
	public string secondQuestion;
	public List<string> answers;
	public int correctAnswer;

	public Question() { }

	public Question(string title, string question, string secondQuestion, List<string> answers, int correctAnswer){
		this.question = question;
		this.secondQuestion = secondQuestion;
		this.answers = answers;
		this.correctAnswer = correctAnswer;
		this.title = title;
		number = -1;
	}

	public Question SetQuestionNumber(int number){
		this.number = number;
		return this;
	}

	public string GetQuestionHtml(QuestionAnswerType questionType) {
		//TODO MARIA -> HTML FOR EACH TYPE.
		switch(questionType) {
		case QuestionAnswerType.AUDIO:
			return "";
		case QuestionAnswerType.AUDIO_IMG:
			return "";
		case QuestionAnswerType.IMG:
			//Para loadear img -> Common.LoadImage(question)
			return "";
		default:
			return "<h4>" + number + ". " + question + "</h1>\n";
		}
	}

	public string GetAnswersHtml(QuestionAnswerType answerType) {
		//TODO MARIA -> HTML FOR EACH TYPE.
		string s = "<ol>";
		foreach(string a in answers) {
			s += "<li>";
			switch(answerType) {
			case QuestionAnswerType.AUDIO:
				s += "";
				break;
			case QuestionAnswerType.AUDIO_IMG:
				s += "";
				break;
			case QuestionAnswerType.IMG:
				s += "";
				break;
			default:
				s += a;
				break;
			}

			s += "</li>";
		}

		s += "</ol>";
		return s;
	}
}