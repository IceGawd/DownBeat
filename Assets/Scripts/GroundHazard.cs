using UnityEngine;

public class GroundHazard : Hazard {
	public int warningSubdivisions;
	public int activeSubdivisions;

	public void initialize(RhythmManager rhythm, int x, int y, float d) {
		base.initialize(rhythm, x, y, d);

		warningSubdivisions = 24;
		activeSubdivisions = 3;
	}

	public override void onSubdivision(int elapsedSubdivisions, double targetTime) {
		if (elapsedSubdivisions < warningSubdivisions) {
			// Warning
			hazardAnim.SetBool("isActive", false);
			return;
		}

		if (elapsedSubdivisions < warningSubdivisions + activeSubdivisions) {
			// Active
			hazardAnim.SetBool("isActive", true);
			PlayerController player = FindAnyObjectByType<PlayerController>();
			PlayerStateManager playerState = FindAnyObjectByType<PlayerStateManager>();

			if (player != null && playerState != null) {
				if (position.equals(player.position)) {
					int currentSubdivision = spawnSubdivision + elapsedSubdivisions;
					playerState.tryTakeDamage(damage, currentSubdivision);
				}
			}
			return;
		}

		// Debug.Log("Destroying ground hazard at " + position.x + ", " + position.y + " spawnSubdivision: " + spawnSubdivision);
		Destroy(gameObject);
	}
}