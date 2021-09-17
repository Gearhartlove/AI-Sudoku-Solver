using System;
using System.Runtime.InteropServices;

namespace AI_Sudoku_Solver
{
    /// <summary>
    /// Goal of class is to not solve, but to produce a better board state than the initial one(s) provided 
    /// using a simple hill climbing algorithm. temperature++ for every constraint voilated.
    /// The constraint a cell can violate are: 
    /// -> one number per row 
    /// -> run number per column
    /// -> one number per Sudoku 3x3 block 
    /// </summary>
    public class BoardTemperature
    {
        
        // public static float GetBoardTemp(in SudokuPuzzle board)
        // {
        //     //iterate over the board and explain how many constraints have been violated
        //     temperature = CheckCellViolations(board, cell); 
        //     //debug
        //     Console.WriteLine(temperature);
        //     return temperature;
        // }
        //
        // private void CheckCellViolations(in SudokuPuzzle board)
        // {
        //     checkRow(boards);
        //     checkColumn(boards);
        //     checkBox(boards);
        // }
        
        private void checkRow(in SudokuPuzzle[] board)
        {
             
        }

        private void checkColumn()
        {
            
        }

        //Check 3x3 Sudoku box
        private void checkBox()
        {
            
        }
    }
}