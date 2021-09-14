namespace AI_Sudoku_Solver
{
    public class GeneticAlgorithm
    {
       //Generate Completely Random Board for Population
       private int Temperature = 0;
       private const int pop_size = 20;
       private SudokuPuzzle[] Population;
       
       //tournament selection
       //rinse and repeat until I find an ideal board

       public GeneticAlgorithm()
       {
           Population = new SudokuPuzzle[pop_size];
           //GenerateRandomPop(); //can be used by both Local Search Algorithms
           //   -> work with the SudokuPuzzle Class
           //HillClimb(); //can be used by both Local Search Algorithms //Annealing will have a more tricks
       }

       
       
       /// <summary>
       /// Mutates Population by using the X operation and Y Selection. (Still need to decide on these)
       /// </summary>
       private void MutatePop()
       {
           
       }

       /// <summary>
       /// Survival of the fittest, based on initial selection of population. This is chosen using X. (Still neeed to
       /// decide)
       /// </summary>
       private void TournamentSelection()
       {
           
       }
       
       /// <summary>
       /// Instantiate GeneticAlgorithm object (calls the constructor) and have a while loop which runs until the
       /// algorithm is solved. General Process Flow: Mutate -> Tournament Selection -> ... 
       /// </summary>
       public static void SolvePuzzle() //files for the board as a parameter?
       {
          //instantiate genetic algorithm object
          //while loop of 
       }

    }
}