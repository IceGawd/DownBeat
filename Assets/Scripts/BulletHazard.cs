using UnityEngine;

public class BulletHazard : Hazard {
	public int dx;
	public int dy;
	public int subdivisionsPerTile;

	public BulletHazard(RhythmManager rhythm, int x, int y, int dx, int dy, int subdivisionsPerTile): base(rhythm, x, y) {
		this.dx = dx;
		this.dy = dy;
		this.subdivisionsPerTile = subdivisionsPerTile;
	}

	public override void onSubdivision(int elapsedSubdivisions, double targetTime) {
		if (elapsedSubdivisions % subdivisionsPerTile != 0) {
			return;
		}

		position.move(dx, dy);
	}
}