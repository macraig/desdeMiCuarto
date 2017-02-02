using System;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Metrics.Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Games.TreasureActivity {
	public class TreasureActivityModel : LevelModel {
		private int currentLvl, exercisesDone;
		private List<Figure> pattern, choices;
	    private int _tableSize;
        private bool _lastCorrect;

        private static int SHAPES = 4, COLORS = 5;

	    public bool LastCorrect
	    {
	        get { return _lastCorrect; }
	        set { _lastCorrect = value; }
	    }

	    public int TableSize
	    {
	        get { return _tableSize; }
	        set { _tableSize = value; }
	    }


	    public bool GameEnded(){
			return exercisesDone == 6;
		}

		public int CurrentLvl(){
			return currentLvl;
		}

		public void NextLvl(){
			currentLvl++;
		}

		public TreasureActivityModel()
		{
            _lastCorrect = true;
            exercisesDone = 0;
			currentLvl = 0;
            pattern = new List<Figure>();
            choices = new List<Figure>();
            MetricsController.GetController().GameStart();

		}

		public void Correct() {
			LogAnswer(true);
            AddExerciseDOne();
		}

		void AddExerciseDOne() {
			exercisesDone++;
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


	    public void SetLevel()
	    {
	        pattern.Clear();
            choices.Clear();
            Randomizer shapeRand = Randomizer.New(SHAPES - 1);
            Randomizer colorRand = Randomizer.New(COLORS - 1);
            switch (currentLvl)
	        {        
	            case 0:
                    TableSize = 4;
                    for (int i = 3; i >= 0; i--)
                     {
                         Figure figure = Figure.F().Color(colorRand.Next()).Shape(shapeRand.Next());
                         figure.Size(false);
                         pattern.Add(figure);
                         choices.Add(figure);
                     }
                    do
                    {
                        Utils.Shuffle(choices);
                    } while (choices[0].Equals(pattern[0]) && choices[1].Equals(pattern[1]) &&
                             choices[2].Equals(pattern[2]));
                    break;

                case 1:
                    TableSize = 4;
	                do
	                {
	                    for (int i = 5; i >= 0; i--)
	                    {
	                        Figure figure = Figure.F().Color(colorRand.Next()).Shape(shapeRand.Next());
	                        figure.Size(false);
	                        if (pattern.Count < 4) pattern.Add(figure);
	                        choices.Add(figure);
	                    }
	                } while (AreTwoFiguresEquals(choices));
                    
	                int target;
	                int source;
                    do
	                {
                        target = Random.Range(1, 3);
	                    source = Random.Range(0, 4);
	                } while (target == source);

	                pattern[target] = pattern[source];
	                if (choices.FindAll(e => e.GetColor() == pattern[target].GetColor()).Count < 2 )
	                {
	                    choices.Find(e => e.GetColor() != pattern[target].GetColor()).Color(pattern[target].GetColor());
	                }
                    Utils.Shuffle(choices);
                    break;


                case 2:
                    TableSize = 4;

                    do
                    {
                        pattern.Clear();
                        choices.Clear();
                        for (int i = 5; i >= 0; i--)
                        {
                            colorRand.Restart();
                            Figure figure = Figure.F().Color(colorRand.Next()).Shape(shapeRand.Next()).Size(Randomizer.RandomBoolean());
                            if (pattern.Count < 4) pattern.Add(figure);
                            choices.Add(figure);
                        }
                    } while (choices.FindAll(e => e.IsBig()).Count < 2 || choices.FindAll(e => !e.IsBig()).Count < 2 || AreTwoFiguresEquals(choices) || pattern.FindAll(e => e.IsBig()).Count < 1 || pattern.FindAll(e => !e.IsBig()).Count < 1);

               
                    Utils.Shuffle(choices);
                    break;

                case 3:
					
                    TableSize = 8;
               
                    do
                    {
                        pattern.Clear();
                        choices.Clear();
                        bool isBig = Randomizer.RandomBoolean();
                        for (int i = 5; i >= 0; i--)
                        {
                            colorRand.Restart();
                            Figure figure = Figure.F().Color(colorRand.Next()).Shape(shapeRand.Next()).Size(isBig);
                            if (pattern.Count < 4) pattern.Add(figure);
                            choices.Add(figure);
                        }
                        Figure toChangeSize = choices[0];
                        choices[4] = Figure.F().Color(toChangeSize.GetColor()).Shape(toChangeSize.GetShape()).Size(!toChangeSize.IsBig());
                        Figure toChangeShape = choices[1];
                        choices[5] = Figure.F().Color(toChangeShape.GetColor()).Shape(GetDifferntShape(toChangeShape.GetShape())).Size(toChangeShape.IsBig());

                    } while (AreTwoFiguresEquals(choices));

	                do
	                {
	                    Utils.Shuffle(choices);
	                } while (choices[0].Equals(pattern[0]) && choices[1].Equals(pattern[1]) &&
	                         choices[2].Equals(pattern[2]));

                    break;
                case 4:
                    TableSize = 8;

                    do
                    {
                        pattern.Clear();
                        choices.Clear();
                        for (int i = 5; i >= 0; i--)
                        {
                            colorRand.Restart();
                            Figure figure = Figure.F().Color(colorRand.Next()).Shape(shapeRand.Next()).Size(Randomizer.RandomBoolean());
                            if(pattern.Count < 4) pattern.Add(figure);
                            choices.Add(figure);
                        }
                    } while (choices.FindAll(e => e.IsBig()).Count < 2 || choices.FindAll(e => !e.IsBig()).Count < 2 || AreTwoFiguresEquals(choices));


                    // repeticion de figura
                    int aTarget;
                    int aSource;
                    do
                    {
                        aTarget = Random.Range(1, 3);
                        aSource = Random.Range(0, 4);
                    } while (aTarget == aSource);

                    pattern[aTarget] = pattern[aSource];

                    do
                    {
                        Utils.Shuffle(choices);
                    } while (choices[0].Equals(pattern[0]) && choices[1].Equals(pattern[1]) &&
                             choices[2].Equals(pattern[2]));

                    break;
                case 5:
                    // 
                    TableSize = 8;

                    do
                    {
                        pattern.Clear();
                        choices.Clear();
                        for (int i = 5; i >= 0; i--)
                        {
                            Figure figure = Figure.F().Color(colorRand.Next()).Shape(shapeRand.Next()).Size(Randomizer.RandomBoolean());
                            if (pattern.Count < 4) pattern.Add(figure);
                            choices.Add(figure);
                        }
                    } while (choices.FindAll(e => e.IsBig()).Count < 2 || choices.FindAll(e => !e.IsBig()).Count < 2 || AreTwoFiguresEquals(choices));


                    // palindrome
                    pattern[3] = pattern[0];

                    if (Randomizer.RandomBoolean())
                    {
                        pattern[2] = pattern[1];
                    }

                    do
                    {
                        Utils.Shuffle(choices);
                    } while (choices[0].Equals(pattern[0]) && choices[1].Equals(pattern[1]) &&
                             choices[2].Equals(pattern[2]));

                    break;
            }
            
            // old cases without table size 
            /*  switch (currentLvl)
	        {        
	            case 0:

                    for (int i = 3; i >= 0; i--)
                     {
                         Figure figure = Figure.F().Color(colorRand.Next()).Shape(shapeRand.Next());
                         figure.Size(false);
                         pattern.Add(figure);
                         choices.Add(figure);
                     }
                     Utils.Shuffle(pattern);
                     Utils.Shuffle(choices);

                    break;

                case 1:

                    for (int i = 3; i >= 0; i--)
                    {
                        Figure figure = Figure.F().Color(colorRand.Next()).Shape(shapeRand.Next());
                        figure.Size(false);
                        pattern.Add(figure);
                        choices.Add(figure);
                    }
	                int target;
	                int source;
                    do
	                {
                        target = Random.Range(1, 3);
	                    source = Random.Range(0, 4);
	                } while (target == source);

	                pattern[target] = pattern[source];
                    Utils.Shuffle(choices);
                    break;


                case 2:

                    do
                    {
                        pattern.Clear();
                        choices.Clear();
                        for (int i = 3; i >= 0; i--)
                        {
                            Figure figure = Figure.F().Color(colorRand.Next()).Shape(shapeRand.Next());
                            pattern.Add(figure);
                            choices.Add(figure);
                        }
                    } while (AreTwoFiguresEquals(choices));

                    // palindrome
                    pattern[3] = pattern[0];

                    if (Randomizer.RandomBoolean())
                    {
                        pattern[2] = pattern[1];
                    }
                    Utils.Shuffle(choices);
                    break;

                case 3:

                    do
                    {
                        pattern.Clear();
                        choices.Clear();
                        for (int i = 5; i >= 0; i--)
                        {
                            Figure figure = Figure.F().Color(colorRand.Next()).Shape(shapeRand.Next()).Size(Randomizer.RandomBoolean());
                            if (pattern.Count < 4) pattern.Add(figure);
                            choices.Add(figure);
                        }
                    } while (choices.FindAll(e => e.IsBig()).Count < 2 || choices.FindAll(e => !e.IsBig()).Count < 2 || AreTwoFiguresEquals(choices));

	                do
	                {
	                    Utils.Shuffle(choices);
	                } while (choices[0].Equals(pattern[0]) && choices[1].Equals(pattern[1]) &&
	                         choices[2].Equals(pattern[2]));

                    break;
                case 4:

                    do
                    {
                        pattern.Clear();
                        choices.Clear();
                        for (int i = 5; i >= 0; i--)
                        {
                            Figure figure = Figure.F().Color(colorRand.Next()).Shape(shapeRand.Next()).Size(Randomizer.RandomBoolean());
                            if(pattern.Count < 4) pattern.Add(figure);
                            choices.Add(figure);
                        }
                    } while (choices.FindAll(e => e.IsBig()).Count < 2 || choices.FindAll(e => !e.IsBig()).Count < 2 || AreTwoFiguresEquals(choices));


                    int aTarget;
                    int aSource;
                    do
                    {
                        aTarget = Random.Range(1, 3);
                        aSource = Random.Range(0, 4);
                    } while (aTarget == aSource);

                    pattern[aTarget] = pattern[aSource];

                    do
                    {
                        Utils.Shuffle(choices);
                    } while (choices[0].Equals(pattern[0]) && choices[1].Equals(pattern[1]) &&
                             choices[2].Equals(pattern[2]));

                    break;
                case 5:

                    do
                    {
                        pattern.Clear();
                        choices.Clear();
                        for (int i = 5; i >= 0; i--)
                        {
                            Figure figure = Figure.F().Color(colorRand.Next()).Shape(shapeRand.Next()).Size(Randomizer.RandomBoolean());
                            if (pattern.Count < 4) pattern.Add(figure);
                            choices.Add(figure);
                        }
                    } while (choices.FindAll(e => e.IsBig()).Count < 2 || choices.FindAll(e => !e.IsBig()).Count < 2 || AreTwoFiguresEquals(choices));


                    // palindrome
                    pattern[3] = pattern[0];

                    if (Randomizer.RandomBoolean())
                    {
                        pattern[2] = pattern[1];
                    }

                    do
                    {
                        Utils.Shuffle(choices);
                    } while (choices[0].Equals(pattern[0]) && choices[1].Equals(pattern[1]) &&
                             choices[2].Equals(pattern[2]));

                    break;
            }*/
	    }

	    private int GetDifferntShape(int getShape)
	    {
            Randomizer shapeRand = Randomizer.New(SHAPES - 1);
	        int shapeToReturn;
	        do
	        {
	            shapeToReturn = shapeRand.Next();
	        } while (shapeToReturn == getShape);

	        return shapeToReturn;
	    }

	    private bool AreTwoFiguresEquals(List<Figure> figures)
	    {
	        for (int i = figures.Count - 1; i >= 0; i--)
	        {
	            for (int j = figures.Count - 1; j >= 0; j--)
	            {
	                if(j == i) continue;
	                if (figures[i].Equals(figures[j])) return true;
	            }
	        }
            return false;
        }


	    /*public void SetLevel(){
			pattern.Clear();
			choices.Clear();

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
		}*/

		void CopyChoices() {
			if(pattern.Count == 4) choices = new List<Figure>(pattern);
		}
	}
}