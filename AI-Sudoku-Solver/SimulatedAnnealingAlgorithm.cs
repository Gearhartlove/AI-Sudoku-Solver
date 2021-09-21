using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AI_Sudoku_Solver
{
    class SimulatedAnnealingAlgorithm : ISolver
    {
        Stopwatch stopwatch = new Stopwatch();
        StringBuilder traceBuilder = new StringBuilder();
        Random rand = new Random();
        int maximumRuns = 2500;
        double temperature = 1000;
        double coolingMultiplier = 0.999;
        int scoreFinal;
        int initialScore;

        public SudokuPuzzle LocalSearchSimulatedAnnealing(SudokuPuzzle _puzzle)
        {
            // Loops through puzzle and fills with unique values
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int x = 3*i; x < 3*i+3; x++)
                    {
                        for (int y = 3*j; y < 3*j+3; y++)
                        {
                            if (_puzzle.getValue(x, y) == -1)
                                _puzzle.setValue(x, y, GetUnusedValue(_puzzle, x, y));
                        }
                    }
                }
            }
            initialScore = Score(_puzzle);
            Swap(_puzzle);
            return _puzzle;
        }

        // Returns a value within a selected 3x3 box that has NOT been used yet
        int GetUnusedValue(SudokuPuzzle _puzzle, int _x, int _y)
        {
            int x = 0;
            int y = 0;

            if (_x <= 2)
                x = 0;
            else if (_x <= 5 && _x > 2)
                x = 1;
            else if (_x > 5)
                x = 2;

            if (_y <= 2)
                y = 0;
            else if (_y <= 5 && _y > 2)
                y = 1;
            else if (_y > 5)
                y = 2;

            int[] currentBox = _puzzle.getSquare(x, y);

            int totalOnes = currentBox.Count(n => n == 1);
            int totalTwos = currentBox.Count(n => n == 2);
            int totalThrees = currentBox.Count(n => n == 3);
            int totalFours = currentBox.Count(n => n == 4);
            int totalFives = currentBox.Count(n => n == 5);
            int totalSixes = currentBox.Count(n => n == 6);
            int totalSevens = currentBox.Count(n => n == 7);
            int totalEights = currentBox.Count(n => n == 8);
            int totalNines = currentBox.Count(n => n == 9);

            // If value has not been used, return it
            if (totalOnes == 0)
                return 1;
            else if (totalTwos == 0)
                return 2;
            else if (totalThrees == 0)
                return 3;
            else if (totalFours == 0)
                return 4;
            else if (totalFives == 0)
                return 5;
            else if (totalSixes == 0)
                return 6;
            else if (totalSevens == 0)
                return 7;
            else if (totalEights == 0)
                return 8;
            else
                return 9;
        }

        // Swaps two randomly chosen values within a randomly chosen 3x3 box
        void Swap(SudokuPuzzle _puzzle)
        {
            for (int i = 0; i < maximumRuns; i++)
            {
                //Tracing information  
                Log("");
                Log("Run Number: " + i);
                Log(_puzzle.ToString());
                Log("Current Score: " + scoreFinal);
                Log("-----------------------------------------------------------");
                
                temperature = (temperature * coolingMultiplier);
                int x = rand.Next(0, 3);
                int y = rand.Next(0, 3);
                SudokuPuzzle currentPuzzle = _puzzle.copy();
                FindValidSwap(_puzzle, x, y);

                double deltaScore = Score(currentPuzzle) - Score(_puzzle);
                double acceptScore = Math.Exp(-deltaScore / temperature) - 1 - rand.NextDouble();

                if (acceptScore > 0)
                    _puzzle = currentPuzzle.copy();

                Score(_puzzle);
            }
        }

        // Verify that a swap is allowed to be made
        void FindValidSwap(SudokuPuzzle _puzzle, int _x, int _y)
        {
            bool isValid = false;

            int x1 = GenerateRandomX(_x);
            int y1 = GenerateRandomY(_y);

            int x2 = GenerateRandomX(_x);
            int y2 = GenerateRandomY(_y);

            if (_puzzle.isLocked(x1, y1) || _puzzle.isLocked(x2, y2))
                isValid = false;
            else
                isValid = true;

            if (isValid)
            {
                int value1 = _puzzle.getValue(x1, y1);
                int value2 = _puzzle.getValue(x2, y2);

                _puzzle.setValue(x1, y1, value2);
                _puzzle.setValue(x2, y2, value1);
                Log("Switched: (" + x1 + ", " + y1 + ") " + value1 + " with (" + x2 + ", " + y2 + ") " + value2);
            }

            if (!isValid)
                FindValidSwap(_puzzle, _x, _y);
        }

        // Returns a random number from 0 to 9
        int GenerateRandomX(int _x)
        {
            int x = rand.Next(0, 3);

            if (_x == 1)
                x += 3;
            else if (_x == 2)
                x += 6;

            return x;
        }

        // Returns a random number from 0 to 9
        int GenerateRandomY(int _y)
        {
            int y = rand.Next(0, 3);

            if (_y == 1)
                y += 3;
            else if (_y == 2)
                y += 6;

            return y;
        }

        // Scores puzzle based on constraints violated
        int Score(SudokuPuzzle _puzzle)
        {
            int score = _puzzle.constraintTest();
            scoreFinal = score;
            return score;
        }

        // Starts method to solve Sudoku puzzle
        public SudokuPuzzle solve(SudokuPuzzle _puzzle)
        {
            traceBuilder.Clear();
            stopwatch.Restart();
            SudokuPuzzle solution = LocalSearchSimulatedAnnealing(_puzzle);
            stopwatch.Stop();
            if (scoreFinal == 0)
                return solution;

            return null;
        }

        // Finds the time elapsed while solving
        public long GetElapsedTimeMili()
        {
            return stopwatch.ElapsedMilliseconds;
        }

        protected void Log(string message)
        {
            traceBuilder.AppendLine(message);
        }

        public string traceWriter()
        {
            return traceBuilder.ToString();
        }

        // Results sent to the console window
        public string result()
        {
            return "ms: " + stopwatch.ElapsedMilliseconds + " | Starting Puzzle Violation Score " + initialScore + " Final Puzzle Violation Score: " + scoreFinal;
        }

        public string solverName()
        {
            return "Simmulated Annealing Local Search";
        }
    }
}