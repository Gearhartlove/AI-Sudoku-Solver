using System;
using System.Data.Common;

namespace AI_Sudoku_Solver
{
    public class GeneticAlgorithm
    {
       //Generate Completely Random Board for Population
       private const int popSize = 3;
       private Random rand;
       private SudokuPuzzle OriginalPuzzle;
       
       
       //tournament selection
       //rinse and repeat until I find an ideal board

       public GeneticAlgorithm()
       {
           //array instantiation
           rand = new Random();
           OriginalPuzzle = new SudokuPuzzle("Easy-P1.csv"); //TODO Change later for all files implementation
           SudokuPuzzle[] blankPopulation = new SudokuPuzzle[popSize];
           SudokuPuzzle[] randomPopulation = GenerateRandomizePopulation(blankPopulation, in OriginalPuzzle);
           //GenerateRandomPop(); //can be used by both Local Search Algorithms
           //   -> work with the SudokuPuzzle Class
           //HillClimb(); //can be used by both Local Search Algorithms //Annealing will have a more tricks
       }

       /// <summary>
       /// Randomizes the initial population
       /// </summary>
       private SudokuPuzzle[] GenerateRandomizePopulation(SudokuPuzzle[] population, in SudokuPuzzle original)
       {
           for (int i = 0; i < popSize; i++)
           {
               SudokuPuzzle newRandPop = original; //Q: how to assign this variable to a value???
               
               //go through each cell, check if it can be change; if it can be changed, assign random number to it]
               for (int column = 0; column < 9; column++)
               {
                   for (int row = 0; row < 9; row++)
                   {
                       //check if cell is changeable
                       //set value:
                       newRandPop.setValue(column, row, rand.Next(0, 10));
                   }
               }
              
               Console.WriteLine("Random Generated: ");
               Console.WriteLine(newRandPop);
               //randomize locations
               
               population[i] = newRandPop;
           }
           return population;
       }
       
       /// <summary>
       /// Mutates Population by using the X operation and Y Selection. (Still need to decide on these)
       /// </summary>
       private void MutatePop()
       {
           
       }

       /// <summary>
       /// Survival of the fittest, based on initial selection of population. This is chosen using X. (Still neeed to
       /// decide)
       /// </summary>
       private void TournamentSelection()
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