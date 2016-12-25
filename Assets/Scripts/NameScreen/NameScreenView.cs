using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.App;

public class NameScreenView : MonoBehaviour {

	//todo -> get var from Settings
	public InputField inputText;
	public Text insertNameText;

	public Animator warningAnimation;


	// Use this for initialization
	void Start () {
		inputText.characterLimit = 20;
		//Fixed variable for testing purposes only
//		int language = 1;
		int language = SettingsController.instance.GetLanguage ();
		inputText.placeholder.GetComponent<Text>().text = (language==1) ?  "Insert your name" : "Ingresa tu nombre";
		insertNameText.text = (language==1) ? "WHAT'S YOUR NAME?" :  "¿CÓMO TE LLAMAS?"  ;
	}

	void Update(){
		if (Input.GetKey (KeyCode.Return)) {
			CheckEnteredUsername ();
		}
	}

	public void OnClickOkBtn(){
        PlayClicSound();
		CheckEnteredUsername ();
	}

	public void DisableAnimation(){
		warningAnimation.SetBool ("showWarning", false);
	}

	void CheckEnteredUsername(){
		SoundController.instance.PlayClickSound ();
        inputText.text = inputText.text.Trim();
		if (inputText.text != "") {
			Debug.Log ("cool name " + inputText.text.ToLower());
			NameScreenController.instance.SaveUsername (inputText.text.ToLower());
		} else {
			//Fixed variable for testing purposes only
//			warningAnimation.SetInteger ("language",1);
			warningAnimation.SetInteger ("language", SettingsController.instance.GetLanguage());
			warningAnimation.SetBool ("showWarning", true);
			Invoke("DisableAnimation",1f); 

		}
	}
	
	public void OnClickBack()
    {
        PlayClicSound();
        NameScreenController.instance.GoBack();
    }

    public void PlayClicSound()
    {
        NameScreenController.PlayClicSound();
    }
}
