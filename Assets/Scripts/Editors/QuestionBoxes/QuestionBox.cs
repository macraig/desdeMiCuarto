using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Menus;

namespace Assets.Scripts.Editors.QuestionBoxes {
	public abstract class QuestionBox : MonoBehaviour {
		public Button editBtn, deleteBtn;
		public Text number;
		private Question question;
		private GeneralOverview overview;

		public void SetOverview(GeneralOverview overview){
			this.overview = overview;
		}

		public void EditQuestion(){
			overview.EditClick(question);
		}

		public void DeleteQuestion(){
			overview.DeleteClick(this, question);
		}

		public virtual void SetQuestion(Question q){
			question = q;
			number.text = q.number.ToString();
		}

		public void UpdateNumber(int number) {
			question.number = number;
			this.number.text = number.ToString();
		}
	}
}