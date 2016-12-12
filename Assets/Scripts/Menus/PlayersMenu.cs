using System;
using UnityEngine;
using Assets.Scripts.App;
using UnityEngine.UI;
using System.Collections.Generic;
using I18N;

namespace Assets.Scripts.Menus {
	public class PlayersMenu : MonoBehaviour {
		public GameObject playersPanel, errorPanel;
		public Button playBtn, addBtn;

		private List<GameObject> playerPrefabs;

		private List<Sprite> characterSprites;
		private ClickGameInfo info;
		private bool isRandom;
		private int questionQuantity;

		public void StartMenu(ClickGameInfo info, bool isRandom){
			SetTexts();
			this.info = info;
			this.isRandom = isRandom;
			characterSprites = Randomizer.RandomizeList(new List<Sprite>(Resources.LoadAll<Sprite>("Sprites/characters")));

			questionQuantity = ((List<Question>) Common.Load(info.GetPath() + "/" + Common.QUESTIONS_FILE)).Count;

			StartPlayers();
		}

		private void StartPlayers() {
			playerPrefabs = new List<GameObject>();
			PlusClick();
		}

		public void PlusClick(){
			if(HasQuestionError()){
				errorPanel.SetActive(true);
				errorPanelText.text = I18n.Msg("playersMenu.errorPanelText", (playerPrefabs.Count + 1).ToString(), QuestionsFromPlayerQuantity().ToString());
				return;
			}

			GameObject box = ViewController.PlayersBox(playersPanel.transform);

			box.transform.GetChild(0).GetComponent<Image>().sprite = characterSprites[playerPrefabs.Count];
			box.transform.GetChild(2).GetComponent<InputField>().onValueChanged.AddListener(CheckButtonsAndInputs);
			box.transform.GetChild(2).GetComponent<InputField>().placeholder.GetComponentInChildren<Text>().text = I18n.Msg("playersMenu.playerPlaceholder");
			box.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => EraseClick(box));

			playerPrefabs.Add(box);
			CheckButtonsAndInputs();
		}

		public void EraseClick(GameObject box){
			int index = playerPrefabs.IndexOf(box);
			Destroy(box);
			playerPrefabs.RemoveAt(index);
			Sprite current = characterSprites[index];
			characterSprites.RemoveAt(index);
			characterSprites.Add(current);

			CheckButtonsAndInputs();
		}

		private int QuestionsFromPlayerQuantity() {
			switch(playerPrefabs.Count) {
			case 0:
				return 100;
			case 1:
				return 6;
			case 2:
				return 9;
			case 3:
				return 12;
			default:
				throw new IndexOutOfRangeException("Unreachable");
			}
		}

		bool HasQuestionError() {
			switch(playerPrefabs.Count) {
			case 0:
				return false;
			case 1:
				return questionQuantity < 6;
			case 2:
				return questionQuantity < 9;
			case 3:
				return questionQuantity < 12;
			default:
				throw new IndexOutOfRangeException("Unreachable");
			}
		}

		public void ErrorPanelClick(){
			errorPanel.SetActive(false);
		}

		public void CheckButtonsAndInputs(string a = "") {
			addBtn.interactable = playerPrefabs.Count != 4;

			playBtn.interactable = CheckPlayButton();

			playerPrefabs.ForEach((GameObject box) => {
				InputField f = box.transform.GetChild(2).GetComponent<InputField>();
				if(f.text.Length > 10) f.text = f.text.Substring(0, 10);
			});
		}

		private bool CheckPlayButton(){
			if(playerPrefabs.Count == 0) return false;

			foreach(GameObject playerPrefab in playerPrefabs) {
				if(playerPrefab.transform.GetChild(2).GetComponent<InputField>().text.Length == 0)
					return false;
			}
			return true;
		}

		public void PlayClick(){
			List<int> spriteNumbers = PlayerSpriteNumbers();
			List<string> playerNames = PlayerNames();

			ViewController.GetController().LoadGameScreen(spriteNumbers, playerNames, info, isRandom);
		}

		List<string> PlayerNames() {
			return playerPrefabs.ConvertAll((GameObject b) => b.transform.GetChild(2).GetComponent<InputField>().text);
		}

		List<int> PlayerSpriteNumbers() {
			List<int> numbers = new List<int>();
			Sprite[] characters = Resources.LoadAll<Sprite>("Sprites/characters");

			for(int i = 0; i < playerPrefabs.Count; i++) {
				numbers.Add(Array.IndexOf(characters, characterSprites[i]));
			}
			return numbers;
		}

		public void BackClick(){
			ViewController.GetController().LoadGamesMenu(false);
		}

		// I18N **********************************************************************************************

		public Text menuTitle, playBtnText, addPlayerBtnText, errorPanelText;

		public void SetTexts(){
			menuTitle.text = I18n.Msg("playersMenu.title");
			playBtnText.text = I18n.Msg("playersMenu.playButton");
			addPlayerBtnText.text = I18n.Msg("playersMenu.addPlayerButton");

		}
	}
}