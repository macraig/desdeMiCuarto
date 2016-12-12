using System;
using Assets.Scripts.App;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using I18N;
using Assets.Scripts.Settings;
using System.IO;
//using UnityEditor.VersionControl;

namespace Assets.Scripts.Menus {
	public class GamesMenu : MonoBehaviour, Internationalizable {
		public ScrollRect gamesScroll;
		public Image noGamesSign;
		public ToggleGroup toggleGroup;
		public Button addBtn, playBtn, printBtn;
		public GameObject deletePanel;

		private GameBox auxBox;
		private ClickGameInfo auxInfo;
		private bool isEdit;

		private List<GameObject> boxes;

		private void SetEditGames() {
			isEdit = true;
			ShowPrintAndPlay();
			List<ClickGameInfo> games = GetGames();
			boxes = new List<GameObject>();
			noGamesSign.gameObject.SetActive(games.Count == 0);

			games.ForEach((ClickGameInfo info) =>{
				GameBox box = ViewController.GameBox(gamesScroll.transform.GetChild(0).GetChild(0));
				box.SetGame(this, info);
				boxes.Add(box.gameObject);

				box.GetComponent<Toggle>().group = toggleGroup;
				box.GetComponent<Toggle>().onValueChanged.AddListener(ActivatePrintAndPlay);
			});
		}

		public void PlayClick(){
			SoundController.GetController ().PlayClickSound ();
			List<ClickGameInfo> games = GetGames();
			int indexOn = GetGameIndexOn();

			GameClick(boxes[indexOn], games[indexOn]);
		}

		private int GetGameIndexOn() {
			return boxes.ConvertAll((GameObject input) => input.GetComponent<Toggle>()).FindIndex((Toggle obj) => obj.isOn);
		}

		public void BackClick(){
			SoundController.GetController ().PlayClickSound ();
			ViewController.GetController().LoadCover();
		}

		public void EditGame(ClickGameInfo info){
			ViewController.GetController().EditGame(info);
		}

		private List<ClickGameInfo> GetGames(){
			var games = GameController.GetController().GetGames();
			//games.Reverse();
			return games;
		}

		public void DeleteGame(GameBox box, ClickGameInfo info){
			deletePanel.SetActive(true);
			auxBox = box;
			auxInfo = info;
		}

		public void DeleteYesClick(){
			SoundController.GetController ().PlayClickSound ();
			GameController.GetController().DeleteGame(auxInfo);
			Destroy(auxBox.gameObject);
			noGamesSign.gameObject.SetActive(GetGames().Count == 0);
			deletePanel.SetActive(false);
		}

		public void DeleteNoClick(){
			SoundController.GetController ().PlayClickSound ();
			deletePanel.SetActive(false);
		}

		public void PlusClick(){
			SoundController.GetController ().PlayClickSound ();
			ViewController.GetController().LoadChooseStyle();
		}

		public void EditMode() {
			SetEditGames();
		}

		public void NonEditMode() {
			SetNonEditGames();
			addBtn.gameObject.SetActive(false);
		}

		void SetNonEditGames() {
			ShowPrintAndPlay();

			List<ClickGameInfo> games = GetGames();
			isEdit = false;
			boxes = new List<GameObject>();
			noGamesSign.gameObject.SetActive(games.Count == 0);

			games.ForEach((ClickGameInfo info) => {
				GameObject box = ViewController.ViewGameBox(gamesScroll.transform.GetChild(0).GetChild(0));
				box.transform.GetChild(1).GetComponent<Text>().text = info.title;
				box.transform.GetChild(2).GetComponent<Text>().text = info.author;

				box.transform.GetChild(3).GetComponent<Toggle>().onValueChanged.AddListener(SetBoxTexts);
				//box.GetComponent<Button>().onClick.AddListener(() => GameClick(box, info));

				boxes.Add(box);

				box.GetComponent<Toggle>().group = toggleGroup;
				box.GetComponent<Toggle>().onValueChanged.AddListener(ActivatePrintAndPlay);
			});
			SetBoxTexts();
		}

		private void ActivatePrintAndPlay(bool change = true){
			printBtn.interactable = true;
			if(!isEdit) {
				playBtn.interactable = true;
			}
		}

		private void ShowPrintAndPlay(bool change = true){
			printBtn.gameObject.SetActive(true);
			printBtn.interactable = false;
			if(!isEdit) {
				playBtn.gameObject.SetActive(true);
				playBtn.interactable = false;
			}
		}

		public void PrintClick(){
			SoundController.GetController ().PlayClickSound ();
			ClickGameInfo info = GetGames()[GetGameIndexOn()];
			string s = Common.DocumentsPath() + "/OXMINIGAMES/" + Common.GAME_NAME + "/";
			Directory.CreateDirectory(s);
			Printer.SavePrintable(info, s);
			Debug.Log("La versión imprimible se ha guardado con éxito en " + s );

		}

		void SetBoxTexts(bool b = false){
			boxes.ForEach(box => {
				Toggle toggle = box.transform.GetChild(3).GetComponent<Toggle>();
				toggle.GetComponentInChildren<Text>().text = I18n.Msg(toggle.isOn ? "gamesMenu.toggleOn" : "gamesMenu.toggleOff");
				box.transform.GetChild(4).GetComponent<Text>().text = I18n.Msg("gamesMenu.random");
			});
		}

		void GameClick(GameObject box, ClickGameInfo info){
			bool isRandom = box.transform.GetChild(3).GetComponent<Toggle>().isOn;

			ViewController.GetController().LoadPlayersMenu(info, isRandom);
		}

		public void SettingsClick(){
			SoundController.GetController ().PlayClickSound ();
			ViewController.GetController().ShowSettingsScreen(gameObject);
		}

		// I18N **********************************************************************************************

		public Text noGamesSignText, gamesMenuTitle, addGameButtonText, playButtonText;
		public Text deleteText, deleteYes, deleteNo;

		public void Start(){
			SetTexts();
		}

		public void SetTexts(){
			noGamesSignText.text = I18n.Msg("gamesMenu.noGamesSign");
			gamesMenuTitle.text = I18n.Msg("gamesMenu.title");
			addGameButtonText.text = I18n.Msg("gamesMenu.addButton");
			playButtonText.text = I18n.Msg("gamesMenu.playButton");
			deleteText.text = I18n.Msg("gamesMenu.delete");

			deleteYes.text = I18n.Msg("common.yes");
			deleteNo.text = I18n.Msg("common.no");

			if(!isEdit) SetBoxTexts();
		}
	}
}