using UnityEngine;

public class EnemyHazard : Hazard {
	public int subdivisionsPerTile;
	public GridPosition targetPosition;

	public void initialize(RhythmManager rhythm, int x, int y, float d, int subdivisionsPerTile, GridPosition initialTarget) {
		base.initialize(rhythm, x, y, d);
		this.subdivisionsPerTile = subdivisionsPerTile > 0 ? subdivisionsPerTile : 1;

		targetPosition = initialTarget;
	}

	public override void onSubdivision(int index, int elapsedSubdivisions, double targetTime) {
		if (elapsedSubdivisions == 0) {
			return;
		}

		if (elapsedSubdivisions % subdivisionsPerTile != 0) {
			return;
		}

		int dx = targetPosition.x - position.x;
		int dy = targetPosition.y - position.y;

		if (dx == 0 && dy == 0)
			return;

		// Move along the axis with the greater distance.
		if (System.Math.Abs(dx) >= System.Math.Abs(dy)) {
			position.move(System.Math.Sign(dx), 0);
		}
		else {
			position.move(0, System.Math.Sign(dy));
		}
	}
}