using UnityEngine;

public class GroundHazard : Hazard {
	[Header("Timing")]
	public int warningSubdivisions = 24;
	public int activeSubdivisions = 3;

	public void initialize(RhythmManager rhythm, int x, int y, float damage, int warning = 24, int active = 3) {
		base.initialize(rhythm, x, y, damage);

		warningSubdivisions = warning;
		activeSubdivisions = active;
	}

	public override void onSubdivision(int index, int elapsedSubdivisions, double targetTime) {
		if (elapsedSubdivisions < warningSubdivisions) {
			// Warning
			hazardAnim.SetBool("isActive", false);
			return;
		}

		if (elapsedSubdivisions < warningSubdivisions + activeSubdivisions) {
			// Active
			hazardAnim.SetBool("isActive", true);
			hitPlayer(index);
			return;
		}

		destroyHazard();
	}
}