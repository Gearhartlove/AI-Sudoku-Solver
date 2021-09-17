namespace AI_Sudoku_Solver
{
    public class Population
    {
        public Population(string file)
        {
            pop = new SudokuPuzzle(file);
        }
        
        public SudokuPuzzle pop;
        public double tourny_percentage = 0f;
        public int constraints_violated = 0;
    }
}