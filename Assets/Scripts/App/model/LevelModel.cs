using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts.App
{
    public abstract class LevelModel
    {
        protected int minSeconds;
        protected int pointsPerSecond;
        protected int pointsPerError;

        public abstract void StartGame();
        public abstract void NextChallenge();
        public abstract void RequestHint();

        public LevelModel() {}

        public int PointsPerError {
            get { return pointsPerError; }
        }

        public int PointsPerSecond {
            get { return pointsPerSecond;}
        }

        public int MinSeconds{
            get{ return minSeconds;}
        }
     
    }
}
