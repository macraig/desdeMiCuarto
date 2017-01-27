using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Games.SchoolActivity {

public class SantiAnimationScript : MonoBehaviour {

		public SchoolActivityView levelView;

		public void OnSantiAnimationEnd()
		{
			levelView.NextMove();
		}

}
}
