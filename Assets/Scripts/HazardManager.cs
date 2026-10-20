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

		spawnGroundHazard(x, y);

		Debug.Log("Spawn hazard! Beat " + (index / 12).ToString() + " scheduled DSP time: " + targetTime.ToString());
	}

	public void spawnGroundHazard(int x, int y) {
		Vector3 bottomLeft = grid.GetWorldPos(x, y);
		Vector3 tileWidth = grid.GetWorldPos(x + 1, y) - bottomLeft;
		Vector3 tileHeight = grid.GetWorldPos(x, y + 1) - bottomLeft;
		Vector3 center = bottomLeft + (tileWidth + tileHeight) * 0.5f;

		GameObject hazardObject = Instantiate(groundHazardPrefab, center, Quaternion.identity);
		GroundHazard hazard = hazardObject.GetComponent<GroundHazard>();
		hazard.initialize(rhythm, x, y);
		hazardObject.SetActive(true);
	}
}
