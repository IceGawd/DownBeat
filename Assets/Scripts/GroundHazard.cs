using UnityEngine;

public class GroundHazard : Hazard {
	public int warningSubdivisions;
	public int activeSubdivisions;

	public GroundHazard(RhythmManager rhythm, int x, int y): base(rhythm, x, y) {
		warningSubdivisions = 24;	// Change on difficulty
		activeSubdivisions = 3; 	// Change on difficulty
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