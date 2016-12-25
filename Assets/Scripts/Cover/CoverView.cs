using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Cover
{
    public class CoverView : MonoBehaviour
    {
        public Button argentineBtn;
        public Button britishBtn;
        public Button startBtn;
        public Button aboutBtn;
        public Button oxBtn;
        public Button aboutScreen;
        public Button oxScreen;
        public List<Sprite> aboutScreenSprites;
        public List<Sprite> oxScreenSprites;
        public List<Sprite> aboutBtnSprites;
        public List<Sprite> startBtnSprites;

        public void OnClicFlagBtn(int toLanguage)
        {
            PlayClicSound();
            CoverController.instance.ChangeLanguage(toLanguage);
            ChangeLanguage(toLanguage);
        }

        public void OnClicStartBtn()
        {
            PlayClicSound();
            CoverController.Play();
        }

        public void OnClicOxBtn()
        {
            PlayClicSound();
            ShowOx();
        }

        public void OnClicAboutBtn()
        {
            PlayClicSound();
            ShowAbout();
        }

        private void ShowOx()
        {
            oxScreen.gameObject.SetActive(true);
        }

        private void ShowAbout()
        {
            aboutScreen.gameObject.SetActive(true);
        }

        private void ChangeLanguage(int language)
        {
            argentineBtn.gameObject.SetActive(language == 0 ? false : true);
            britishBtn.gameObject.SetActive(language == 1 ? false : true);
            oxScreen.image.sprite = oxScreenSprites[language];
            aboutBtn.image.sprite = aboutBtnSprites[language];
            aboutScreen.image.sprite = aboutScreenSprites[language];
            startBtn.image.sprite = startBtnSprites[language];
        }

        public void PlayClicSound() {
            CoverController.PlayClicSound();
        }
    }
}
