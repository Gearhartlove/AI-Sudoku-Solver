using System;

namespace AI_Sudoku_Solver {
	internal class Program {
		public static void Main(string[] args) {
			var t = new SudokuPuzzle("Easy-P1.csv");
			Console.Out.WriteLine(t.isLegalSquare(0, 0, 1));
		}
	}
}