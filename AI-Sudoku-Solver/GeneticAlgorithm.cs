using System;
using System.Collections.Generic;
using System.Data.Common;

namespace AI_Sudoku_Solver
{
    public class GeneticAlgorithm
    {
       //Generate Completely Random Board for Population
       private const int popSize = 15;
       private const double SelectionPercentage = 1.0/3; 
       private Random rand;
       private SudokuPuzzle OriginalPuzzle;
       
       
       //tournament selection
       //rinse and repeat until I find an ideal board

       public GeneticAlgorithm()
       {
           rand = new Random();
           OriginalPuzzle = new SudokuPuzzle("Easy-P1.csv"); //TODO Change later for all files implementation
           SudokuPuzzle[] blankPopulation = new SudokuPuzzle[popSize];
           //Generate Local Population
           SudokuPuzzle[] randomPopulation = GenerateRandomBoard(blankPopulation, in OriginalPuzzle);
           SudokuPuzzle winner = Tournament(Selection(randomPopulation, SelectionPercentage));
            
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
       private SudokuPuzzle[] GenerateRandomBoard(SudokuPuzzle[] population, in SudokuPuzzle original)
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
               population[i] = newRandPop;
           }
           return population;
       }

       /// <summary>
       /// Randomly Selects a portion of the population to take part in the tournament
       /// Enter the tournament
       /// </summary>
       private SudokuPuzzle[] Selection(SudokuPuzzle[] boards, double percentage)
       {
           //pick X% of the population of the population
           int selectPopNumber = (int)Math.Round(boards.Length * percentage);
           SudokuPuzzle[] selectedPopulation = new SudokuPuzzle[selectPopNumber];
           List<int> randomList = new List<int>();
           int randomNumber = -1;
           //Select Populations
           for (int i = 0; i < selectPopNumber; i++)
           {
               do
               {
                   randomNumber = rand.Next(boards.Length);
                   randomList.Add(randomNumber);
               } while (!randomList.Contains(randomNumber));
               selectedPopulation[i] = boards[randomNumber];
           }
           
           return selectedPopulation;
       }

       /// <summary>
       /// Survival of the fittest, based on initial selection of population. This is chosen using X. (Still neeed to
       /// decide)
       /// </summary>
       private SudokuPuzzle Tournament(SudokuPuzzle[] selection)
       {
          //even
          //odd
       }
       
       /// <summary>
       /// Mutates Population by using the X operation and Y Selection. (Still need to decide on these)
       /// </summary>
       private void MutatePop()
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