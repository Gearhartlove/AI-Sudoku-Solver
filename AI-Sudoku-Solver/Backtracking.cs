using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using AI_Sudoku_Solver;

namespace Backtracking {
	public abstract class BacktrackingSolver : ISolver {
		
		private Stopwatch stopwatch = new Stopwatch();
		private StringBuilder traceBuilder = new StringBuilder();
		
		protected long backtracks = 0;
		protected long nodesExplored = 0;
		protected SortedList<CellChangeInfo, Node<CellChangeInfo>> possibleNodes;

		public bool shouldTrace = true;

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
			var r = recurse(new Node<CellChangeInfo>(new CellChangeInfo(init)), 
				new SortedList<CellChangeInfo, Node<CellChangeInfo>>(getComparer()));
			stopwatch.Stop();
			return r;
		}

		public long getElapsedTimeMilli() {
			return stopwatch.ElapsedMilliseconds;
		}
		
		private SudokuPuzzle recurse(Node<CellChangeInfo> currentNode, SortedList<CellChangeInfo, Node<CellChangeInfo>> list) {
			nodesExplored++;
			var puzzle = currentNode.getValue().delta;
			if (checkGoalState(puzzle)) return puzzle;

			log("Node Selected (" + currentNode.getPath() + ")");
			
			var possibleChildren = discover(currentNode);
			foreach (var child in possibleChildren) {
				list.Add(child.getValue(), child);
			}
			// if (possibleChildren == null || possibleChildren.Count == 0) {
			// 	log("");
			// 	log("    No Children Discovered");
			// 	log("       !!BACKTRACKING!!");
			// 	log("");
			// 	backtracks++;
			// 	return null;
			// }

			foreach (var child in list) {
				if (checkConstraints(puzzle, child.Key)) {
					log("  Possible Move " + child + " | Constraint Test: PASSED");
					var modifiedPuzzle = child.Value.getValue().delta;
					
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

		protected abstract List<Node<CellChangeInfo>> discover(Node<CellChangeInfo> parent);

		protected abstract bool checkConstraints(SudokuPuzzle puzzle, CellChangeInfo description);

		protected abstract bool checkGoalState(SudokuPuzzle puzzle);

		protected abstract IComparable<CellChangeInfo> getComparer();
		public string result() {
			return "ms: " + stopwatch.ElapsedMilliseconds + " | backtracks: " + backtracks + " | nodes visit: " +
			       nodesExplored;
		}

		public abstract string solverName();

		public string traceWriter() {
			return traceBuilder.ToString();
		}

		protected void log(string message) {
			if(shouldTrace) traceBuilder.AppendLine(message);
		}
	}

	public class SimpleBacktracking : BacktrackingSolver {
		private int lastX = 0;
		private int lastY = 0;

		private int counter = 0;
		
		private bool goal = false;
		
		protected override List<Node<CellChangeInfo>> discover(Node<CellChangeInfo> parent) {
			var puzzle = parent.getValue().delta;

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

	public class ArcBacktracking : BacktrackingSolver {

		private int[,,] domainsRemaining = new int[9, 9, 9];
		
		protected override List<NodeChangeDescription> discover(Node<SudokuPuzzle> parent) {
			throw new NotImplementedException();
		}

		protected override bool checkConstraints(SudokuPuzzle puzzle, NodeChangeDescription description) {
			throw new NotImplementedException();
		}

		protected override bool checkGoalState(SudokuPuzzle puzzle) {
			throw new NotImplementedException();
		}

		public override string solverName() {
			return "Arc Consistancy";
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

	public class CellChangeInfo {
		public int x, y, value;
		public SudokuPuzzle delta;
		public int[] domain;

		public CellChangeInfo(SudokuPuzzle puzzle) {
			delta = puzzle;
		}
		public CellChangeInfo(SudokuPuzzle puzzle, int x, int y, int value) {
			this.x = x;
			this.y = y;
			this.value = value;

			delta = puzzle.copy();
			delta.setValue(x, y, value);
			
			this.domain = delta.getDomain(x, y);
		}
	}
}


