using System;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.App;
using I18N;

namespace Assets.Scripts.Menus {
	public class ChooseStyle : MonoBehaviour {
		public GameObject previewsPanel;
		public List<Image> previews;
		public Button okBtn;
		public List<Toggle> toggles;
		public Text upperTxt;

		private int SelectedToggle(){
			return toggles.FindIndex((Toggle obj) => obj.isOn);
		}

		public void ReturnClick(){
			SoundController.GetController ().PlayToggleSound ();
			ViewController.GetController().LoadCover();
		}

		public void OkClick(){
			SoundController.GetController ().PlayClickSound ();
			ViewController.GetController().LoadStyle(SelectedToggle());
		}

		public void Start(){
			PreviewPanelClick();
			okBtn.interactable = false;
			SetTexts();
		}

		public void PreviewPanelClick(){
			HideAllPreviews();
			previewsPanel.SetActive(false);
		}

		public void PreviewClick(int index){
			SoundController.GetController ().PlayClickSound ();
			previewsPanel.SetActive(true);
			previews[index].gameObject.SetActive(true);
		}

		private void HideAllPreviews() {
			foreach(Image preview in previews) {
				preview.gameObject.SetActive(false);
			}
		}

		public void ToggleChange(Toggle t){
			SoundController.GetController ().PlayBlopSound ();
			okBtn.interactable = t.isOn;
		}

		// I18N **********************************************************************************************

		public Text title;
		public List<Text> questionTexts;
		public List<Text> txtTxt, txtImg, soundImg, soundTxt, imgTxt;

		public void SetTexts(){
			title.text = I18n.Msg("chooseStyle.title");

			foreach(Text q in questionTexts) {
				q.text = I18n.Msg("chooseStyle.question");
			}

			List<Text>[] aux = { txtTxt, txtImg, soundImg, soundTxt, imgTxt };

			foreach(List<Text> ans in aux) {
				for(int i = 0; i < ans.Count; i++) {
					ans[i].text = I18n.Msg("chooseStyle.answer", (i + 1).ToString());
				}
			}
		}
	}
}