using System;
using System.Linq;

namespace AI_Sudoku_Solver
{
    class PuzzleChecker
    {
        bool isValidPuzzle = false;

        // Takes in a 9x9 puzzle array and checks if it's valid.
        public bool CheckPuzzle(int[,]_puzzle)
        {
            Console.WriteLine("Starting puzzle check");

            // Loop through each row and verify it adds to 45
            for (int row = 0; row < 9; row++)
            {
                int[] currentRow = new int[9];
                int rowSum = 0;
                for (int col = 0; col < 9; col++)
                {
                    rowSum += _puzzle[row, col];
                    currentRow[col] = _puzzle[row, col];
                }

                if (!CheckDuplicates(currentRow))
                    isValidPuzzle = false;
                else
                    isValidPuzzle = true;

                if (rowSum != 45)
                    isValidPuzzle = false;
                else
                    isValidPuzzle = true;
            }

            // Loop through each column and verify it adds to 45.
            for (int col = 0; col < 9; col++)
            {
                int[] currentColumn = new int[9];
                int columnSum = 0;
                for (int row = 0; row < 9; row++)
                {
                    columnSum += _puzzle[row, col];
                    currentColumn[row] = _puzzle[row, col];
                }

                if (!CheckDuplicates(currentColumn))
                    isValidPuzzle = false;
                else
                    isValidPuzzle = true;

                if (columnSum != 45)
                    isValidPuzzle = false;
                else
                    isValidPuzzle = true;
            }

            // Loops through each larger 3x3 cell, then through the smaller 3x3 cells to verify each sum is 45.
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int[] currentBox = new int[9];
                    int boxSum = 0;
                    for (int x = 0; x < 3; x++)
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            int currentRow = (3 * i) + x;
                            int currentColumn = (3 * j) + y;
                            boxSum += _puzzle[currentRow, currentColumn];
                            currentBox[y] = _puzzle[currentRow, currentColumn];
                        }
                    }

                    if (!CheckDuplicates(currentBox))
                        isValidPuzzle = false;
                    else
                        isValidPuzzle = true;

                    if (boxSum != 45)
                        isValidPuzzle = false;
                    else
                        isValidPuzzle = true;
                }
            }
            Console.WriteLine("Ending puzzle check");

            return isValidPuzzle;
        }

        bool CheckDuplicates(int[] _array)
        {
            int totalOnes = _array.Count(n => n == 1);
            int totalTwos = _array.Count(n => n == 2);
            int totalThrees = _array.Count(n => n == 3);
            int totalFours = _array.Count(n => n == 4);
            int totalFives = _array.Count(n => n == 5);
            int totalSixes = _array.Count(n => n == 6);
            int totalSevens = _array.Count(n => n == 7);
            int totalEights = _array.Count(n => n == 8);
            int totalNines = _array.Count(n => n == 9);

            if (totalOnes > 1 || totalTwos > 1 || totalThrees > 1 || totalFours > 1 || totalFives > 1 || totalSixes > 1 || totalSevens > 1 || totalEights > 1 || totalNines > 1)
                return false;
            else
                return true;
        }
    }
}