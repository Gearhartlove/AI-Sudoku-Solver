using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Text;
using AI_Sudoku_Solver;

namespace Backtracking {
	public abstract class BacktrackingSolver : ISolver {
		
		
		private Stopwatch stopwatch = new Stopwatch();
		protected long backtracks = 0;
		protected long nodesExplored = 0;
		private StringBuilder traceBuilder = new StringBuilder();
		
		public SudokuPuzzle solve(SudokuPuzzle init) {
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

		public long getElapsedTimeMili() {
			return stopwatch.ElapsedMilliseconds;
		}
		
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
					
//					log(puzzle.ToString());
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

		protected abstract List<NodeChangeDescription> discover(Node<SudokuPuzzle> parent);

		protected abstract bool checkConstraints(SudokuPuzzle puzzle, NodeChangeDescription description);

		protected abstract bool checkGoalState(SudokuPuzzle puzzle);
		public string result() {
			return "ms: " + stopwatch.ElapsedMilliseconds + " | backtracks: " + backtracks + " | nodes visit: " +
			       nodesExplored;
		}

		public abstract string solverName();

		public string traceWriter() {
			return traceBuilder.ToString();
		}

		protected void log(string message) {
			traceBuilder.AppendLine(message);
		}
	}

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

	public class ArcBacktracking {
		
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


