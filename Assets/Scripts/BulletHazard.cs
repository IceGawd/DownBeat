using UnityEngine;

public class BulletHazard : Hazard
{
	[Header("Movement")]
	public int dx;
	public int dy;
	public int warningSubdivisions = 12;
	public int subdivisionsPerTile = 12;

	public void initialize(RhythmManager rhythm, int x, int y, float d, int dx, int dy, int subdivisionsPerTile) {
		base.initialize(rhythm, x, y, d);
		
		this.dx = dx;
		this.dy = dy;
		this.subdivisionsPerTile = subdivisionsPerTile;

		hazardAnim.SetBool("isActive", false);
	}

	public override void onSubdivision(int index, int elapsedSubdivisions, double targetTime) {
		if (elapsedSubdivisions <= 0) {
			return;
		}

		if (elapsedSubdivisions <= warningSubdivisions) {
			return;
		}

		// The bullet has entered the player's tile.
		if (hitPlayer(index, true)) {
			destroyHazard();
		}

		if (elapsedSubdivisions % subdivisionsPerTile != 0) {
			return;
		}

		// Advance exactly one tile.
		position.move(dx, dy);
		transform.position = grid.GetWorldPos(position.x, position.y);
		hazardAnim.SetBool("isActive", true);

		// The bullet has left the grid.
		if (!grid.inGrid(position)) {
			destroyHazard();
			return;
		}

		// The bullet has entered the player's tile.
		if (hitPlayer(index, true)) {
			destroyHazard();
		}
	}
}