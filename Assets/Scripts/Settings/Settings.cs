using System;
using UnityEngine;
using Assets.Scripts.App;
using I18N;

namespace Assets.Scripts.Settings {
	public class Settings : MonoBehaviour {
		private static Settings settings;

		private bool music, sfx, upperCase;

		public static string MUSIC_KEY = "MUSIC_KEY", UPPERCASE_KEY = "UPPERCASE_KEY", SFX_KEY = "SFX_KEY";

		void Awake() {
			if (settings == null) {
				settings = this;
			} else if (settings != this) {
				Destroy(gameObject);
			}
			LoadSettings();
		}

		void LoadSettings() {
			music = GetBool(MUSIC_KEY,true);
			sfx = GetBool(SFX_KEY,true);
			upperCase = GetBool(UPPERCASE_KEY,false);
		}

		bool GetBool(string key, bool defaultValue) {
			if(!PlayerPrefs.HasKey(key)){
				SetBool(key, defaultValue);
			}
			return PlayerPrefs.GetInt(key) == 1;
		}

		void SetBool(string key, bool value) {
			PlayerPrefs.SetInt(key, value ? 1 : 0);
			PlayerPrefs.Save();
		}

		public void SetMusic(bool music) {
			this.music = music;
			Debug.Log("Set music " + music);
			SetBool(MUSIC_KEY, music);
			if (!music) SoundController.GetController().StopMusic();
			else SoundController.GetController().PlayMusic();
		}

		internal bool GetMusic() {
			return music;
		}

		public void SetSFX(bool sfx) {
			Debug.Log("Set sfx " + sfx);
			this.sfx = sfx;
			SetBool(SFX_KEY, sfx);
		}

		public void SetUppercase(bool upperCase) {
			Debug.Log("Set uppercase " + upperCase);
			this.upperCase = upperCase;
			SetBool(UPPERCASE_KEY, upperCase);
		}

		public void SwitchLanguage(int languageToggle) {
			I18n.SetLocale(languageToggle);
		}

		public bool MusicOn() { return music; }
		public bool SfxOn() { return sfx; }
		public bool UppercaseOn() { return upperCase; }


		public static Settings Instance() { return settings; }
	}
}