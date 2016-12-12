using UnityEngine;
using Assets.Scripts.Settings;

namespace Assets.Scripts.App{

    public class SoundController : MonoBehaviour{
	    private static SoundController soundController;

	    public AudioClip wrongAnswerSound;
	    public AudioClip rightAnswerSound;
	    public AudioClip levelCompleteSound;
		public AudioClip clickSound,toggleSound,blopSound,typeSound;
        public AudioClip music;

        public AudioSource soundSource;
	    public AudioSource musicSource;

        void Awake(){
            if (soundController == null){
                soundController = this;
            }
            else if (soundController != this){
                Destroy(gameObject);
            }
            DontDestroyOnLoad(this);
        }       
    
        public void PlayClip(AudioClip customClip){
	        soundSource.clip = customClip;
	        soundSource.Play();
        }

        public void PlayFailureSound(){
			if (Settings.Settings.Instance().SfxOn()) {
				soundSource.clip = wrongAnswerSound;
		        soundSource.Play();
	        }
        }

        public void PlayClickSound(){
			if (Settings.Settings.Instance().SfxOn()) {
		        soundSource.clip = clickSound;
		        soundSource.Play();
	        }
        }

		public void PlayToggleSound(){
			if (Settings.Settings.Instance().SfxOn()) {
				soundSource.clip = toggleSound;
				soundSource.Play();
			}
		}

		public void PlayTypeSound(){
			if (Settings.Settings.Instance().SfxOn()) {
				soundSource.clip = typeSound;
				soundSource.Play();
			}
		}

		public void PlayBlopSound(){
			if (Settings.Settings.Instance().SfxOn()) {
				soundSource.clip = blopSound;
				soundSource.Play();
			}
		}

        public void PlayRightAnswerSound(){ 
			if (Settings.Settings.Instance().SfxOn()) {
		        soundSource.clip = rightAnswerSound;
		        soundSource.Play();
	        }
        }

        public void PlayLevelCompleteSound(){
			if (Settings.Settings.Instance().SfxOn()) {
		        soundSource.clip = levelCompleteSound;
		        soundSource.Play();
	        }
        }    

        public void PlayMusic(){
			if (Settings.Settings.Instance().MusicOn() && !musicSource.isPlaying) {
				musicSource.clip = music;
		    	musicSource.Play();         	
	        }
        }

        public void StopMusic(){
	        musicSource.Stop();
        }

        public void StopSound(){
            soundSource.Stop();
        }

        public static SoundController GetController(){
            return soundController;
        }
    }
}