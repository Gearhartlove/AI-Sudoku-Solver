using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace AI_Sudoku_Solver
{
    public class GeneticAlgorithm : ISolver
    {
       //Generate Completely Random Board for Population
       private const int PopSize = 100;
       private const double SelectionPercentage = 0.11;
       private const double ProgramCounter = 15000;
       private const double MutationTicks = 2;
       private readonly Random rand;
       private SudokuPuzzle OriginalPuzzle;
       private SudokuPuzzle SolutionPuzzle;
       private List<Population> blankPopulation;
       private List<Population> randomPopulation;
       //for results / insights from each Sudoku puzzle file
       private StringBuilder traceBuilder = new StringBuilder();
       private int startWorstPopulation = 0;
       private int startBestPopulation = 0;
       private int endWorstPopulation = 0;
       private int endBestPopulation = 0;
       private double startPopMean = 0;
       private double startPopMode = 0;
       private double startPopMedian = 0;
       private double endPopMean = 0;
       private double endPopMode = 0;
       private double endPopMedian = 0;
        
       /// <summary>
       /// Constructor for instantiating variables
       /// </summary>
       public GeneticAlgorithm()
       {
           rand = new Random();
           blankPopulation = new List<Population>();
       }

        /// <summary>
        /// Attempts to solve the sudoku puzzle within {ProgramCounter} attempts. Runs and orchestrates the entirety of
        /// the program, including the Selection, Tournament, Crossover, Mutate, Evaluate, and Replace methods.
        /// </summary>
        /// <param name="puzzle"></param>
       public SudokuPuzzle solve(SudokuPuzzle puzzle)
        {
            ResultBookkeeping(); //Reset Variables for output
             
            //Solving the Program
            if (randomPopulation!= null) randomPopulation.Clear(); //reset my random population
            randomPopulation = GenerateRandomBoard(blankPopulation, puzzle);
            Insights(in randomPopulation, ref startPopMean, ref startPopMedian, ref startBestPopulation,
                ref startWorstPopulation); //calculate insights related to each population's ecosystem
            
            for (int loops = 0; loops < ProgramCounter; loops++)
            { 
               List<Population> crossoverParents = new List<Population>(); 
               //Select Two Winners of the tournament, from a random selection of the population
               crossoverParents.Add(Tournament(Selection(randomPopulation, SelectionPercentage)));
               crossoverParents.Add(Tournament(Selection(randomPopulation, SelectionPercentage)));
               var newPopMembers= Crossover(in crossoverParents, puzzle);
               Mutate(ref newPopMembers);
               Tracing(in crossoverParents, in newPopMembers);
               Evaluate(newPopMembers);
               Replace(ref randomPopulation, newPopMembers);
            }
            Insights(in randomPopulation, ref endPopMean, ref endPopMedian, ref endBestPopulation, 
                ref endWorstPopulation);
            //The Population list is always sorted from worst (index 0) to best (index Count-1)
            //Assigning the solution to the hightest fitness individual (least constraints violated)
            SolutionPuzzle = randomPopulation[randomPopulation.Count-1].pop; //TODO test this 
            if (SolutionPuzzle.constraintTest() == 0) return SolutionPuzzle;
            else return null;
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
                   if (child.pop.isLocked(x, y)) m -= 1; //if cell is a given start cell, mutate again 
                   else
                   {
                       child.pop.setValue(x, y, rand.Next(1, 10));
                   }
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
           //test the constraints of both children
           cross_children[0].constraints_violated = cross_children[0].pop.constraintTest();
           cross_children[1].constraints_violated = cross_children[1].pop.constraintTest();

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
           //sort the population
           random_pop.Sort((x,y)=>x.constraints_violated.CompareTo(y.constraints_violated)); 
           random_pop.Reverse(); //worst --> best sort
        }
        
        private void ResultBookkeeping()
        {
            startWorstPopulation = 0;
            startBestPopulation = 0;
            endWorstPopulation = 0;
            endBestPopulation = 0;
            startPopMean = 0;
            startPopMode = 0;
            startPopMedian = 0;
            endPopMean = 0;
            endPopMode = 0;
            endPopMedian = 0;
            traceBuilder.Clear();
        }
        /// <summary>
        /// Gather insights about the population, including best and worst population members, mean, and medien
        /// constraints violated population members.
        /// </summary>
        /// <param name="randPopulation"></param>
        /// <param name="mean"></param>
        /// <param name="median"></param>
        /// <param name="best"></param>
        /// <param name="worst"></param>
        private void Insights(in List<Population> randPopulation, ref double mean, ref double median, ref int best, 
            ref int worst)
        {
                //mean
                int sum = 0;
                foreach (Population p in randPopulation)
                {
                    sum += p.constraints_violated;
                }
                mean = (sum / (double)PopSize);
                //median
                median = randPopulation[randomPopulation.Count / 2].constraints_violated;
                //worst
                worst = randPopulation[0].constraints_violated;
                //best
                best = randPopulation[randPopulation.Count - 1].constraints_violated;
        }

        /// <summary>
        /// Used for writing to the output file in bin/Debug/output/genetic \local \search  to track the mutation
        /// and crossover children
        /// </summary>
        /// <param name="parents"></param>
        /// <param name="children"></param>
        public void Tracing(in List<Population> parents, in List<Population> children)
        {
            log("Parents:");
            //tracing parents
            foreach (Population p in parents)
            {
                log(p.pop.ToString());
                log("");
            }
            log(""); 
            log("Children:");
            //tracing children
            foreach (Population p in children)
            {
                log(p.pop.ToString());
                log("");
            }

            log("");
            log("-------------------------------------------------------------------------------");
        }
        
        /// <summary>
        /// Supportive method to make writing traces easier
        /// </summary>
        /// <returns></returns>
        public string traceWriter()
        {
            return traceBuilder.ToString();
        }

        /// <summary>
        /// Supportive to make logging traces easier (recording important user strings)
        /// </summary>
        /// <param name="mesesage"></param>
        public void log(string mesesage)
        {
            traceBuilder.AppendLine(mesesage);
        }
        
        /// <summary>
        /// Output to terminal when program is run
        /// </summary>
        /// <returns></returns>
        public string result()
        {
            return "\n    \t(Constraints violated by the population)\n    \tSTART worst: " + startWorstPopulation
                + " best: " + startBestPopulation + " mean: " + startPopMean + " median: " + startPopMedian +
                "\n    \tEND   worst: " + endWorstPopulation + " best: " + endBestPopulation + " mean: " + endPopMean + 
                " median: " + endPopMedian;


        }

        /// <summary>
        /// Name of the algorithm
        /// </summary>
        /// <returns></returns>
        public string solverName() {
            return "Genetic Local Search";
        }
    }
}