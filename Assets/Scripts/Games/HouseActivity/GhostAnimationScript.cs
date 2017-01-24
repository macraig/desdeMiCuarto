using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Games.HouseActivity {

public class GhostAnimationScript : MonoBehaviour {

	public HouseActivityView view;

	public void OnRightAnswerAnimationEnd()
	{
		gameObject.SetActive(false);
		view.OnRightGhostAnimationEnd();
	}

	public void OnWrongAnswerAnimationEnd()
	{
		gameObject.SetActive(false);
		view.OnWrongGhostAnimationEnd();
	}

	public void ShowAnimation()
	{
		gameObject.SetActive(true);

	}
}
}