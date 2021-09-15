using System;
using System.IO;

public class SudokuPuzzle {

	private int[,] cells = new int[9,9];
	private bool[,] lockedCells = new bool[9,9];

	public SudokuPuzzle() {
		for(int i = 0; i < 9; i++) {
			for (int j = 0; j < 9; j++) {
				cells[i, j] = -1;
				lockedCells[i, j] = false;
			}
		}
		
	}
	
	public SudokuPuzzle(int[,] cells, bool[,] lockedCells) {
		this.cells = cells;
		this.lockedCells = lockedCells;
	}
	
	public SudokuPuzzle(string filename) {
		for(int i = 0; i < 9; i++) {
			for (int j = 0; j < 9; j++) {
				lockedCells[i, j] = false;
			}
		}
		
		fromFile(filename);
	}

	public int setValue(int x, int y, int value) {
		if (isLocked(x, y)) return cells[x, y];
		cells[x, y] = value;
		return value;
	}

	public int setLockedValue(int x, int y, int value) {
		if (isLocked(x, y)) return cells[x, y];
		cells[x, y] = value;
		lockedCells[x, y] = true;
		return value;
	}

	public int getValue(int x, int y) {
		return cells[x, y];
	}

	public int[] getRow(int row) {
		row = Util.Clamp(row, 0, 8);
		var r = new int[9];
		for (int i = 0; i < 9; i++) {
			r[i] = cells[i, row];
		}

		return r;
	}
	
	public int[] getCol(int col) {
		col = Util.Clamp(col, 0, 8);
		var c = new int[9];
		for (int i = 0; i < 9; i++) {
			c[i] = cells[col, i];
		}

		return c;
	}

	public int[] getSquare(int x, int y) {
		x = Util.Clamp(x, 0, 2);
		y = Util.Clamp(y, 0, 2);

		var s = new int[9];
		
		for (int j = y * 3; j < (y * 3) + 3; j++) {
			for (int i = x * 3; i < (x * 3) + 3; i++) {
				s[i + j * 3] = cells[i, j];
			}
		}

		return s;
	}

	public bool isLocked(int x, int y) {
		return lockedCells[x, y];
	}

	public bool isLegalRow(int row, int value) {
		row = Util.Clamp(row, 0, 8);
		for (int i = 0; i < 9; i++) {
			if (cells[i, row] == value) return false;
		}

		return true;
	}
	
	public bool isLegalCol(int col, int value) {
		col = Util.Clamp(col, 0, 8);
		for (int i = 0; i < 9; i++) {
			if (cells[col, i] == value) return false;
		}

		return true;
	}
	
	public bool isLegalSquare(int x, int y, int value) {
		x = Util.Clamp(x, 0, 2);
		y = Util.Clamp(y, 0, 2);

		for (int j = y * 3; j < (y * 3) + 3; j++) {
			for (int i = x * 3; i < (x * 3) + 3; i++) {
				if (cells[i, j] == value) return false;
			}
		}

		return true;
	}

	public bool isLegalMove(int x, int y, int value) {
		return isLegalCol(x, value)
		    && isLegalRow(y, value)
		    && isLegalSquare(x / 3, y / 3, value);
	}

	public SudokuPuzzle copy() {
		return new SudokuPuzzle(cells.Clone() as int[,], lockedCells.Clone() as bool[,]);
	}

	private void fromFile(string filename) {
		string[] lines = File.ReadAllLines("./puzzles/" + filename);

		int x = 0, y = 0;
		foreach (var line in lines) {
			var numbers = line.Split(',');
			foreach (var i in numbers) {
				if (i == "?") {
					cells[x, y] = -1;
				} else {
					this.setLockedValue(x, y, Int32.Parse(i));
				}
				x++;
			}

			y++;
			x = 0;
		}
	}

	public void toFile(string filename) {
		Directory.CreateDirectory("solutions");
		File.WriteAllText("./solutions/" + filename, toOutputString());
	}

	public string toOutputString() {
		string o = "";
		
		for (int y = 0; y < 9; y++) {
			for (int x = 0; x < 9; x++) {
				if (cells[x, y] < 0) o += "?";
				else o += cells[x, y];

				if (x != 8) o += ",";
			}
			
			o += "\n";
		}

		return o;
	}
	
	public override string ToString() {
		string o = "";
		
		for (int y = 0; y < 9; y++) {
			if (y == 0 || y == 3 || y == 6) o += "+-----------------+\n";
			for (int x = 0; x < 9; x++) {
				if (x == 0 || x == 3 || x == 6) o += "|";
				else o += " ";
				if (cells[x, y] < 0) o += "?";
				else o += cells[x, y];
				if (x == 8) o += "|";
			}
			
			o += "\n";
		}

		o += "+-----------------+";
		
		return o;
	}
}
