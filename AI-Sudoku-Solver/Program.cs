using System;
using Backtracking;

namespace AI_Sudoku_Solver {
	internal class Program {
		public static void Main(string[] args) {
			var t = new SudokuPuzzle("Easy-P1.csv");

			SimulatedAnnealingAlgorithm SAA = new SimulatedAnnealingAlgorithm();
			SAA.LocalSearchSimulatedAnnealing(t);

			//var sovler = new SimpleBacktracking();
			
			//Console.WriteLine(sovler.solve(t));
			//Console.WriteLine(sovler.getElapsedTimeMili());

//			var solver = new SimpleBacktracking();
//			Console.Out.WriteLine(solver.solve(t));
//			Console.Out.WriteLine(solver.getElapsedTimeMili());

		}
	}
}