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
           rand = new Random();
           OriginalPuzzle = new SudokuPuzzle("Easy-P1.csv"); //TODO Change later for all files implementation
           SudokuPuzzle[] blankPopulation = new SudokuPuzzle[popSize];
           //Generate Local Population
           SudokuPuzzle[] randomPopulation = GenerateRandomBoard(blankPopulation, in OriginalPuzzle);
           randomPopulation = HillClimb(randomPopulation); //can be used by both Local Search Algorithms //Annealing will have a more tricks
           //Loop : stop when I have a solved board
           //  Evaluate
           //  Mutate
           //  Tournament
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
       /// Used to create better population from the start, so that their traits are not terrible (should speed up
       /// search a lot). 
       /// </summary>
       private SudokuPuzzle[] HillClimb(SudokuPuzzle[] population)
       {
            //int permutationCap = 2000; //can change based on algorithm execution
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