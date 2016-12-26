using UnityEngine;

namespace Assets.Scripts.Login
{

    public class IncorrectUserAnimation : MonoBehaviour {

        public LoginView loginView;

        public void OnIncorrectUserAnimationEnd()
        {
            gameObject.GetComponent<Animation>().Stop();
            gameObject.SetActive(false);
            loginView.OnIncorrectInputAnimationEnd();
        }

        internal void ShowIncorrecrUserAnimation()
        {
            gameObject.SetActive(true);
            gameObject.GetComponent<Animation>().Play();

        }

    }

}
