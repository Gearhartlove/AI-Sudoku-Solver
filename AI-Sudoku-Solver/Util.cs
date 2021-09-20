using System;

public class Util {
	public static T Clamp<T>(T val, T min, T max) where T : IComparable<T>
	{
		if (val.CompareTo(min) < 0) return min;
		else if(val.CompareTo(max) > 0) return max;
		else return val;
	}

	public static T[] getRow<T>(T[,] arr, int row) {
		row = Util.Clamp(row, 0, 8);
		var r = new T[9];
		for (int i = 0; i < 9; i++) {
			r[i] = arr[i, row];
		}

		return r;
	}
	
	public static T[] getCol<T>(T[,] arr, int col) {
		col = Util.Clamp(col, 0, 8);
		var r = new T[9];
		for (int i = 0; i < 9; i++) {
			r[i] = arr[col, i];
		}

		return r;
	}
	
	public static T[] getSquare<T>(T[,] arr, int x, int y) {
		x = Util.Clamp(x, 0, 2);
		y = Util.Clamp(y, 0, 2);

		var s = new T[9];
		
		for (int j = y * 3; j < (y * 3) + 3; j++) {
			for (int i = x * 3; i < (x * 3) + 3; i++) {
				s[(i - (x*3))  + ((j - (y*3)) * 3)] = arr[i, j];
			}
		}

		return s;
	}
	
	public static T[] getSquareXY<T>(T[,] arr, int x, int y) {
		return Util.getSquare(arr, (int) Math.Floor((decimal) (x / 3)), (int) Math.Floor((decimal) (y / 3)));
	}
}
