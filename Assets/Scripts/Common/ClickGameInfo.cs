using System;
using System.Collections.Generic;

[System.Serializable]
public class ClickGameInfo : Printable {
	public string id;
	public string author;
	public string title;
	public QuestionAnswerType questionType;
	public QuestionAnswerType answerType;

	public ClickGameInfo() { }

	public ClickGameInfo(string id, string author, string title, QuestionAnswerType questionType, QuestionAnswerType answerType){
		this.id = id;
		this.answerType = answerType;
		this.questionType = questionType;
		this.title = title;
		this.author = author;
	}

	public string GetPath(){
		return Common.GetBaseDir() + "/" + id;
	}

	//TODO MARIA -> HTML ?
	public string GetAsHtml() {
		string html = Prints.InitFile();

		html += Prints.AddTitle(author + "-" + title);
		html += Prints.AddLine();

		List<Question> questions = (List<Question>) Common.Load(GetPath() + "/" + Common.QUESTIONS_FILE);

		foreach(Question q in questions) {
			html += q.GetQuestionHtml(questionType);
			html += q.GetAnswersHtml(answerType);
			html += Prints.AddLine();
		}

		html += Prints.CloseFile();
		return html;
	}

	public string GetFileName() {
		return author + "-" + title + "-" + id + ".html";
	}
}