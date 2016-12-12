using System;
using Assets.Scripts.App;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menus {
	public class GameBox : MonoBehaviour {
		public Text title, author;
		private ClickGameInfo info;
		private GamesMenu menu;

		public void SetGame(GamesMenu menu, ClickGameInfo info){
			this.menu = menu;
			this.info = info;
			title.text = info.title;
			author.text = info.author;
		}

		public void EditGame(){
			SoundController.GetController ().PlayClickSound ();
			menu.EditGame(info);
		}

		public void DeleteGame(){
			SoundController.GetController ().PlayClickSound ();
			menu.DeleteGame(this, info);
		}

		public void OnValueChanged(){
			SoundController.GetController ().PlayBlopSound ();
		}
	
		}
}