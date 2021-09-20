using System;
using System.Collections.Generic;
using System.IO;

public class SudokuPuzzle {

	private int[,] cells = new int[9,9];
	private bool[,] lockedCells = new bool[9,9];
	private int spacesLeft = 81;

	public SudokuPuzzle() {
		for(int i = 0; i < 9; i++) {
			for (int j = 0; j < 9; j++) {
				cells[i, j] = -1;
				lockedCells[i, j] = false;
			}
		}
	}
	
	public SudokuPuzzle(int[,] cells, bool[,] lockedCells, int spacesLeft) {
		this.cells = cells;
		this.lockedCells = lockedCells;
		this.spacesLeft = spacesLeft;
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
		x = Util.Clamp(x, 0, 8);
		y = Util.Clamp(y, 0, 8);
		value = Util.Clamp(value, 1, 9);
		
		if (isLocked(x, y)) return cells[x, y];
		if (cells[x, y] == -1 || cells[x, y] == 0) spacesLeft--;
		cells[x, y] = value;
		return value;
	}

	public int setLockedValue(int x, int y, int value) {
		x = Util.Clamp(x, 0, 8);
		y = Util.Clamp(y, 0, 8);
		value = Util.Clamp(value, 1, 9);
		
		if (isLocked(x, y)) return cells[x, y];
		if (cells[x, y] == -1 || cells[x, y] == 0) spacesLeft--;
		cells[x, y] = value;
		lockedCells[x, y] = true;
		return value;
	}

	public int getValue(int x, int y) {
		return cells[x, y];
	}

	public int[] getRow(int row) {
		return Util.getRow(cells, row);
	}
	
	public int[] getCol(int col) {
		return Util.getCol(cells, col);
	}

	public int[] getSquare(int x, int y) {
		return Util.getSquare(cells, x, y);
	}
	
	
	public int[] getSquareXY(int x, int y) {
		return Util.getSquareXY(cells, (int) Math.Floor((decimal) (x / 3)), (int) Math.Floor((decimal) (y / 3)));
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
				if  (cells[i, j] == value) return false;
			}
		}

		return true;
	}
	
	public bool isLegalSquareXY(int x, int y, int value) {
		return isLegalSquare((int) Math.Floor((decimal) (x / 3)), (int) Math.Floor((decimal) (y / 3)), value);
	}

	public bool isLegalMove(int x, int y, int value) {
		return isLegalCol(x, value)
		    && isLegalRow(y, value)
		    && isLegalSquareXY(x, y, value)
			&& !isLocked(x, y);
	}

	public int constraintTestRow(int row) {
		var array = this.getRow(row);
		var count = 0;
		
		for (int i = 0; i < 9; i++) {
			var value = array[i];
			if(value == -1) continue;
			for (int j = 0; j < 9; j++) {
				if(j == i) continue;
				if (value == array[j]) {
					count++;
					break;
				}
			}
		}

		return count;
	}
	
	public int constraintTestCol(int col) {
		var array = this.getCol(col);
		var count = 0;
		
		for (int i = 0; i < 9; i++) {
			var value = array[i];
			if(value == -1) continue;
			for (int j = 0; j < 9; j++) {
				if(j == i) continue;
				if (value == array[j]) {
					count++;
					break;
				}
			}
		}

		return count;
	}
	
	public int constraintTestSquare(int x, int y) {
		var array = this.getSquare(x, y);
		var count = 0;
		
		for (int i = 0; i < 9; i++) {
			var value = array[i];
			if(value == -1) continue;
			for (int j = 0; j < 9; j++) {
				if(j == i) continue;
				if (value == array[j]) {
					count++;
					break;
				}
			}
		}

		return count;
	}
	
	public int constraintTestSquareXY(int x, int y) {
		return constraintTestSquare((int) Math.Floor((decimal) (x / 3)), (int) Math.Floor((decimal) (y / 3)));
	}

	public int[] getDomain(int x, int y) {
		var row = getRow(y);
		var col = getCol(x);
		var square = getSquareXY(x, y);

		List<int> domain = new List<int>();
		domain.AddRange(new []{1, 2, 3, 4, 5, 6, 7, 8, 9});

		for (var i = 0; i < 9; i++) {
			domain.Remove(row[i]);
			domain.Remove(col[i]);
			domain.Remove(square[i]);
		}

		return domain.ToArray();
	}

	public int constraintTest() {
		int count = 0;

		for (int i = 0; i < 9; i++) {
			count += constraintTestRow(i);
			count += constraintTestCol(i);
		}

		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++) {
				count += constraintTestSquare(i, j);
			}
		}

		return count;
	}

	public int getEmptyCellCount() {
		return spacesLeft;
	}

	public int getFilledCellCount() {
		return 81 - spacesLeft;
	}

	public SudokuPuzzle copy() {
		return new SudokuPuzzle(cells.Clone() as int[,], lockedCells.Clone() as bool[,], spacesLeft);
	}

	public int[,] getCells() {
		return cells;
	}

	private void fromFile(string filename) {
		string[] lines = File.ReadAllLines(filename);

		int x = 0, y = 0;
		foreach (var line in lines) {
			var numbers = line.Split(',');
			foreach (var i in numbers) {
				if (i == "?") {
					cells[x, y] = -1;
				} else {
					setLockedValue(x, y, Int32.Parse(i));
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
			if (y == 0 || y == 3 || y == 6) o += "+-----------------------+\n";
			for (int x = 0; x < 9; x++) {
				if (x == 0 || x == 3 || x == 6) o += "|";
				else o += "  ";
				if (cells[x, y] < 0) o += "?";
				else o += cells[x, y];
				if (x == 8) o += "|";
			}
			
			o += "\n";
		}

		o += "+-----------------------+";
		
		return o;
	}
}
