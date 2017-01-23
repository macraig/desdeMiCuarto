using System;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Metrics.Model;
using System.Globalization;

namespace Assets.Scripts.Games.TreasureActivity {
	public class TreasureActivityModel : LevelModel {
		private int currentLvl, difficulty;
		private List<Figure> pattern, choices;

		private static int SHAPES = 4, COLORS = 5;

		public bool GameEnded(){
			return currentLvl == 6;
		}

		public int CurrentLvl(){
			return currentLvl;
		}

		public void NextLvl(){
			currentLvl++;
		}

		public TreasureActivityModel() {
			difficulty = 0;
			currentLvl = 0;
			MetricsController.GetController().GameStart();

		}

		public void Correct() {
			LogAnswer(true);

			NextDifficulty();
		}

		void NextDifficulty() {
			difficulty++;
		}

		public void Wrong(){
			LogAnswer(false);
		}

		public List<Figure> GetPattern() {
			return pattern;
		}

		public List<Figure> GetOptions() {
			return choices;
		}

		// Horrible y cableadisimo porque no entendi nada del doc :)
		public void SetLevel(){
			pattern = new List<Figure>();
			choices = new List<Figure>();

			Randomizer shapeRand = Randomizer.New(SHAPES - 1);
			Randomizer secondShapeRand = Randomizer.New(SHAPES - 1);
			Randomizer colorRand = Randomizer.New(COLORS - 1);
			Randomizer secondColorRand = Randomizer.New(COLORS - 1);
			bool palindrome = Randomizer.RandomBoolean();
			Figure newFigure;

			while(choices.Count < 6) {
				bool isBig = Randomizer.RandomBoolean();
				bool leftOrRight = Randomizer.RandomBoolean();
				
				switch(currentLvl) {
				case 0:
					//todos colores distintos, con todas formas distintas. Un solo tamaño.
					newFigure = Figure.F().Big().Color(colorRand.Next()).Shape(shapeRand.Next());
					if(pattern.Count < 4){
						pattern.Add(newFigure);
						CopyChoices();
					} else {
						if(!choices.Contains(newFigure)) choices.Add(newFigure);
					}
					break;
				case 1:
					//Puede haber repetidos, puede haber capicua, uno de cada color. Un solo tamaño
					if(pattern.Count < 2){
						newFigure = Figure.F().Big().Color(colorRand.Next()).Shape(shapeRand.Next());
						pattern.Add(newFigure);
					} else if(pattern.Count >= 2 && pattern.Count < 4){
						if(palindrome){
							pattern.Add(Figure.F().Big().Color(colorRand.Next()).Shape(pattern[0].GetShape()));
							pattern.Add(Figure.F().Big().Color(colorRand.Next()).Shape(pattern[1].GetShape()));
						} else {
							newFigure = Figure.F().Big().Color(colorRand.Next()).Shape(secondShapeRand.Next());
							pattern.Add(newFigure);
						}
						CopyChoices();
					} else {
						newFigure = Figure.F().Big().Color(colorRand.Next()).Shape(shapeRand.Next());
						if(!choices.Contains(newFigure)) choices.Add(newFigure);
					}

					break;
				case 2:
					// Un solo tamaño. Si hay formas repetidas deben ser del mismo color (en el patron nada mas, obvio)
					if(pattern.Count < 2){
						newFigure = Figure.F().Big().Color(colorRand.Next()).Shape(shapeRand.Next());
						pattern.Add(newFigure);
					} else if(pattern.Count >= 2 && pattern.Count < 4){
						if(palindrome){
							pattern.Add(Figure.F().Big().Color(pattern[0].GetColor()).Shape(pattern[0].GetShape()));
							pattern.Add(Figure.F().Big().Color(pattern[1].GetColor()).Shape(pattern[1].GetShape()));

							CopyChoices();
						} else {
							newFigure = Figure.F().Big().Color(secondColorRand.Next()).Shape(secondShapeRand.Next());

							Figure sameShapeFigure = pattern.Find((Figure f) => f.GetShape() == newFigure.GetShape());

							if(sameShapeFigure == null)
								pattern.Add(newFigure);
							else
								pattern.Add(newFigure.Color(sameShapeFigure.GetColor()));

							CopyChoices();
						}
					} else {
						newFigure = Figure.F().Big().Color(colorRand.Next()).Shape(shapeRand.Next());
						if(!choices.Contains(newFigure)) choices.Add(newFigure);
					}

					break;
				case 3:
					// Un solo tamaño. Cualquier cosa.

					newFigure = Figure.F().Big().Color(leftOrRight ? colorRand.Next() : secondColorRand.Next()).Shape(leftOrRight ? shapeRand.Next() : secondShapeRand.Next());
					if(pattern.Count < 4){
						if(!pattern.Contains(newFigure)) pattern.Add(newFigure);
						CopyChoices();
					} else {
						if(!choices.Contains(newFigure)) choices.Add(newFigure);
					}

					break;
				case 4:
					// Diferentes tamaños. No se repiten figuras, no se repiten colores.

					newFigure = Figure.F().Size(isBig).Color(colorRand.Next()).Shape(shapeRand.Next());
					if(pattern.Count < 4){
						pattern.Add(newFigure);
						CopyChoices();
					} else {
						if(!choices.Contains(newFigure)) choices.Add(newFigure);
					}

					break;
				case 5:
					// Cualquier cosa pero no se repite figura.

					newFigure = Figure.F().Size(isBig).Color(leftOrRight ? colorRand.Next() : secondColorRand.Next()).Shape(shapeRand.Next());
					if(pattern.Count < 4){
						pattern.Add(newFigure);
						CopyChoices();
					} else {
						if(!choices.Contains(newFigure)) choices.Add(newFigure);
					}

					break;
				}
			}

			choices = Randomizer.RandomizeList(choices);
		}

		void CopyChoices() {
			if(pattern.Count == 4) choices = new List<Figure>(pattern);
		}
	}
}