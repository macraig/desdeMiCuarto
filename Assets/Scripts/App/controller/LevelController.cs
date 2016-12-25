using UnityEngine;
using System.Collections;


namespace Assets.Scripts.App{

	public abstract class LevelController : MonoBehaviour{

		public abstract void NextChallenge ();
		public abstract void ShowHint ();
		public abstract void InitGame ();
		public abstract void RestartGame ();

		public void LogAnswer(bool isCorrect){
			MetricsController.instance.LogAnswer (isCorrect);
		}

		public void LogHint(){
			MetricsController.instance.LogHint ();
		}

		public void LogMetrics(int lapsedSeconds,int minSeconds,int pointsPerSecond,int pointsPerError){
			MetricsController.instance.GameFinished (minSeconds, pointsPerSecond, pointsPerError);
		}

		public void CheckIfLevelUp(){
			if (SettingsController.instance.GetMode () == 1) {
				int currentLevel = AppController.instance.GetCurrentLevel ();
				if (currentLevel==AppController.instance.GetMaxLevelPossible()&&currentLevel!=16) {
					AppController.instance.SetMaxLevelPossible(currentLevel + 1);
				}

			}
		}

		public void EndGame(int minSeconds,int pointsPerSecond,int  pointsPerError){
			CheckIfLevelUp();
			MetricsController.instance.GameFinished(minSeconds, pointsPerSecond, pointsPerError);
			ViewController.instance.LoadLevelComplete();
		}

		public void UnloadLevel(){
			Destroy (gameObject);
		}
	}
}