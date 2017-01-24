using System;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Games.NeighbourhoodActivity {
	public class Building : IEquatable<Building> {
		string name;
		bool isDouble;
		bool isStreet;
		bool isLeft;

		private Building(string name, bool isDouble = false, bool isStreet = false, bool isLeft = false) {
			this.isLeft = isLeft;
			this.isStreet = isStreet;
			this.isDouble = isDouble;
			this.name = name;
		}

		public static Building B(string name, bool isDouble = false, bool isStreet = false, bool isLeft = false){
			return new Building(name, isDouble, isStreet, isLeft);
		}

		public string GetName(){
			return name;
		}

		public bool IsDouble(){
			return isDouble;
		}

		public bool IsStreet(){
			return isStreet;
		}

		public bool IsLeft(){
			return isLeft;
		}

		public Sprite GetSprite(Dictionary<string, Sprite> buildingSprites) {
			return buildingSprites[GetName() + (isDouble ? (isLeft ? "Left" : "Right") : "")];
		}

		#region IEquatable implementation

		public bool Equals(Building other) {
			if(other == null) return false;
			return name == other.name && isDouble == other.isDouble && isStreet == other.isStreet;
		}

		#endregion
	}
}