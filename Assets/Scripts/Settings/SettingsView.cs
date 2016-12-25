using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Scripts.App;

namespace Assets.Scripts.Settings
{
    public class SettingsView : MonoBehaviour
    {
    
        public GameObject settingsOptions;
        // text labels -> [mode, language, music, sound]
        public List<Text> labels;
        public Text title;
        // buttons with text -> [mode (free or challenge), language (español or english), switchPlayer]
        public List<Button> labelButtons;
        // buttons with Sprites (images) [music, sound]
        public List<Button> spriteButtons;

        public List<Sprite> musicBtnSprites;
        public List<Sprite> soundBtnSprites;

        public Image popUp;
        public InputField newName;
        public Button ticBtn;
        public Button popUpCross;
        public Text popUpTitle;

        public Animator warningAnimation;


        // Use this for initialization
        void Start()
        {
            SettingsController settings = SettingsController.instance;

            RefreshScreen (settings.GetLanguage());
			RefreshModeBtn ();
			RefreshMusicBtn ();
			RefreshSFXBtn ();

            Debug.Log("Language: " + settings.GetLanguage());
            Debug.Log("Music: " + settings.MusicOn());
            Debug.Log("Sound: " + settings.SfxOn());
            Debug.Log("Mode: " + settings.GetMode());
            Debug.Log("Player name: " + settings.GetUsername());

        }

        // Update is called once per frame
        void Update()
        {
            if (popUp.IsActive() && Input.GetKey(KeyCode.Return))
            {
                CheckEnteredUsername();
            }
        }

        public void OnClicSwitchName()
        {
            PlayClicSound();
            popUp.gameObject.SetActive(true);
            settingsOptions.gameObject.SetActive(false);
            newName.gameObject.SetActive(true);
            newName.text = SettingsController.instance.GetUsername();

        }

		public void OnClickExitButton()
		{
			PlayClicSound();
			Application.Quit();
		}

        public void OnClicMusicBtn()
        {
            PlayClicSound();
            SettingsController.instance.ToggleMusic();
			RefreshMusicBtn ();
           
        }

        public void OnClicSound()
        {
            PlayClicSound();
            SettingsController.instance.ToggleSFX();
			RefreshSFXBtn ();
        }

        public void OnClicSwitchMode()
        {
            PlayClicSound();
            int mode = SettingsController.instance.GetMode() == 1 ? 0 : 1;
            SettingsController.instance.SwitchMode(mode);
			RefreshModeBtn ();
        }

        public void OnClicLanguageBtn()
        {
            PlayClicSound();
            int language = SettingsController.instance.GetLanguage() == 1 ? 0 : 1;
            SettingsController.instance.SwitchLanguage(language);
			RefreshScreen (language);
        }

        public void OnClicCrossBtn()
        {
            PlayClicSound();
			ViewController.instance.LoadMainMenu();
        }

        public void OnClicPopUpCrossBtn()
        {
            PlayClicSound();
            settingsOptions.gameObject.SetActive(true);
            popUp.gameObject.SetActive(false);
        }

        public void OnClicTicBtn()
        {
            PlayClicSound();
            CheckEnteredUsername();
        }


        private void CheckEnteredUsername()
        {
            newName.text = newName.text.Trim();
            if (newName.text != "")
            {
                Debug.Log("cool name " + newName.text.ToLower());
                SettingsController.instance.SwitchName(newName.text.ToLower());
                popUp.gameObject.SetActive(false);
                settingsOptions.gameObject.SetActive(true);
            }
            else {
                //Fixed variable for testing purposes only
                //			warningAnimation.SetInteger ("language",1);
                warningAnimation.SetInteger("language", SettingsController.instance.GetLanguage());
                warningAnimation.SetBool("showWarning", true);
                Invoke("DisableAnimation", 1f);

            }
        }

        public void DisableAnimation()
        {
            warningAnimation.SetBool("showWarning", false);
        }

        private void PlayClicSound()
        {
            SettingsController.instance.PlayClicSound();
        }

		private void RefreshMusicBtn(){
			spriteButtons[0].image.sprite = musicBtnSprites[SettingsController.instance.MusicOn() ? 0 : 1];
		}

		private void RefreshSFXBtn(){
			spriteButtons[1].image.sprite = soundBtnSprites[SettingsController.instance.SfxOn() ? 0 : 1];	
		}

		private void RefreshModeBtn(){
			switch (SettingsController.instance.GetLanguage())
			{
			case 0:
				labelButtons[0].GetComponentInChildren<Text>().text = SettingsController.instance.GetMode() == 0 ? "Libre" : "Desafío";
				break;
			case 1:
				labelButtons[0].GetComponentInChildren<Text>().text = SettingsController.instance.GetMode() == 0 ? "Free" : "Challenge";
				break;
			}
		}

		private void RefreshScreen(int language){
			switch (language)
			{
			case 0: // spanish
				title.text = "Configuración";
				labels[0].text = "Modo";
				labels[1].text = "Switch to";
				labels[2].text = "Música";
				labels[3].text = "Sonido";

				labelButtons[0].GetComponentInChildren<Text>().text = SettingsController.instance.GetMode() == 0 ? "Libre" : "Desafío";
				labelButtons[1].GetComponentInChildren<Text>().text = "English";
				labelButtons[2].GetComponentInChildren<Text>().text = "Cambiar jugador";
				popUpTitle.text = "Cambiar jugador";
				newName.placeholder.GetComponent<Text>().text = "Ingresa tu nombre";

				break;
			case 1: // english
				title.text = "Settings";
				labels[0].text = "Mode";
				labels[1].text = "Cambiar a";
				labels[2].text = "Music";
				labels[3].text = "Sound";

				labelButtons[0].GetComponentInChildren<Text>().text = SettingsController.instance.GetMode() == 0 ? "Free" : "Challenge";
				labelButtons[1].GetComponentInChildren<Text>().text = "Español";
				labelButtons[2].GetComponentInChildren<Text>().text = "Switch player";
				popUpTitle.text = "Switch player";
				newName.placeholder.GetComponent<Text>().text = "Insert your name";
				break;

			}

		}
    }
}