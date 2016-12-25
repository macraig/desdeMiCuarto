using UnityEngine;
using System.Collections;
using System;

namespace Assets.Scripts.App
{

    public class SettingsController : MonoBehaviour
    {

        public static SettingsController instance;
        private SettingsModel settingsModel;

        // Use this for initialization
        void Awake()
        {
            //Check if instance already exists
            if (instance == null)
                //if not, set instance to this
                instance = this;
            //If instance already exists and it's not this:
            else if (instance != this)
                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);
            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(transform.root.gameObject);
			SystemLanguage systemLanguage = Application.systemLanguage;
			settingsModel = new SettingsModel();
        }

		public int GetLanguage(){ return settingsModel.language;}
		public int GetMode(){ return settingsModel.currentMode;}
		public string GetUsername(){ return settingsModel.userName;}
		public bool MusicOn(){ return settingsModel.music;}
		public bool SfxOn(){ return settingsModel.soundEffects;}


        public void SwitchName(string newName)
        {
            settingsModel.userName = newName;
			MetricsController.instance.LoadFromDisk();
        }

        public void SwitchMode(int newMode)
        {
            settingsModel.currentMode = newMode;
        }

        public void ToggleMusic()
        {
			settingsModel.music = (settingsModel.music) ? false : true;

			if (!settingsModel.music)
				SoundController.instance.StopMusic ();
			else
				SoundController.instance.PlayMusic ();
        }

        public void ToggleSFX()
        {
			settingsModel.soundEffects = (settingsModel.soundEffects) ? false : true;
        }

        
        public void SwitchLanguage(int language)
        {
            settingsModel.language = language;
            
        }

        internal void PlayClicSound()
        {
			SoundController.instance.PlayClickSound();

        }
    }
}
