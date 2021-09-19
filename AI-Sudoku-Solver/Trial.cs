using System;
using System.IO;

namespace AI_Sudoku_Solver {
    public class Trial {
        public void runTrail(string folderIn, string folderOut, params ISolver[] solvers) {
            string[] dir = Directory.GetFiles(folderIn);

            foreach (var file in dir) {
                var puzzle = new SudokuPuzzle(file);
                foreach (var solver in solvers) {
                    Directory.CreateDirectory("solutions");
                    
                }
            }
        }
    }
}