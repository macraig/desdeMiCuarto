using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
namespace Assets.Scripts.Games.SchoolActivity {

public class SchoolTile : MonoBehaviour {

	public GameObject santi;
	private Animator santiAnimator;

		void Start(){
			santiAnimator = santi.GetComponent<Animator> ();
		}

		public void ShowSanti(bool show){
			santi.SetActive (true);
		}

		public void MoveSantiLeft(){
			santiAnimator.SetTrigger ("santiLeft");
		}

		public void MoveSantiRight(){
			santiAnimator.SetTrigger ("santiRight");
		}

		public void MoveSantiUp(){
			santiAnimator.SetTrigger ("santiUp");
		}

		public void MoveSantiDown(){
			santiAnimator.SetTrigger ("santiDown");
		}
	
	
}
}
