using System;
using System.IO;
using System.Threading;

namespace AI_Sudoku_Solver {
    public class Trial {
        private PuzzleChecker checker = new PuzzleChecker();
        private string resultsOut = "";
        
        public void runTrail(string folderIn, string folderOut, params ISolver[] solvers) {
            string[] dir = Directory.GetFiles(folderIn);
            Directory.CreateDirectory(folderOut);
            
            foreach (var solver in solvers) {
                string path = folderOut + "/" + stringToFilename(solver.solverName().ToLower());
                Directory.CreateDirectory(path);
            }

            resultsOut = DateTime.Now.ToString() + "\n\n";
            
            foreach (var file in dir) {
                var puzzle = new SudokuPuzzle(file);
                log(Path.GetFileName(file) + ":");
                foreach (var solver in solvers) {
                    var solvedPuzzle = solver.solve(puzzle);
                    //var checkedResult = checker.CheckPuzzle(solvedPuzzle.getCells());
                    string path = folderOut + "/" +
                                  stringToFilename(solver.solverName().ToLower()) + "/"
                                  + Path.GetFileNameWithoutExtension(file);

                    Directory.CreateDirectory(path);

                    var solvedString = solvedPuzzle != null ? "SOLVED" : "UNSOLVED";
                    log("    (" + solver.solverName() + ") - " + solvedString + " | " + solver.result());
                    
                    File.WriteAllText(path + "/traces.txt", solver.traceWriter());
                    if(solvedPuzzle != null) File.WriteAllText(path + "/solution.txt", solvedPuzzle.toOutputString());
                }
                log("");
            }
            
            File.WriteAllText(folderOut + "/results.txt", resultsOut);
        }
        
        private string stringToFilename(string s) {
            string o = s;
            foreach (char c in System.IO.Path.GetInvalidFileNameChars()) {
                o = o.Replace(c, '_');
            }
            return o;
        }

        private void log(string message) {
            resultsOut += message + "\n";
            Console.WriteLine(message);
        }
    }
}