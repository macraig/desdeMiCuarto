﻿using System;
using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Common.Dragger;
using Assets.Scripts.Metrics.Model;

namespace Assets.Scripts.Games.BedroomActivity {
	public class BedroomActivityModel : LevelModel {
		private int currentLvl, lvlCorrect = 0;
		private List<BedroomLevel> lvls;
		private AudioClip levelAudio;

	

		public void NextLvl(){
			currentLvl++;

			lvlCorrect = 0;
		}

		public bool GameEnded(){
			return currentLvl == lvls.Count;
		}

		public bool CanDropInSlot(DraggerSlot slot) {
			if (slot.gameObject == lvls [currentLvl].Target ())
				return true;
			
			List<GameObject> falseTargets = lvls [currentLvl].FalseTargets ();
			foreach (GameObject falseTarget in falseTargets) {
				if (slot.gameObject == falseTarget)
					return true;
				
			}
			return false;

		}

		public bool TargetIsCorrect (DraggerSlot where)
		{
			return where.gameObject == lvls [currentLvl].Target ();
		}

		public int CurrentLvl(){
			return currentLvl;
		}

		public void SetCurrentLevel(bool enabled) {
			lvls[currentLvl].Set(enabled);
			levelAudio = lvls [currentLvl].GetAudioClip ();
		}

		public void EnableLevelComponents(bool enabled){
			lvls[currentLvl].Set(enabled);
		}

		public AudioClip GetLevelSound(){
			return levelAudio;
		}

		private BedroomActivityModel(List<BedroomLevel> lvls) {
			this.lvls = lvls;
			currentLvl = 0;
			MetricsController.GetController().GameStart();

		}

		public static BedroomActivityModel StartFromJson(JSONArray lvls){
			List<BedroomLevel> bedroomLvls = new List<BedroomLevel>();
			foreach(JSONClass lvl in lvls) {
				StageType type = (StageType) Enum.Parse(typeof(StageType), lvl["type"].Value);
				string sound = lvl["sound"].Value;

				switch(type) {
				case StageType.CLICK:
				case StageType.TOGGLE:
					List<GameObject> correct = GameObjects(lvl["correct"].AsArray);
					List<GameObject> wrong = GameObjects(lvl["wrong"].AsArray);

					bedroomLvls.Add(BedroomLevel.FromClickOrToggle(type, correct, wrong, sound));
					break;
				case StageType.DRAG:
					GameObject target = GameObject.Find(lvl["target"].Value);
					List<GameObject> falseTargets = GameObjects(lvl["wrong"].AsArray);
					List<GameObject> draggers = GameObjects(lvl["object"].AsArray);

					bedroomLvls.Add(BedroomLevel.FromDrag(type, draggers, target,falseTargets, sound));
					break;
				case StageType.CARPET_SCREEN:
					bedroomLvls.Add(BedroomLevel.FromScreen(type));
					break;
				}
			}

			return new BedroomActivityModel(bedroomLvls);
		}

		static List<GameObject> GameObjects(JSONArray lvl) {
			return new List<JSONNode>(lvl.Childs).ConvertAll((JSONNode c) => GameObject.Find(c.Value));
		}

		public void Correct() {
			LogAnswer(true);
			lvlCorrect++;
		}

		public bool IsLvlDone(){
			return lvlCorrect == lvls[currentLvl].Correct().Count;
		}

		public void Wrong(){
			LogAnswer(false);
		}

		public void SetToggle(Sprite[] spr) {
			lvls[currentLvl].SetToggle(spr);
		}

		public bool CurrentCarpet() {
			return lvls[currentLvl].IsCarpet();
		}

		public void RestartGame(){
			
		}
	}
}

