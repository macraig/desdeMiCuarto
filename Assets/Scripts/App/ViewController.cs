using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using Assets.Scripts.Menus;
using Assets.Scripts.Game;
using Assets.Scripts.Settings;

namespace Assets.Scripts.App {
    public class ViewController : MonoBehaviour {
        private static ViewController viewController;

        public GameObject viewPanel;
        private GameObject currentGameObject;

        void Awake() {
            if (viewController == null) viewController = this;
            else if (viewController != this) Destroy(gameObject);      
            DontDestroyOnLoad(this);
        }  

		void Start() {
            LoadCover();
		}  

        private GameObject LoadPrefab(string name) {
            return Resources.Load<GameObject>("Prefabs/" + name);
        }

		internal static GameObject InstantiatePrefab(string name) {
			return Instantiate(Resources.Load<GameObject>("Prefabs/" + name));
		}

        private void ChangeCurrentObject(GameObject newObject) {
            this.gameObject.GetComponent<CanvasScaler>().matchWidthOrHeight = 1f;
            GameObject child = Instantiate(newObject);
            FitObjectTo(child, viewPanel);
            Destroy(currentGameObject);
            currentGameObject = child;
        }

		internal static void FitObjectTo(GameObject child, GameObject parent) {
			child.transform.SetParent(parent.transform, true);
			child.transform.localPosition = Vector3.zero;
			child.GetComponent<RectTransform>().offsetMax = Vector2.zero;
			child.GetComponent<RectTransform>().offsetMin = Vector2.zero;
			child.transform.localScale = Vector3.one;
		}


		internal void LoadCover() {
			ChangeCurrentObject(LoadPrefab("ClickCover"));
		}

		public void LoadGameScreen(List<int> spriteNumbers, List<string> playerNames, ClickGameInfo info, bool isRandom) {
			if(info.questionType == QuestionAnswerType.TEXT) ChangeCurrentObject(LoadPrefab("Game/TxtAnyGame"));
			else ChangeCurrentObject(LoadPrefab("Game/GeneralGame"));

			currentGameObject.GetComponent<ClickGame>().StartGame(info, isRandom, spriteNumbers, playerNames);
		}

		public void LoadGamesMenu(bool editMode) {
			ChangeCurrentObject(LoadPrefab("GamesMenu"));
			if(editMode) currentGameObject.GetComponent<GamesMenu>().EditMode();
			else currentGameObject.GetComponent<GamesMenu>().NonEditMode();
		}

		internal void LoadChooseStyle() {
			ChangeCurrentObject(LoadPrefab("MenuChooseStylePanel"));
		}

		internal void LoadPlayersMenu(ClickGameInfo info, bool isRandom) {
			ChangeCurrentObject(LoadPrefab("PlayersMenu"));
			currentGameObject.GetComponent<PlayersMenu>().StartMenu(info, isRandom);
		}

		internal void LoadStyle(int styleNumber) {
			string creationScreen = MapNumberToScreen(styleNumber);
			GameObject generalOverview = LoadPrefab("QuestionsCreatorMenu");
			ChangeCurrentObject(generalOverview);
			currentGameObject.GetComponent<GeneralOverview>().SetCreationScreen(creationScreen);
		}

		internal void EditGame(ClickGameInfo info){
			GameObject generalOverview = LoadPrefab("QuestionsCreatorMenu");
			ChangeCurrentObject(generalOverview);
			currentGameObject.GetComponent<GeneralOverview>().EditGame(info);
		}

		private string MapNumberToScreen(int n){
			switch(n) {
			case 0: return "TxtTxtEditor";
			case 1: return "TxtImgEditor";
			case 2: return "SoundImgEditor";
			case 3: return "SoundTxtEditor";
			case 4: return "ImgTxtEditor";
			case 5: return "SoundImageTxtEditor";
			default: throw new Exception("Style number does not exist.");
			}
		}

//		public static QuestionBox QuestionBox(QuestionAnswerType type, Transform parent){
//			QuestionBox box = null;
//			switch(type){
//			case QuestionAnswerType.IMG:
//			case QuestionAnswerType.AUDIO_IMG:
//				box = InstantiatePrefab("QuestionType/imageQuestionBox").GetComponent<QuestionBox>();
//				break;
//			case QuestionAnswerType.AUDIO:
//				box = InstantiatePrefab("QuestionType/soundQuestionBox").GetComponent<QuestionBox>();
//				break;
//			default:
//				box = InstantiatePrefab("QuestionType/txtQuestionBox").GetComponent<QuestionBox>();
//				break;
//			}
//
//			box.transform.SetParent(parent);
//
//			box.transform.localPosition = Vector3.zero;
//			box.GetComponent<RectTransform>().offsetMax = Vector2.zero;
//			box.GetComponent<RectTransform>().offsetMin = Vector2.zero;
//			box.transform.localScale = Vector3.one;
//
//			return box;
//		}

		public static GameBox GameBox(Transform parent){
			GameBox box = InstantiatePrefab("GamesMenu/GameDisplayPrefab").GetComponent<GameBox>();

			box.transform.SetParent(parent);

			box.transform.localPosition = Vector3.zero;
			box.GetComponent<RectTransform>().offsetMax = Vector2.zero;
			box.GetComponent<RectTransform>().offsetMin = Vector2.zero;
			box.transform.localScale = Vector3.one;

			return box;
		}

		public static GameObject ViewGameBox(Transform parent){
			GameObject box = InstantiatePrefab("GamesMenu/GameDisplayViewerPrefab");

			box.transform.SetParent(parent);

			box.transform.localPosition = Vector3.zero;
			box.GetComponent<RectTransform>().offsetMax = Vector2.zero;
			box.GetComponent<RectTransform>().offsetMin = Vector2.zero;
			box.transform.localScale = Vector3.one;

			return box;
		}

		public static GameObject PlayersBox(Transform parent){
			GameObject box = InstantiatePrefab("PlayersMenu/players");

			box.transform.SetParent(parent);

			box.transform.localPosition = Vector3.zero;
			box.GetComponent<RectTransform>().offsetMax = Vector2.zero;
			box.GetComponent<RectTransform>().offsetMin = Vector2.zero;
			box.transform.localScale = Vector3.one;

			return box;
		}

		public static GameObject PlayerScoreBox(Transform parent){
			GameObject box = InstantiatePrefab("Game/ScorePlayerPanel");

			box.transform.SetParent(parent);

			box.transform.localPosition = Vector3.zero;
			box.GetComponent<RectTransform>().offsetMax = Vector2.zero;
			box.GetComponent<RectTransform>().offsetMin = Vector2.zero;
			box.transform.localScale = Vector3.one;

			return box;
		}

		internal void LoadMainMenu() {
			ChangeCurrentObject(LoadPrefab("MainMenu"));
			//  if (!Settings.Instance().GetMusic()) SoundController.GetController().StopMusic();
			//            else SoundController.GetController().PlayMusic();
		}

		public void ShowSettingsScreen(GameObject parent){
			GameObject settings = InstantiatePrefab("SettingsPanel");

			FitObjectTo(settings, parent);
		}

		public void InternationalizeCurrent() {
			currentGameObject.GetComponent<Internationalizable>().SetTexts();
		}

        public static ViewController GetController() { return viewController; }
    }
}
