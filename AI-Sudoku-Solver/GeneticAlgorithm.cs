using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace AI_Sudoku_Solver
{
    public class GeneticAlgorithm
    {
       //Generate Completely Random Board for Population
       private const int popSize = 100;
       private const double SelectionPercentage = 0.20;
       private const double SelectionPressure = 0.80;

       private const double MutationTicks = 1;
       //private const double 
       private Random rand;
       private SudokuPuzzle OriginalPuzzle;
       private SudokuPuzzle SolutionPuzzle;
       
       
       //tournament selection
       //rinse and repeat until I find an ideal board

       public GeneticAlgorithm()
       {
           rand = new Random();
           OriginalPuzzle = new SudokuPuzzle("Easy-P1.csv"); //TODO Change later for all files implementation
           List<Population> blankPopulation = new List<Population>();
           List<Population> new_pop_members = new List<Population>();
           //Generate Local Population
           List<Population> randomPopulation = GenerateRandomBoard(blankPopulation, in OriginalPuzzle);
           while (SolutionPuzzle == null)
           {
               List<Population> crossover_parents = new List<Population>();
               //Select Two Winners of the tournament, from a random selection of the population
               crossover_parents.Add(Tournament(Selection(randomPopulation, SelectionPercentage), SelectionPressure));
               crossover_parents.Add(Tournament(Selection(randomPopulation, SelectionPercentage), SelectionPressure));
               new_pop_members = Crossover(crossover_parents);
               if (Evaluate(new_pop_members)) break; // If I find a solution, break out of the loop, return solution
               //TODO think about this strategy if algo doesn't work
               Mutate(ref crossover_parents);
               Replace(ref randomPopulation, ref crossover_parents); //Steady State (replace to worst to) 
           }
           //Loop : stop when I have a solved board
           //  selectedPopulation[] = Selection(randomPopulation)
           //  tournamentVictors[] = Tournament(selectedPopulation[]) 
           //  Crossover(tournamentVictors[], randomPopulation) //
           //  Mutate(Crossovers) //Who am I mutating? How am I mutating them?
           //  Evaluate(randomPopulation[])
           //  randomPopulation = Replace(randomPopulation[]) //Who am I replacing? 
       }

       /// <summary>
       /// Randomizes the initial population
       /// </summary>
       private List<Population> GenerateRandomBoard(List<Population> population, in SudokuPuzzle original)
       {
           for (int i = 0; i < popSize; i++)
           {
               Population newRandPop = new Population("Easy-P1.csv"); 
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

               newRandPop.constraints_violated = newRandPop.pop.
               population.Add(newRandPop);
           }
           return population;
       }

       /// <summary>
       /// Randomly Selects a portion of the population to take part in the tournament
       /// Enter the tournament
       /// </summary>
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
           Console.WriteLine(selectedPopulation.Count);  
           return selectedPopulation;
       }

       /// <summary>
       /// Survival of the fittest, based on initial selection of population. This is chosen using X. (Still neeed to
       /// decide). Selection Pressure determined by: p*(1-p)^a where p is probability and a is individual's fitness in
       /// the table
       /// </summary>
       private Population Tournament(List<Population> selection, double p)
       {
           int a = 1; //fitness tier in the table of selected population (starts at 1,2,3, ... )
           double winner_piechart = 0; // used for calculating the winner of the tournament for each participant
           double winner_choice = rand.Next(); //generate random winner of the tournament
           Population winner = new Population("Easy-P1.csv"); //TODO change for any file type
           //sort selection based on fitness (constraints violated)
           selection.Sort((x,y)=>x.constraints_violated.CompareTo(y.constraints_violated));
           //For each puzzle, assign a probability to them of winning
           foreach (Population pop in selection)
           {
               pop.tourny_percentage = p * (Math.Pow((1 - p), a));
               a++;
               winner_piechart += pop.tourny_percentage;
               if (winner_piechart >= winner_choice)
               {
                   winner = pop;
                   break;
               }
           }
           //return the winner of the tournament
           return winner;
       }
        
       /// <summary>
       /// Crosses over the winners of the tournament, creating a new population which will replace the older population
       /// </summary>
       private List<Population> Crossover(List<Population> Winners)
       {
           //Iterate through each cell of cell, 50% chance to take number from Winner1, 50% chance to 
           //take number from Winner2 for any given cell
           //Output two new children
           List<Population> children = Winners; //TODO is this a good idea? 
           for (int column = 0; column < 9; column++)
           {
               for (int row = 0; row < 9; row++)
               {
                   //TODO Check with Debugger :)
                   //For every given cell, 50% to input winner1 value, 50% to input winner2 value 
                   //Creates 2 new children engineered from the parents
                   if (rand.Next(0, 2) == 0)
                   {
                       children[0].pop.setValue(column,row,Winners[0].pop.getValue(column,row));
                       children[1].pop.setValue(column, row, Winners[1].pop.getValue(column, row));
                   }
                   else
                   {
                       children[1].pop.setValue(column,row,Winners[1].pop.getValue(column,row));
                       children[0].pop.setValue(column, row, Winners[0].pop.getValue(column, row)); 
                   }
               }
           }

           return children;
       }
       
       /// <summary>
       /// Mutates the crossover created children using random mutation, returns to be added to the population 
       /// </summary>
       private List<Population> Mutate(ref List<Population> cross_children)
       {
           foreach (Population child in cross_children)
           {
               child.pop.setValue(rand.Next(0, 9), rand.Next(0, 9), rand.Next(1, 10)); //TODO Check
           }
           return cross_children;
       }

       private bool Evaluate(List<Population> cross_children)
       {
           if (cross_children[0].pop.getConstraints == 0)
           {
               SolutionPuzzle = cross_children[0].pop;
               return true;
           }
           else if (cross_children[1].pop.getConstraints == 0)
           {
               SolutionPuzzle = cross_children[1].pop;
               return true;
           }

           return false;
       }

       private void Replace(List<Population> random_pop, List<Population> adding_pop)
       {
          //sort by evaluation, remove the worst two pop, then add the newest two pop
       }

       //private void Reset(List<Population> cross_parents) => cross_parents.Clear();



    }
}