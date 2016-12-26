﻿using Assets.Scripts.Settings;
using System;
using System.Collections.Generic;
using Assets.Scripts.Sound;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.Metrics.View
{

    public class MetricsRow : MonoBehaviour
    {
        [SerializeField]
        private Image icon;
        [SerializeField]
        private Text activityName;
        [SerializeField]
        private Text score;
        [SerializeField]
        private Text levelText;
        [SerializeField]
        private List<Image> stars;
        [SerializeField]
        private List<Image> levels;
        [SerializeField]
        private Button viewDetailsButton;
        private int area;
        private int idGame;
        private int level;


        void Start()
        {
            UpdateTexts();
        }

        private void UpdateTexts()
        {
            switch (SettingsController.GetController().GetLanguage())
            {
                case 0:
                    levelText.text = "nivel";
                    viewDetailsButton.GetComponentInChildren<Text>().text = "VER DETALLES";
                    break;
                default:
                    levelText.text = "level";
                    viewDetailsButton.GetComponentInChildren<Text>().text = "VIEW DETAILS";
                    break;
            }
        }

        public void SetActivity(string activityName)
        {
            this.activityName.text = activityName;
        }

        internal void DisableViewDetails()
        {
            viewDetailsButton.interactable = false;
        }

        public void SetScore(int currentScore)
        {
            this.score.text = (currentScore == 0 ? "-" : "" + currentScore);
        }

        public void SetStars(int currentStars)
        {
            for (int i = 0; i < stars.Count; i++)
            {
                stars[i].gameObject.SetActive(i < currentStars);
            }
        }

        public void OnClickViewDetails()
        {
            SoundController.GetController().PlayClickSound();
            MetricsView.GetMetricsView().ShowDetailsOf(area, idGame, level);
        }

        internal void SetArea(int area)
        {
            this.area = area;
        }

        internal void SetIndex(int index)
        {
            this.idGame = index;
        }

        internal void SetLevel(int level)
        {
            this.level = level;
            for (int i = 0; i < levels.Count; i++)
            {
                levels[i].gameObject.SetActive(i <= level);
            }
        }

        internal int GetArea()
        {
            return area;
        }

        internal void SetIcon(Sprite sprite)
        {
            this.icon.sprite = sprite;
        }
    }
}