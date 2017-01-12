﻿using System;
using System.Collections.Generic;
using Assets.Scripts.App;
using UnityEngine;

namespace Assets.Scripts.Metrics.Model{
    [Serializable]
	public class MetricsModel 
	{
        private const int MAX_SCORE = 10000;
        private const int MIN_SCORE = 500;
		private int TOTAL_GAMES;

		public List<List<GameMetrics>> metrics;
		private List<GameMetrics> currentGameMetrics;

		public MetricsModel(List<Game> games){
			TOTAL_GAMES = games.Count;
			metrics = new  List<List<GameMetrics>> ();
			/*TESTING ONLY*/
			foreach (Game game in games) {
				GameMetrics gameMetrics = new GameMetrics (game.GetId ());
				List<GameMetrics> metricsList = new List<GameMetrics> ();
				metricsList.Add (gameMetrics);
				metrics.Add (metricsList);
			}
		}
			

        internal void DiscardCurrentMetrics()
		{
			currentGameMetrics.RemoveAt(currentGameMetrics.Count - 1);
        }

        internal void GameFinished(int lapsedSeconds, int minSeconds, int pointsPerSecond, int pointsPerError){
            CalculateFinalScore(lapsedSeconds, minSeconds, pointsPerSecond, pointsPerError);
			GetCurrentMetrics().SetStars(CalculateStars());
        }

        internal void AddWrongAnswer() {
			GetCurrentMetrics().AddWrongAnswer();
        }

        internal void AddRightAnswer()
        {
			GetCurrentMetrics().AddWrongAnswer();
        }
			
		public List<GameMetrics> SearchMetricsByGame(int gameId){
			for (int i = 0; i < metrics.Count; i++) {
				if (metrics [i] [0].GetGameId () == gameId)
					return metrics [i];
			}
			Debug.Log ("ERROR: No metrics for this game, check MetricsModel.SearchMetricsByGame");
			return null;
		}

        internal GameMetrics GetCurrentMetrics()
        {
			return currentGameMetrics [currentGameMetrics.Count - 1];
        }

	

        internal List<int> GetLevelIndexes(int currentPage, int maxRows)
        {
            List<int> myList = new List<int>(maxRows);
            int initIndex = currentPage * maxRows;
            for (int i = initIndex; i < initIndex + maxRows && i < metrics.Count && initIndex >= 0; i++)
            {
                myList.Add(i);
            }
            return myList;
        }

        internal GameMetrics GetBestMetric(int level)
        {
            if (metrics[level].Count == 0) return null;
            GameMetrics max = metrics[level][0];
            for (int i = 1; i < metrics[level].Count; i++){
				if (metrics[level][i].GetScore() > max.GetScore()){max = metrics[level][i];}
            }
			Debug.Log(level + ": max score -> " + max.GetScore());
            return max;
        }
      
        public void GameStarted(){
			Game currentGame = AppController.GetController ().GetAppModel ().GetCurrentGame ();
			GameMetrics newMetrics = new GameMetrics (currentGame.GetId());
			currentGameMetrics = SearchMetricsByGame (currentGame.GetId());

			if (currentGameMetrics != null) {
				currentGameMetrics.Add (newMetrics);	
			} else {
				currentGameMetrics = new List<GameMetrics> ();
				currentGameMetrics.Add (newMetrics);
				metrics.Add (currentGameMetrics);
			}
				
        }       

        private void CalculateFinalScore(int lapsedSeconds, int minSeconds, int pointsPerSecond, int pointsPerError){
            int score = MAX_SCORE - GetCurrentMetrics().GetWrongAnswers() * pointsPerError;
			if (GetCurrentMetrics().GetScore() < MIN_SCORE) score = MIN_SCORE;
			GetCurrentMetrics ().SetScore (score);
        }

        public int GetFinalScore(){
			return GetCurrentMetrics().GetScore();
        }
     
        private int CalculateStars(){
			float percentage = (GetCurrentMetrics().GetScore() + 0f) / (MAX_SCORE + 0f);

            if (percentage > 0.75)
            {
                return 3;
            } else if(percentage > 0.5)
            {
                return 2;
            } else if(percentage > 0.25)
            {
                return 1;
            } else
            {
                return 0;
            }
        }

        public List<List<GameMetrics>> GetMetrics()
        {
            List<List<GameMetrics>> filteredList = new List<List<GameMetrics>>();
            for(int i = 0; i < metrics.Count; i++)
            {
				filteredList.Add(new List<GameMetrics>());
                for(int j = 0; j < metrics[i].Count; j++) {
                    filteredList[i].Add(metrics[i][j]);
                }
            }
            return filteredList;
        }
    }
}

