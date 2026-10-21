using UnityEngine;

public class BulletHazard : Hazard
{
	[Header("Movement")]
	public int dx;
	public int dy;
	public int subdivisionsPerTile = 12;

	private bool hasMoved;

	public void initialize(RhythmManager rhythm, int x, int y, float damage, int dx, int dy, int subdivisionsPerTile) {
		base.initialize(rhythm, x, y, damage);

		this.dx = dx;
		this.dy = dy;
		this.subdivisionsPerTile = Mathf.Max(1, subdivisionsPerTile);

		hasMoved = false;
	}

	public override void onSubdivision(int elapsedSubdivisions, double targetTime) {
		if (elapsedSubdivisions <= 0) {
			return;
		}

		if (elapsedSubdivisions % subdivisionsPerTile != 0) {
			return;
		}

		// Advance exactly one tile.
		position = new GridPosition(position.x + dx, position.y + dy);
		transform.position = grid.getTileCenterPos(position.x, position.y);
		hasMoved = true;

		// The bullet has left the grid.
		if (!grid.inGrid(position)) {
			destroyHazard();
			return;
		}

		// The bullet has entered the player's tile.
		if (hitPlayer(elapsedSubdivisions)) {
			destroyHazard();
		}
	}
}