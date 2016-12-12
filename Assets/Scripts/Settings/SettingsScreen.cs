using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using I18N;
using Assets.Scripts.App;

namespace Assets.Scripts.Settings {
	public class SettingsScreen : MonoBehaviour, Internationalizable {
		public Toggle music, sfx, uppercase;

		private bool start;

		public List<Toggle> languages;

		public void Start(){
			start = true;
			languages[I18n.GetLanguageToggle()].isOn = true;
			music.isOn = Settings.Instance().MusicOn();
			sfx.isOn = Settings.Instance().SfxOn();
			uppercase.isOn = Settings.Instance().UppercaseOn();
			start = false;
			Debug.Log("music:" + music.isOn + ",sfx:" + sfx.isOn + "uppercase:" + uppercase.isOn);
		}

		public void ToggleChange(){
			SoundController.GetController ().PlayToggleSound ();
			int index = languages.FindIndex((Toggle toggle) => toggle.isOn);
			I18n.SetLocale(index);
			SetTexts();
			ViewController.GetController().InternationalizeCurrent();
		}

		public void ToggleMusic(){
			SoundController.GetController ().PlayClickSound ();
			if(!start) Settings.Instance().SetMusic(!music.isOn);
		}

		public void ToggleSfx(){
			SoundController.GetController ().PlayClickSound ();
			if(!start) Settings.Instance().SetSFX(!sfx.isOn);
		}

		public void ToggleUppercase(){
			SoundController.GetController ().PlayToggleSound ();
			if(!start) Settings.Instance().SetUppercase(uppercase.isOn);
			uppercaseToggleText.text = I18n.Msg(uppercase.isOn ? "common.toggleOn" : "common.toggleOff");
		}

		public void CloseSettings(){
			SoundController.GetController ().PlayClickSound ();
			Destroy(gameObject);
		}

		// I18N ********************************************************************************************

		public Text uppercaseText, musicText, sfxText, uppercaseToggleText;
		
		public void SetTexts() {
			uppercaseText.text = I18n.Msg("settings.uppercase");
			musicText.text = I18n.Msg("settings.music");
			sfxText.text = I18n.Msg("settings.sfx");
			uppercaseToggleText.text = I18n.Msg(uppercase.isOn ? "common.toggleOn" : "common.toggleOff");
		}
	}
}