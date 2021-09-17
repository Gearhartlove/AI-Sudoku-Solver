using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace Backtracking {
	// public abstract class BacktrackingSolver {
	// 	
	// 	private Stopwatch stopwatch = new Stopwatch();
	// 	public SudokuPuzzle solve(SudokuPuzzle init) {
	// 		stopwatch.Restart();
	// 		var r = recurse(new Node<SudokuPuzzle>(init));
	// 		stopwatch.Stop();
	// 		return r;
	// 	}
	//
	//
	// 	public long getElapsedTimeMili() {
	// 		return stopwatch.ElapsedMilliseconds;
	// 	}
	// 	
	// 	private SudokuPuzzle recurse(Node<SudokuPuzzle> currentNode) {
	// 		//Console.Out.WriteLine(currentNode.getValue());
	// 		
	// 		var puzzle = currentNode.getValue();
	// 		if (checkGoalState(puzzle)) return puzzle;
	//
	// 		var possibleChildren = discover(currentNode);
	// 		if (possibleChildren == null || possibleChildren.Count == 0) return null;
	//
	// 		foreach (var child in possibleChildren) {
	// 			if (checkConstraints(puzzle, child)) {
	// 				var modifiedPuzzle = puzzle.copy();
	// 				modifiedPuzzle.setValue(child.x, child.y, child.value);
	// 				var next = currentNode.createChild(modifiedPuzzle);
	// 				var value  = recurse(next);
	// 				if (value != null) return value;
	// 			}
	// 		}
	//
	// 		return null;
	// 	}
	//
	// 	protected abstract List<NodeChangeDescription> discover(Node<SudokuPuzzle> parent);
	//
	// 	protected abstract bool checkConstraints(SudokuPuzzle puzzle, NodeChangeDescription description);
	//
	// 	protected abstract bool checkGoalState(SudokuPuzzle puzzle);
	// }
	//
	// public class SimpleBacktracking : BacktrackingSolver {
	// 	private int lastX = 0;
	// 	private int lastY = 0;
	//
	// 	private int counter = 0;
	// 	
	// 	private bool goal = false;
	// 	
	// 	protected override List<NodeChangeDescription> discover(Node<SudokuPuzzle> parent) {
	// 		var puzzle = parent.getValue();
	//
	// 		int xt = 0, yt = 0;
	// 		
	// 		for (int y = 0; y < 9; y++) {
	// 			for (int x = 0; x < 9; x++) {
	// 				if (!puzzle.isLocked(x, y) && puzzle.getValue(x, y) == -1) {
	// 					xt = x;
	// 					yt = y;
	// 					break;
	// 				}
	// 			}
	// 		}
	// 		
	// 		List<NodeChangeDescription> list = new List<NodeChangeDescription>();
	// 		for (int i = 1; i < 10; i++) {
	// 			list.Add(new NodeChangeDescription(xt, yt, i));
	// 		}
	//
	// 		return list;
	// 	}
	//
	// 	protected override bool checkConstraints(SudokuPuzzle puzzle, NodeChangeDescription description) {
	// 		return puzzle.isLegalMove(description.x, description.y, description.value);
	// 	}
	//
	// 	protected override bool checkGoalState(SudokuPuzzle puzzle) {
	// 		return puzzle.getEmptyCellCount() == 0;
	// 	}
	// }
	//
	// public class LFBacktracking : BacktrackingSolver {
	// 	protected override List<NodeChangeDescription> discover(Node<SudokuPuzzle> parent) {
	// 		throw new NotImplementedException();
	// 	}
	//
	// 	protected override bool checkConstraints(SudokuPuzzle puzzle, NodeChangeDescription description) {
	// 		throw new NotImplementedException();
	// 	}
	//
	// 	protected override bool checkGoalState(SudokuPuzzle puzzle) {
	// 		throw new NotImplementedException();
	// 	}
	// }
	//
	// public class ArcBacktracking {
	// 	
	// }
	//
	// public class NodeChangeDescription {
	// 	public int x, y, value;
	//
	// 	public NodeChangeDescription(int x, int y, int value) {
	// 		this.x = x;
	// 		this.y = y;
	// 		this.value = value;
	// 	}
	// }
}


