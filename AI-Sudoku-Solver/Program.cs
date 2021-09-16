using System;
using Backtracking;

namespace AI_Sudoku_Solver {
	internal class Program {
		public static void Main(string[] args) {
			var t = new SudokuPuzzle("Easy-P1.csv");

			t.setValue(0, 0, 5);
			t.setValue(1, 0, 5);
			t.setValue(1, 1, 4);
			
			Console.WriteLine(t);
			Console.WriteLine(t.constraintTestSquare(0, 0));
			
//			var solver = new SimpleBacktracking();
//			Console.Out.WriteLine(solver.solve(t));
//			Console.Out.WriteLine(solver.getElapsedTimeMili());

		}
	}
}