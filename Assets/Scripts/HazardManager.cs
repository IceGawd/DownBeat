using UnityEngine;

public class HazardManager : MonoBehaviour {
	[Header("References")]
	[SerializeField] public RhythmManager rhythm;
	[SerializeField] public GameObject groundHazardPrefab;
	public Grid grid;

	public int spawnCount = 0;

	public void Awake() {
		rhythm = FindAnyObjectByType<RhythmManager>();
	}

	public void OnEnable() {
		rhythm.onSubdivision += handleSubdivision;
	}

	public void OnDisable() {
		rhythm.onSubdivision -= handleSubdivision;
	}

	public void handleSubdivision(int index, double targetTime) {
		if (index % 12 != 0) {
			return;
		}

		int x = spawnCount % 5;
		int y = 2;

		spawnCount += 1;

		spawnGroundHazard(x, y);

		Debug.Log("Spawn hazard! Beat " + (index / 12).ToString() + " scheduled DSP time: " + targetTime.ToString());

		// Destroy(this);
	}

	public void spawnGroundHazard(int x, int y) {
		GameObject hazardObject = Instantiate(groundHazardPrefab, grid.GetWorldPos(x, y), Quaternion.identity);
		GroundHazard hazard = hazardObject.GetComponent<GroundHazard>();
		hazard.initialize(rhythm, x, y);
		hazardObject.SetActive(true);
	}
}
