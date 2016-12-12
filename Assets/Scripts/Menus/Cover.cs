using System;
using UnityEngine.UI;
using UnityEngine;
using Assets.Scripts.App;
using System.Collections.Generic;
using I18N;

namespace Assets.Scripts.Menus {
	public class Cover : MonoBehaviour {
		public Image aboutPanel;
		public Image oxPanel;
		public List<Toggle> languages;

		public void Start(){
			languages[I18n.GetLanguageToggle()].isOn = true;
		}

		public void ToggleChange(){
			SoundController.GetController ().PlayToggleSound ();
			int index = languages.FindIndex((Toggle toggle) => toggle.isOn);
			I18n.SetLocale(index);
			SetTexts();
		}

		public void ClickPanel(){
			aboutPanel.gameObject.SetActive(false);
			oxPanel.gameObject.SetActive(false);
		}

		public void ShowPanel(Image panel){
			SoundController.GetController ().PlayClickSound ();
			panel.gameObject.SetActive(true);
		}

		public void Create(){
			SoundController.GetController ().PlayClickSound ();
			ViewController.GetController().LoadGamesMenu(true);
		}

		public void Load(){
			SoundController.GetController ().PlayClickSound ();
			ViewController.GetController().LoadGamesMenu(false);
		}

		// I18N ***************************************************************************************

		public Text create, load, about;
		public Text aboutName, aboutGameName, aboutArea, aboutAreaName, credits;
		public Text oxPanelInfo, oxPanelMoreInfo, oxPanelContact;

		void SetTexts() {
			create.text = I18n.Msg("cover.create");
			load.text = I18n.Msg("cover.load");
			about.text = I18n.Msg("cover.about");

			aboutName.text = I18n.Msg("cover.about.name");
			aboutGameName.text = I18n.Msg("cover.about.gameName");
			aboutArea.text = I18n.Msg("cover.about.area");
			aboutAreaName.text = I18n.Msg("cover.about.areaName");
			credits.text = I18n.Msg("cover.about.credits");

			oxPanelInfo.text = I18n.Msg("cover.oxPanel.info");
			oxPanelMoreInfo.text = I18n.Msg("cover.oxPanel.moreInfo");
			oxPanelContact.text = I18n.Msg("cover.oxPanel.contact");
		}
	}
}