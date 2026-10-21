using UnityEngine;

[System.Serializable]
public struct GridPosition {
	public int x;
	public int y;

	public GridPosition(int x, int y) {
		this.x = x;
		this.y = y;
	}

	public void move(int dx, int dy) {
		x += dx;
		y += dy;
	}

	public bool equals(GridPosition other) {
		return x == other.x && y == other.y;
	}
}