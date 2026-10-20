using UnityEngine;
using System;

public abstract class Hazard : MonoBehaviour {
	public RhythmManager rhythm;
	public GridPosition position;

	public int spawnSubdivision = -1;

	public void initialize(RhythmManager rhythm, int x, int y) {
		this.rhythm = rhythm;
		position = new GridPosition(x, y);
	}

	public void Awake() {
		rhythm = FindAnyObjectByType<RhythmManager>();
		rhythm.onSubdivision += handleSubdivision;
	}

	public void handleSubdivision(int subdivisionIndex, double targetTime) {
		if (spawnSubdivision < 0) {
			spawnSubdivision = subdivisionIndex;
		}

		int elapsed = subdivisionIndex - spawnSubdivision;

		onSubdivision(elapsed, targetTime);
	}

	// Hazard specific for override
	public abstract void onSubdivision(int elapsedSubdivisions, double targetTime);

	public void OnDestroy() {
		rhythm.onSubdivision -= handleSubdivision;
	}
}