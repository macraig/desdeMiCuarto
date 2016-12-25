using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class GameMetrics  {

	public int lapsedTime,rightAnswers,wrongAnswers,hints,stars,score;
	public int activity;
    public int mode;
    internal string date;


    public GameMetrics(int activity, int mode){
        this.activity = activity;
		stars = 0;
        lapsedTime = 0;
        rightAnswers = 0;
        wrongAnswers = 0;
        hints = 0;
        score = 0;
        date = System.DateTime.Now.Day + "/" + System.DateTime.Now.Month + "/" + System.DateTime.Now.Year;
        this.mode = mode;
    }

    internal void Reset()
    {
        stars = 0;
        lapsedTime = 0;
        rightAnswers = 0;
        wrongAnswers = 0;
        hints = 0;
        score = 0;
    }

    internal bool IsMode(int mode)
    {
        return this.mode == mode;
    }
}
