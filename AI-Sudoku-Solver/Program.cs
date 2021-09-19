using System;
using Backtracking;

namespace AI_Sudoku_Solver {
	internal class Program {
		public static void Main(string[] args) {
			Trial trial = new Trial();
			trial.runTrail("puzzles", "output", new SimpleBacktracking());
		}
	}
}