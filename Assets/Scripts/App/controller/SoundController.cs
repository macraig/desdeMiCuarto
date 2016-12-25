using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.App{
    public class SoundController : MonoBehaviour{
		public static SoundController instance;


	    public AudioClip failureSound;
	    public AudioClip rightAnswerSound;
	    public AudioClip levelCompleteSound;
	    public AudioClip music;
	    public AudioClip clickSound;


	    public AudioSource mySource;
	    public AudioSource musicSource;
	    public List<List<AudioClip>> instructionSounds;

		private bool concatenatingAudios = false;


    //Awake is always called before any Start functions
        void Awake()
    {
	    if (instance == null)           
		    instance = this;
	    else if (instance != this)
		    Destroy(gameObject);


	    DontDestroyOnLoad(transform.root.gameObject);
    }


        public void PlayClip(AudioClip myClip)
    {
	    mySource.clip = myClip;
		mySource.Play();
    }

        public void PlayFailureSound()
    {
	    if (SettingsController.instance.SfxOn()) {
		    mySource.clip = failureSound;
		    mySource.Play();
	    }
	
    }

        public void PlayClickSound()
    {
	    if (SettingsController.instance.SfxOn()) {
		    mySource.clip = clickSound;
		    mySource.Play();
	    }

    }

        public void PlayRightAnswerSound()
    {
	    if (SettingsController.instance.SfxOn()) {
		    mySource.clip = rightAnswerSound;
		    mySource.Play();
	    }
    }

    public void PlayLevelCompleteSound()
    {
	    if (SettingsController.instance.SfxOn()) {
		    mySource.clip = levelCompleteSound;
		    mySource.Play();
	    }
    }

    public void PlayInstruction(int level, int language)
    {
	    if (SettingsController.instance.SfxOn()) {
		    mySource.clip = instructionSounds[language][level];
		    mySource.Play();         	
	    }
    }

    public void PlayMusic()
   	{
			if (SettingsController.instance.MusicOn()&&!musicSource.isPlaying) {
				musicSource.clip = music;
		    	musicSource.Play();         	
	    }
    }

        public void StopMusic()
    {
	    musicSource.Stop();
    }

        public void StopSound()
            {
	            mySource.Stop();
				concatenatingAudios = false;
            }

		//Trying to concatenate audios!

		private List<AudioClip> playingAudios;
		private AudioClip currentAudio;
		private int currentAudioIndex;
		private Action action;

		public void ConcatenateAudio(AudioClip audio, Action f){
			List<AudioClip> l = new List<AudioClip> ();
			l.Add (audio);
			ConcatenateAudios (l, f);
		}

		public void ConcatenateAudios(List<AudioClip> audios, Action f){
			concatenatingAudios = true;
			action = f;
			currentAudio = null;
			playingAudios = audios;
			Invoke ("PlayCurrentAudios", 0.1f);
		}

		public void PlayCurrentAudios(){
			if (concatenatingAudios) {
				if (currentAudio == null) {
					currentAudio = playingAudios [0];
					currentAudioIndex = 0;
				}
				else if (currentAudioIndex == playingAudios.Count - 1) {
					currentAudio = null;
					concatenatingAudios = false;
					action.Invoke ();
					return;
				} else {
					currentAudioIndex++;
					currentAudio = playingAudios [currentAudioIndex];
				}
				SoundController.instance.PlayClip(currentAudio);
				Invoke ("PlayCurrentAudios", currentAudio.length);
			}



		}

    }

}