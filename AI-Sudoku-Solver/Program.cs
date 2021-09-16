using System;
using Backtracking;

namespace AI_Sudoku_Solver {
	internal class Program {
		public static void Main(string[] args) {
			var t = new SudokuPuzzle("Easy-P1.csv");

			foreach (var item in t.getSquare(2, 2)) {
				Console.WriteLine(item);
			}
//			var solver = new SimpleBacktracking();
//			Console.Out.WriteLine(solver.solve(t));
//			Console.Out.WriteLine(solver.getElapsedTimeMili());
			
		}
	}
}