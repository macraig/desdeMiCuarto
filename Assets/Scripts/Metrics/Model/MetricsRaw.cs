using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.Metrics.Model{
public class MetricsRaw {

    private GameObject fullObject;

    public MetricsRaw(GameObject fullObject)
    {
        this.fullObject = fullObject;
    }

	// Use this for initialization
	void Start () {
       
    }
	
    public void setActivity(string activityName)
    {
        fullObject.GetComponentsInChildren<Text>(true)[0].text = activityName;
    }

    public void setScore(int currentScore)
    {
        fullObject.GetComponentsInChildren<Text>(true)[1].text = (currentScore == 0 ? "-" : "" + currentScore);
    }

    public void setStars(int currentStars)
    {

        for(int i = 0; i < fullObject.GetComponentsInChildren<Image>(true).Length; i++)
        {            
            fullObject.GetComponentsInChildren<Image>(true)[i].gameObject.SetActive(i < currentStars);
        }
    }

    public Button getViewDetailsBtn(){
        return fullObject.GetComponentInChildren<Button>();
    }

    internal void Hide(){
        fullObject.SetActive(false);
    }

    internal void Show(){
        fullObject.SetActive(true);
    }
}
}