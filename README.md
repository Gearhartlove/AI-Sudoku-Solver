# AI-Sudoku-Solver
Solving Sudoku puzzles with backtracking and local search algorithms <br />
Contributers: [Gearhartlove](https://github.com/Gearhartlove) | [LordThesus](https://github.com/LordThesus) 
| [MrVintage710](https://github.com/MrVintage710)

* **Simple Backtracking**
* Backtrackign with **forward checking**
* Backtracking using **arc consistency**
* **Genetic Algorithm** with a tournament and penalty function
* **Simulated Annealing** using a minimum conflict heuristic

## Input
9x9 grid filled with number or ? (empty space) as a .txt file <br /><br />
?,?,8,?,5,6,?,?,?<br />
7,?,4,?,?,?,6,1,9<br />
?,?,?,?,?,?,8,5,?<br />
6,?,7,?,2,9,5,?,?<br />
?,?,9,?,6,?,1,?,?<br />
?,?,2,3,1,?,9,?,4<br />
?,3,5,?,?,?,?,?,?<br />
4,2,1,?,?,?,3,?,6<br />
?,?,?,8,3,?,4,?,?<br />

The Input for the project is stored at: AI-Sudoku-Solver/AI-Sudoku-Solver/puzzles <br />
There are a total of 19 puzzle files similar to the example above. The program runs 
each algorithm for each puzzle, one at a time. 

## Output
```
Hard-P4.csv:
    (Simple Backtracking) - SOLVED (Checker) - PASSED | ms: 2 | backtracks: 44 | nodes visit: 98
    (Look Forward Backtracking) - SOLVED (Checker) - PASSED | ms: 5 | backtracks: 44 | nodes visit: 98
    (MAC Backtracking) - SOLVED (Checker) - PASSED | ms: 312 | backtracks: 9534 | nodes visit: 9588
    (Genetic Local Search) - UNSOLVED (Checker) - FAILED | 
        (Constraints violated by the population)
        START worst: 161 best: 123 mean: 141.48 median: 142
        END   worst: 56 best: 24 mean: 24.65 median: 24
    (Simmulated Annealing Local Search) - UNSOLVED (Checker) - FAILED | ms: 122
Starting Puzzle Violation Score: 87 | Final Puzzle Violation Score: 22
```


