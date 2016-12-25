using UnityEngine;
using System.Collections;

namespace Assets.Scripts.App{

public class GameMenuController : MonoBehaviour {

		public static GameMenuController instance;

		//Awake is always called before any Start functions
		void Awake()
		{
			if (instance == null)           
				instance = this;
			else if (instance != this)
				Destroy(gameObject);


			DontDestroyOnLoad(gameObject);
		}

		public void BackToGame(){
			//todo
		}

		public void RestartGame(){
			//todo
		}


		public void ShowInstructions(){
			//todo: call the ShowInstructions method in the LevelController
		}

		public void BackToMenu(){
			//todo
		}



	}
}