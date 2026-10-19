using UnityEngine;

public class HazardManager : MonoBehaviour {
	[Header("References")]
	[SerializeField] public RhythmManager rhythm;

	public void Awake() {
		rhythm = FindAnyObjectByType<RhythmManager>();
	}

	public void OnEnable() {
		rhythm.onSubdivision += HandleSubdivision;
	}

	public void OnDisable() {
		rhythm.onSubdivision -= HandleSubdivision;
	}

	public void HandleSubdivision(int index, double targetTime) {
		if (index % 12 != 0) {
			return;
		}

		Debug.Log("Spawn hazard! Beat " + (index / 12).ToString() + " scheduled DSP time: " + targetTime.ToString());
	}
}
