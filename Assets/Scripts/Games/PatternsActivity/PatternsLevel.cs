using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Assets.Scripts.Common;

namespace Assets.Scripts.Games.PatternsActivity {
	public class PatternsLevel {
		int imageIndex;
		List<string> usedColors, colorArray;

		public PatternsLevel(int imageIndex, List<string> usedColors, List<string> colorArray) {
			this.imageIndex = imageIndex;
			this.usedColors = usedColors;
			this.colorArray = colorArray;
		}

		public List<string> GetColors(List<string> allColors, int maxColors) {
			List<string> colors = new List<string>(usedColors);

			if(!colors.Contains("BLANK")) colors.Add("BLANK");

			Randomizer colorRandomizer = Randomizer.New(allColors.Count - 1);
			while(colors.Count < maxColors){
				string c = allColors[colorRandomizer.Next()];
				if(!colors.Contains(c)) colors.Add(c);
			}
			return colors;
		}

		public List<string> GetColorArray(){
			return colorArray;
		}

		public int GetImageIndex(){
			return imageIndex;
		}
	}
}