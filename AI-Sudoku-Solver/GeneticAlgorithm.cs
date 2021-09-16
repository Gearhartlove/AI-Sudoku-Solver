using System;
using System.Collections.Generic;
using System.Data.Common;

namespace AI_Sudoku_Solver
{
    public class GeneticAlgorithm
    {
       //Generate Completely Random Board for Population
       private const int popSize = 20;
       private const double SelectionPercentage = 0.20;
       private const double SelectionPressure = 0.80;
       private Random rand;
       private SudokuPuzzle OriginalPuzzle;
       
       
       //tournament selection
       //rinse and repeat until I find an ideal board

       public GeneticAlgorithm()
       {
           rand = new Random();
           OriginalPuzzle = new SudokuPuzzle("Easy-P1.csv"); //TODO Change later for all files implementation
           List<SudokuPuzzle> blankPopulation = new List<SudokuPuzzle>();
           //Generate Local Population
           List<SudokuPuzzle> randomPopulation = GenerateRandomBoard(blankPopulation, in OriginalPuzzle);
           Tournament(Selection(randomPopulation, SelectionPercentage), SelectionPressure);
           //SudokuPuzzle winner = Tournament(Selection(randomPopulation, SelectionPercentage), SelectionPressure);

           //GenerateRandomBoards()
           //Evaluate()
           //Loop : stop when I have a solved board
           //  selectedPopulation[] = Selection(randomPopulation)
           //  tournamentVictors[] = Tournament(selectedPopulation[]) 
           //  Crossover(tournamentVictors[], randomPopulation) //using One Point Crossover ??? do more research 
           //  Mutate(???) //Who am I mutating? How am I mutating them?
           //  Evaluate(randomPopulation[])
           //  randomPopulation = Replace(randomPopulation[]) //Who am I replacing? 
       }

       /// <summary>
       /// Randomizes the initial population
       /// </summary>
       private List<SudokuPuzzle> GenerateRandomBoard(List<SudokuPuzzle> population, in SudokuPuzzle original)
       {
           for (int i = 0; i < popSize; i++)
           {
               SudokuPuzzle newRandPop = new SudokuPuzzle("Easy-P1.csv"); 
               //go through each cell, check if it can be change; if it can be changed, assign random number to it]
               for (int column = 0; column < 9; column++)
               {
                   for (int row = 0; row < 9; row++)
                   { 
                       //change values of cells which are not already filled from start state
                       if (newRandPop.getValue(column, row) == -1)
                           newRandPop.setValue(column, row, rand.Next(1, 10));
                       //lock already filled cells in
                       else
                           newRandPop.setLockedValue(column, row, newRandPop.getValue(column, row));
                   }
               }
               population.Add(newRandPop);
           }
           return population;
       }

       /// <summary>
       /// Randomly Selects a portion of the population to take part in the tournament
       /// Enter the tournament
       /// </summary>
       private List<SudokuPuzzle> Selection(List<SudokuPuzzle> boards, double percentage)
       {
           //pick X% of the population of the population
           int selectPopNumber = (int)Math.Round(boards.Count * percentage);
           List<SudokuPuzzle> selectedPopulation = new List<SudokuPuzzle>();
           List<int> randomList = new List<int>();
           int randomNumber = -1;
           //Select Populations
           for (int i = 0; i < selectPopNumber; i++)
           {
               do
               {
                   randomNumber = rand.Next(boards.Count);
                   randomList.Add(randomNumber);
               } while (!randomList.Contains(randomNumber));
               selectedPopulation.Add(boards[randomNumber]);
           }
           Console.WriteLine(selectedPopulation.Count);  
           return selectedPopulation;
       }

       /// <summary>
       /// Survival of the fittest, based on initial selection of population. This is chosen using X. (Still neeed to
       /// decide). Selection Pressure determined by: p*(1-p)^a where p is probability and a is individual's fitness in
       /// the table
       /// </summary>
       private void Tournament(List<SudokuPuzzle> selection, double percentage)
       {
           
       }
       
       /// <summary>
       /// Mutates Population by using the X operation and Y Selection. (Still need to decide on these)
       /// </summary>
       private void MutatePop()
       {
           
       }


       private void Crossover()
       {
           
       }
       
       /// <summary>
       /// Instantiate GeneticAlgorithm object (calls the constructor) and have a while loop which runs until the
       /// algorithm is solved. General Process Flow: Mutate -> Tournament Selection -> ... 
       /// </summary>
       public static void SolvePuzzle() //files for the board as a parameter?
       {
          //instantiate genetic algorithm object
          //while loop of 
       }

    }
}