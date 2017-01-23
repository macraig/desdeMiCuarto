using System;
using Assets.Scripts.Games.TreasureActivity;
using UnityEngine;

namespace Assets.Scripts.Games.TreasureActivity {
	public class Figure : IEquatable<Figure> {
		private bool isBig;
		private int shape, color;

		private Figure() { }

		public static Figure F(){
			return new Figure();
		}

		public Figure Big(){
			isBig = true;
			return this;
		}

		public Figure Size(bool isBig){
			this.isBig = isBig;
			return this;
		}

		public Figure Shape(int s){
			shape = s;
			return this;
		}

		public Figure Color(int c){
			color = c;
			return this;
		}

		public bool Equals(Figure other) {
			return isBig == other.isBig && shape == other.shape && color == other.color;
		}

		public int GetShape() {
			return shape;
		}

		public int GetColor() {
			return color;
		}

		public bool IsBig(){
			return isBig;
		}

		public int SpriteNumber() {
			return shape + (8 * color) + (isBig ? 4 : 0);
		}
	}
}