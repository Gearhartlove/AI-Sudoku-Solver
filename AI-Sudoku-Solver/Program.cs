using System;

namespace AI_Sudoku_Solver {
	internal class Program {
		public static void Main(string[] args)
		{
			GeneticAlgorithm ga = new GeneticAlgorithm();
			SudokuPuzzle puzzle = new SudokuPuzzle("Evil-P3.csv");
			ga.Solve(puzzle);
		}
	}
}