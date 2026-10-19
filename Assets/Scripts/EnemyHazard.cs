using UnityEngine;

public class EnemyHazard : Hazard {
	public int subdivisionsPerTile;
	public GridPosition targetPosition;

	public EnemyHazard(RhythmManager rhythm, int x, int y, int subdivisionsPerTile, GridPosition initialTarget) : base(rhythm, x, y) {
		this.subdivisionsPerTile =
			subdivisionsPerTile > 0 ? subdivisionsPerTile : 1;

		targetPosition = initialTarget;
	}

	public override void onSubdivision(int elapsedSubdivisions, double targetTime) {
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
			position.Move(System.Math.Sign(dx), 0);
		}
		else {
			position.Move(0, System.Math.Sign(dy));
		}
	}
}