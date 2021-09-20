using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Text;
using AI_Sudoku_Solver;

namespace Backtracking {
	public abstract class BacktrackingSolver : ISolver {
		
		//These are variables used only for logging
		private Stopwatch stopwatch = new Stopwatch();
		protected long backtracks = 0;
		protected long nodesExplored = 0;
		private StringBuilder traceBuilder = new StringBuilder();
		
		//This is the method that is called that sets up the problem and runs the
		//recursive method.
		public SudokuPuzzle solve(SudokuPuzzle init) {
			this.init(init);
			backtracks = 0;
			nodesExplored = 0;
			traceBuilder.Clear();
			
			log("       Start Algorithm       ");
			log("-----------------------------");
			log("");
			
			log("Initial State:");
			log(init.ToString());
			log("");
			
			stopwatch.Restart();
			var r = recurse(new Node<SudokuPuzzle>(init));
			stopwatch.Stop();
			return r;
		}

		//Returns the time in milliseconds of last run
		public long getElapsedTimeMili() {
			return stopwatch.ElapsedMilliseconds;
		}
		
		//This is the method that recurses and solves the puzzle
		private SudokuPuzzle recurse(Node<SudokuPuzzle> currentNode) {
			nodesExplored++;
			var puzzle = currentNode.getValue();
			if (checkGoalState(puzzle)) return puzzle;

			log("Node Selected (" + currentNode.getPath() + ")");
			
			var possibleChildren = discover(currentNode);
			if (possibleChildren == null || possibleChildren.Count == 0) {
				log("");
				log("    No Children Discovered");
				log("       !!BACKTRACKING!!");
				log("");
				backtracks++;
				return null;
			}

			foreach (var child in possibleChildren) {
				if (checkConstraints(puzzle, child)) {
					log("  Possible Move " + child + " | Constraint Test: PASSED");
					var modifiedPuzzle = puzzle.copy();
					modifiedPuzzle.setValue(child.x, child.y, child.value);
					
					log("            |            ");
					log("            |            ");
					log("            V            ");
					log(modifiedPuzzle.ToString());
					log("");
					
					var next = currentNode.createChild(modifiedPuzzle);
					var value  = recurse(next);
					if (value != null) return value;
				} else {
					log("  Possible Move " + child + " | Constraint Test: FAILED");
				}
			}
			
			log("");
			log("  No Valid Children Discovered");
			log("       !!BACKTRACKING!!");
			log("");
			backtracks++;
			return null;
		}

		//This method is called when the tree is looking for possible children in the decision tree.
		protected abstract List<NodeChangeDescription> discover(Node<SudokuPuzzle> parent);

		//This method will check the constraints of a potential move
		protected abstract bool checkConstraints(SudokuPuzzle puzzle, NodeChangeDescription description);

		//This will check if any given state is the goal state
		protected abstract bool checkGoalState(SudokuPuzzle puzzle);
		
		//This is the result of the algorithm used in the trial
		public string result() {
			return "ms: " + stopwatch.ElapsedMilliseconds + " | backtracks: " + backtracks + " | nodes visit: " +
			       nodesExplored;
		}

		//Returns the name of the solver for the trial
		public abstract string solverName();

		//Returns the trace for debuging
		public string traceWriter() {
			return traceBuilder.ToString();
		}

		//Logs debug text too trace
		protected void log(string message) {
			traceBuilder.AppendLine(message);
		}
		
		//Used by subclasses to setup stuff
		protected virtual void init(SudokuPuzzle start) {}
	}

	//The Simple Backtracking Solver
	public class SimpleBacktracking : BacktrackingSolver {
		private int lastX = 0;
		private int lastY = 0;

		private int counter = 0;
		
		private bool goal = false;
		
		protected override List<NodeChangeDescription> discover(Node<SudokuPuzzle> parent) {
			var puzzle = parent.getValue();

			int xt = 0, yt = 0;
			
			for (int y = 0; y < 9; y++) {
				for (int x = 0; x < 9; x++) {
					if (!puzzle.isLocked(x, y) && puzzle.getValue(x, y) == -1) {
						xt = x;
						yt = y;
						break;
					}
				}
			}
			
			
			List<NodeChangeDescription> list = new List<NodeChangeDescription>();
			for (int i = 1; i < 10; i++) {
				list.Add(new NodeChangeDescription(xt, yt, i));
			}

			return list;
		}

		protected override bool checkConstraints(SudokuPuzzle puzzle, NodeChangeDescription description) {
			return puzzle.isLegalMove(description.x, description.y, description.value);
		}

		protected override bool checkGoalState(SudokuPuzzle puzzle) {
			return puzzle.getEmptyCellCount() == 0;
		}

		public override string solverName() {
			return "Simple Backtracking";
		}
	}

	//Backtracking with Forward looking Solver
	public class LFBacktracking : BacktrackingSolver {
		protected override List<NodeChangeDescription> discover(Node<SudokuPuzzle> parent) {
			var puzzle = parent.getValue();
			
			int xt = 0, yt = 0;
			
			for (int y = 0; y < 9; y++) {
				for (int x = 0; x < 9; x++) {
					if (!puzzle.isLocked(x, y) && puzzle.getValue(x, y) == -1) {
						xt = x;
						yt = y;
						break;
					}
				}
			}
			
			List<NodeChangeDescription> list = new List<NodeChangeDescription>();
			for (int i = 1; i < 10; i++) {
				if(puzzle.isLegalMove(xt, yt, i)) list.Add(new NodeChangeDescription(xt, yt, i));
			}

			return list;
		}

		protected override bool checkConstraints(SudokuPuzzle puzzle, NodeChangeDescription description) {
			return true;
		}

		protected override bool checkGoalState(SudokuPuzzle puzzle) {
			return puzzle.getEmptyCellCount() == 0;
		}

		public override string solverName() {
			return "Look Forward Backtracking";
		}
	}

	//Backtracking with Arc Consistancy Solver
	public class ArcBacktracking : BacktrackingSolver {

		private short[,] domains = new short[9,9];

		protected override void init(SudokuPuzzle start) {
			for (int i = 0; i < 9; i++) {
				for (int j = 0; j < 9; j++) {
					if (start.isLocked(i, j)) {
						domains[i, j] = -1;
					} else {
						domains[i, j] = 0b111111111;
					}
				}	
			}
		}

		protected override List<NodeChangeDescription> discover(Node<SudokuPuzzle> parent) {
			var puzzle = parent.getValue();
			
			int xt = 0, yt = 0;
			
			for (int y = 0; y < 9; y++) {
				for (int x = 0; x < 9; x++) {
					if (!puzzle.isLocked(x, y) && puzzle.getValue(x, y) == -1) {
						xt = x;
						yt = y;
						break;
					}
				}
			}

			List<NodeChangeDescription> list = new List<NodeChangeDescription>();
			foreach (var value in getValues(domains[xt, yt])) {
				list.Add(new NodeChangeDescription(xt, yt, value));
			}

			return list;
		}

		protected override bool checkConstraints(SudokuPuzzle puzzle, NodeChangeDescription description) {
			if (puzzle.isLegalMove(description.x, description.y, description.value)) {
				var row = Util.getRow(domains, description.y);
				var col = Util.getCol(domains, description.x);
				var square = Util.getSquareXY(domains, description.x, description.y);

				short mask = (short) ((0b1 << (description.value - 1)) ^ 0b111111111);
			
				for (int i = 0; i < 9; i++) {
					row[i] &= mask;
					col[i] &= mask;
					square[i] &= mask;

					if (row[i] == 0 || col[i] == 0 || square[i] == 0) {
						return false;
					} 
				}

				return domains[description.x, description.y] != 0;
			}

			return false;
		}

		protected override bool checkGoalState(SudokuPuzzle puzzle) {
			return puzzle.getEmptyCellCount() == 0;
		}

		public override string solverName() {
			return "MAC Backtracking";
		}

		private bool isSingleton(short value) {
			return value == 0b100000000 ||
			       value == 0b010000000 ||
			       value == 0b001000000 ||
			       value == 0b000100000 ||
			       value == 0b000010000 ||
			       value == 0b000001000 ||
			       value == 0b000000100 ||
			       value == 0b000000010 ||
			       value == 0b000000001;
		}

		private int[] getValues(short value) {
			List<int> list = new List<int>();
			
			if ((value & 0b100000000) == 0b100000000) list.Add(9);
			if ((value & 0b010000000) == 0b010000000) list.Add(8);
			if ((value & 0b001000000) == 0b001000000) list.Add(7);
			if ((value & 0b000100000) == 0b000100000) list.Add(6);
			if ((value & 0b000010000) == 0b000010000) list.Add(5);
			if ((value & 0b000001000) == 0b000001000) list.Add(4);
			if ((value & 0b000000100) == 0b000000100) list.Add(3);
			if ((value & 0b000000010) == 0b000000010) list.Add(2);
			if ((value & 0b000000001) == 0b000000001) list.Add(1);

			return list.ToArray();
		}
	}

	public class NodeChangeDescription {
		public int x, y, value;

		public NodeChangeDescription(int x, int y, int value) {
			this.x = x;
			this.y = y;
			this.value = value;
		}

		public override string ToString() {
			return "(Cell: " + x + ", " + y + " | Value:" + value + ")";
		}
	}
}


