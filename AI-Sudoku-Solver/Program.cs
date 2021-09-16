﻿using System;
using Backtracking;

namespace AI_Sudoku_Solver {
	internal class Program {
		public static void Main(string[] args) {
			var t = new SudokuPuzzle("Easy-P3.csv");
			
			var solver = new SimpleBacktracking();
			Console.Out.WriteLine(solver.solve(t));
			Console.Out.WriteLine(solver.getElapsedTimeMili());
			
//			Console.Out.WriteLine(t);
//			Console.Out.WriteLine(t.isLegalMove(3, 0, 3));

//			var h = t.copy();
//			Console.Out.WriteLine(h);


			//SimpleBacktracking solver = new SimpleBacktracking();
			//solver.solve(t);
		}
	}
}