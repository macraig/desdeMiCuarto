using UnityEngine;
using System.Collections;
using Assets.Scripts.App;
using System;

namespace Assets.Scripts.Cover
{

    public class CoverController : MonoBehaviour
    {

        public static CoverController instance;
        public CoverView coverView;
	

        // Use this for initialization
        void Start()
        {
            //Check if instance already exists
            if (instance == null)
                //if not, set instance to this
                instance = this;
            //If instance already exists and it's not this:
            else if (instance != this)
                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ChangeLanguage(int language)
        {
            SettingsController.instance.SwitchLanguage(language);
        }

        internal static void Play()
        {

			ViewController.instance.LoadNameScreen();

        }

        internal static void PlayClicSound()
        {
			SoundController.instance.PlayClickSound();
        }
    }
}
