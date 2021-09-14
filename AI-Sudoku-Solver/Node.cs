using System;
using System.Collections.Generic;
namespace AI_Sudoku_Solver
{
    public class Node
    {
        public Node parent;
        public List<Node> next_nodes = new List<Node>();
        private int value = 0; //Sudoku Value // Do we need this?
    }
}