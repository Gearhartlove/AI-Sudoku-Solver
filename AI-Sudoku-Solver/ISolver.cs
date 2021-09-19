using System.IO;

namespace AI_Sudoku_Solver
{
    public interface ISolver {
        SudokuPuzzle solve(SudokuPuzzle puzzle);

        string traceWriter();

        string result();

        string solverName();
    }
}