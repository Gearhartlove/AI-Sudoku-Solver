namespace AI_Sudoku_Solver
{
    public interface ISolver
    {

        SudokuPuzzle solve(SudokuPuzzle puzzle);


        string trace();
    }
}