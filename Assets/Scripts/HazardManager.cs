
using UnityEngine;

public class HazardManager : MonoBehaviour {
	[Header("References")]
	[SerializeField] public RhythmManager rhythm;

	public void Awake() {
		if (rhythm == null)
			rhythm = FindAnyObjectByType<RhythmManager>();
	}

	public void OnEnable() {
		if (rhythm != null)
			rhythm.OnSubdivision += HandleSubdivision;
	}

	public void OnDisable() {
		if (rhythm != null)
			rhythm.OnSubdivision -= HandleSubdivision;
	}

	public void HandleSubdivision(int index, double targetTime) {
		if (index % 12 != 0) {
			return;
		}

		Debug.Log($"Spawn hazard! Beat {index / 12}, " + $"scheduled DSP time: {targetTime:F3}");
	}
}
