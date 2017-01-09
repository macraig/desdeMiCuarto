using Assets.Scripts.App;
using Assets.Scripts.Metrics;
using Assets.Scripts.Sound;
using Assets.Scripts.Common;
using UnityEngine;

namespace Assets.Scripts.Games
{
    // His childs have to implement a method called NextChallenge that
    // recieve whatever it needs
    public abstract class LevelView : MonoBehaviour
    {
//      
		//Ingame Menu Panel
		public GameObject menuPanel;
		//Explanation Panel
		public GameObject explanationPanel;

		public void OnClickMenuBtn(){
			PlaySoundClick();
			ShowInGameMenu();
		}

		public void ShowInGameMenu(){
			menuPanel.SetActive (true);
		}

		public void HideInGameMenu(){
			PlaySoundClick ();
			menuPanel.SetActive (false);
		}

		public void OnClickInstructionsButton(){
			PlaySoundClick ();
			HideInGameMenu ();
			ShowExplanation ();

		}

		public void OnClickRestartButton(){
			PlaySoundClick ();
			HideInGameMenu ();
			RestartGame ();

		}

		public void OnClickExitGameButton(){
			PlaySoundClick ();
			HideInGameMenu ();
			ExitGame ();

		}

		internal void ShowExplanation(){
			explanationPanel.SetActive(true);
			//play al sonido de instructions
		}


		public void HideExplanation(){
			PlaySoundClick ();
			explanationPanel.SetActive (false);

		}



		internal void ExitGame(){
//			MetricsController.GetController().DiscardCurrentMetrics();
			ViewController.GetController().LoadMainMenu();
		}

        // This method have to restart the view of the game to the initial state
        public abstract void RestartGame();

        // This method have to be called when the user clicks menuButton
       
        public void OnClickSurrender()
        {
            PlaySoundClick();
//            LevelController.GetLevelController().ResolveExercise();

            MetricsController.GetController().OnSurrender();
        }
        // This method have to be called when the user clicks a button
        internal void PlaySoundClick()
        {
            SoundController.GetController().PlayClickSound();
        }
        // This method have to be called when the answers is correct
        internal void PlayRightSound()
        {
            SoundController.GetController().PlayRightAnswerSound();
        }
        // This method have to be called when the answers is incorrect
        internal void PlayWrongSound()
        {
            SoundController.GetController().PlayFailureSound();
        }
			

        public void OnClickNextButton()
        {
            PlaySoundClick();
            LevelController.GetLevelController().NextChallenge();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { OnClickMenuBtn(); }
        }
    }
}
