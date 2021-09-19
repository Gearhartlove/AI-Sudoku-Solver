using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

//LOG TO OUTPUF FILE 
//Implement the Interface for Formatting 
//Make formatting method 
namespace AI_Sudoku_Solver
{
    public class GeneticAlgorithm : IResults, ISolver
    {
       //Generate Completely Random Board for Population
       private const int PopSize = 100;
       private const double SelectionPercentage = 0.15;
       private const double ProgramCounter = 2000;
       private const double MutationTicks = 5;
       //private const double 
       private readonly Random rand;
       private SudokuPuzzle OriginalPuzzle;
       private SudokuPuzzle SolutionPuzzle;
       private List<Population> blankPopulation;
       private List<Population> randomPopulation;

       /// <summary>
       /// Constructor for instantiating variables
       /// </summary>
       public GeneticAlgorithm()
       {
           rand = new Random();
           blankPopulation = new List<Population>();
       }

       public string PrintResults() {
           return "";
       }

       public SudokuPuzzle solve()
       {
           return SolutionPuzzle;
       }
       
        /// <summary>
        /// Attempts to solve the sudoku puzzle within {ProgramCounter} attempts. Runs and orchestrates the entirety of
        /// the program, including the Selection, Tournament, Crossover, Mutate, Evaluate, and Replace methods.
        /// </summary>
        /// <param name="puzzle"></param>
       public  void Solve(SudokuPuzzle puzzle)
       {
           randomPopulation = GenerateRandomBoard(blankPopulation, puzzle);
           
           for (int loops = 0; loops < ProgramCounter; loops++)
           { 
               List<Population> crossoverParents = new List<Population>(); 
               //Select Two Winners of the tournament, from a random selection of the population
               crossoverParents.Add(Tournament(Selection(randomPopulation, SelectionPercentage)));
               crossoverParents.Add(Tournament(Selection(randomPopulation, SelectionPercentage)));
               var newPopMembers= Crossover(in crossoverParents, puzzle);
               Mutate(ref newPopMembers);
               Evaluate(newPopMembers);
               Replace(ref randomPopulation, newPopMembers);
           }
       }
        
        /// <summary>
        /// Randomizes the initial population board states.
        /// </summary>
        /// <param name="population"></param>
        /// <param name="original"></param>
        /// <returns></returns>
        private List<Population> GenerateRandomBoard(List<Population> population, SudokuPuzzle original)
       {
           for (int i = 0; i < PopSize; i++)
           {
               Population newRandPop = new Population(original); 
               //go through each cell, check if it can be change; if it can be changed, assign random number to it]
               for (int column = 0; column < 9; column++)
               {
                   for (int row = 0; row < 9; row++)
                   { 
                       //change values of cells which are not already filled from start state
                       if (newRandPop.pop.getValue(column, row) == -1)
                           newRandPop.pop.setValue(column, row, rand.Next(1, 10));
                       //lock already filled cells in
                       else
                           newRandPop.pop.setLockedValue(column, row, newRandPop.pop.getValue(column, row));
                   }
               }

               newRandPop.constraints_violated = newRandPop.pop.constraintTest();
               population.Add(newRandPop);
           }
           //sort population, index[0]worst -> index[list.count-1]best
           population.Sort((x,y)=>x.constraints_violated.CompareTo(y.constraints_violated));
           population.Reverse();
           return population;
       }

        /// <summary>
        /// Selects a random subset of the population based on the {SelectionPercentage} tuneable variable. Sorts
        /// the array from Worst -> Best, with the worst (fitness wise) being at index 0.
        /// </summary>
        /// <param name="boards"></param>
        /// <param name="percentage"></param>
        /// <returns></returns>
       private List<Population> Selection(List<Population> boards, double percentage)
       {
           //pick X% of the population of the population
           int selectPopNumber = (int)Math.Round(boards.Count * percentage);
           List<Population> selectedPopulation = new List<Population>();
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
           selectedPopulation.Sort((x,y)=>x.constraints_violated.CompareTo(y.constraints_violated));
           selectedPopulation.Reverse(); //sort worst -> best
           return selectedPopulation;
       }
        
        /// <summary>
        /// Returns the population member from the selection which violated the least constraints (highest fitness).
        /// </summary>
        /// <param name="selection"></param>
        /// <returns></returns>
       private Population Tournament(List<Population> selection)
       {
           return selection[selection.Count-1]; //TODO check the sorting of the constraints
       }
        
        /// <summary>
        /// Uses a uniform operator to create children based on the two tournament winner parents.
        /// </summary>
        /// <param name="Winners"></param>
        /// <returns></returns>
       private List<Population> Crossover(in List<Population> Winners, SudokuPuzzle puzzle)
       {
           //Iterate through each cell of cell, 50% chance to take number from Winner1, 50% chance to 
           //take number from Winner2 for any given cell
           //Output two new children
           List<Population> children = new List<Population>();  
           children.Add(new Population(puzzle)); //TODO Change
           children.Add(new Population(puzzle));
           for (int row = 0; row < 9; row ++)
           {
               for (int column = 0; column < 9; column ++)
               {
                   //TODO Check with Debugger :)
                   //For every given cell, 50% to input winner1 value, 50% to input winner2 value 
                   //Creates 2 new children engineered from the parents
                   if (rand.Next(0, 2) == 0)
                   {
                       //child A get A gene
                       children[0].pop.setValue(column, row, Winners[0].pop.getValue(column, row));
                       //child B get B gene
                       children[1].pop.setValue(column, row, Winners[1].pop.getValue(column, row));
                   }
                   else
                   {
                       //child A get B gene
                       children[0].pop.setValue(column, row, Winners[1].pop.getValue(column, row));
                       //child B get A gene
                       children[1].pop.setValue(column, row, Winners[0].pop.getValue(column, row));
                   }
               }
           }
           return children;
       }
        
        /// <summary>
        /// Mutates the children using random mutation by selecting a random cell and changing the value to any value
        /// between 1 and 9.
        /// </summary>
        /// <param name="cross_children"></param>
        /// <returns></returns>
       private List<Population> Mutate(ref List<Population> cross_children)
       {
           for (int m = 0; m < MutationTicks; m++)
           {
               foreach (Population child in cross_children)
               {
                   int x = rand.Next(0, 9);
                   int y = rand.Next(0, 9);
                   child.pop.setValue(x, y, rand.Next(1, 10));
               }

           }

           return cross_children;
       }
        
        /// <summary>
        /// Calculate the constraints violated by the two children created from cross-over, after they have mutated.
        /// If one of the children has 0 constraints violated, assign the solution and return true.
        /// </summary>
        /// <param name="cross_children"></param>
        /// <returns></returns>
       private bool Evaluate(List<Population> cross_children)
       {
           cross_children[0].constraints_violated = cross_children[0].pop.constraintTest();
           cross_children[1].constraints_violated = cross_children[1].pop.constraintTest();

           Console.WriteLine(cross_children[0].constraints_violated);
           Console.WriteLine(cross_children[1].constraints_violated);
           if (cross_children[0].constraints_violated == 0)
           {
               SolutionPuzzle = cross_children[0].pop;
               return true;
           }
           else if (cross_children[1].constraints_violated == 0)
           {
               SolutionPuzzle = cross_children[1].pop;
               return true;
           }

           return false;
       }

        /// <summary>
        /// Replace the two worst population members with the two newly created and mutated cross-over children.
        /// </summary>
        /// <param name="random_pop"></param>
        /// <param name="adding_pop"></param>
       private void Replace(ref List<Population> random_pop, List<Population> adding_pop)
       {
          //sort by evaluation, remove the worst two pop, then add the newest two pop
          random_pop.RemoveAt(0);
          random_pop.RemoveAt(1);
          random_pop.Add(adding_pop[0]);
          random_pop.Add(adding_pop[1]);
       }
    }
}