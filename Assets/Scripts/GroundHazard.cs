using UnityEngine;

public class GroundHazard : Hazard {
	public int warningSubdivisions;
	public int activeSubdivisions;

	public void initialize(RhythmManager rhythm, int x, int y) {
		base.initialize(rhythm, x, y);

		warningSubdivisions = 24;
		activeSubdivisions = 3;
	}

	public override void onSubdivision(int elapsedSubdivisions, double targetTime) {
		if (elapsedSubdivisions < warningSubdivisions) {
			// Warning
			return;
		}

		if (elapsedSubdivisions < warningSubdivisions + activeSubdivisions) {
			// Active
			return;
		}

		Destroy(this);
	}
}