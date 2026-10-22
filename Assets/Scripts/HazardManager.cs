using UnityEngine;

public class HazardManager : MonoBehaviour {
	[Header("References")]
	[SerializeField] public RhythmManager rhythm;
	[SerializeField] public Grid grid;
	[SerializeField] public PlayerController player;

	[Header("Ground Hazard Prefabs")]
	[SerializeField] public GameObject jawPrefab;
	[SerializeField] public GameObject thornPrefab;

	[Header("Bullet Prefabs")]
	[SerializeField] public GameObject slowBulletPrefab;
	[SerializeField] public GameObject fastBulletPrefab;

	[Header("Damage")]
	[SerializeField] public float jawDamage = 1f;
	[SerializeField] public float thornDamage = 0.5f;
	[SerializeField] public float slowBulletDamage = 2.5f;
	[SerializeField] public float fastBulletDamage = 1.5f;

	[Header("Bullet Timing")]
	[SerializeField] public int slowBulletSubdivisionsPerTile = 12;
	[SerializeField] public int fastBulletSubdivisionsPerTile = 6;

	[Header("Ground Hazard Timing")]
	[SerializeField] public int groundWarningSubdivisions = 24;
	[SerializeField] public int groundActiveSubdivisions = 3;

	[Header("Progression")]
	[SerializeField] public int subdivisionsPerSpawn = 12;
	[SerializeField] public int hazardsPerSpawn = 1;

	public int nextStageIndex = 0;
	public int currentStage = -1;
	public int spawnCount = 0;

	public void OnEnable() {
		rhythm.onSubdivision += handleSubdivision;
	}

	public void OnDisable() {
		rhythm.onSubdivision -= handleSubdivision;
	}

	public void handleSubdivision(int index, double targetTime) {
		// Process progression milestones
		bool stageChanged = false;

		while (nextStageIndex < 10 && rhythm.songProgress >= nextStageIndex / 10.0) {
			currentStage = nextStageIndex;

			Debug.Log("Hazard stage " + nextStageIndex + " unlocked at " + (rhythm.songProgress * 100.0).ToString() + "% song progress.");

			spawnStageHazards(nextStageIndex, index);
			nextStageIndex++;
			stageChanged = true;
		}

		// Spawn random hazards every subdivisionsPerSpawn subdivisions
		// Also, don't spawn random hazards on the exact same subdivision as a milestone's debut
		if (!stageChanged && currentStage >= 0 && subdivisionsPerSpawn > 0 && index % subdivisionsPerSpawn == 0) {
			for (int i = 0; i < hazardsPerSpawn; i++) {
				spawnRandomUnlockedHazard(index);
			}
		}
	}

	public int getRandomCoordinate(int min, int max, int playerCoordinate) {
		int maxDistance = Mathf.Max(Mathf.Abs(min - playerCoordinate), Mathf.Abs(max - playerCoordinate));

		float totalWeight = 0f;

		for (int coordinate = min; coordinate <= max; coordinate++) {
			int distance = Mathf.Abs(coordinate - playerCoordinate);
			totalWeight += 1f + (float)rhythm.songProgress * (maxDistance - distance);
		}

		float randomValue = Random.value * totalWeight;

		for (int coordinate = min; coordinate <= max; coordinate++) {
			int distance = Mathf.Abs(coordinate - playerCoordinate);
			float weight = 1f + (float)rhythm.songProgress * (maxDistance - distance);

			randomValue -= weight;

			if (randomValue < 0f) {
				return coordinate;
			}
		}

		return max;
	}

	public void spawnStageHazards(int stage, int index) {
		switch (stage) {
			case 0:
				// 1x1 jaw ground (1)
				spawnJawSquare(1, index);
				break;

			case 1:
				// 1 slow bullet (2.5)
				spawnRandomBullet(slowBulletPrefab, slowBulletDamage, slowBulletSubdivisionsPerTile, index);
				break;

			case 2:
				// 1 fast bullet (3)
				spawnRandomBullet(fastBulletPrefab, fastBulletDamage, fastBulletSubdivisionsPerTile, index);
				break;

			case 3:
				// 2x2 jaw ground (4)
				spawnJawSquare(2, index);
				break;

			case 4:
				// 9 thorns in a row or column (4.5)
				spawnThornRow(index);
				break;

			case 5:
				// Start spawning two random unlocked hazards every 12 subdivisions from now on
				if (hazardsPerSpawn != 2) {
					hazardsPerSpawn = 2;
				}
				else {
					// ... unless it already exists, in which case spawn an extra (recursion hazard but idgaf lol)
					spawnRandomUnlockedHazard(index);
					spawnRandomUnlockedHazard(index);
				}
				break;

			case 6:
				// 2 slow bullets (5)
				for (int i = 0; i < 2; i++) {
					spawnRandomBullet(slowBulletPrefab, slowBulletDamage, slowBulletSubdivisionsPerTile, index);
				}
				break;

			case 7:
				// 2 fast bullets (6)
				for (int i = 0; i < 2; i++) {
					spawnRandomBullet(fastBulletPrefab, fastBulletDamage, fastBulletSubdivisionsPerTile, index);
				}
				break;

			case 8:
				// 17 thorns: a complete row and column (8.5)
				spawnThornCross(index);
				break;

			case 9:
				// 3x3 jaw ground (9)
				spawnJawSquare(3, index);
				break;
		}
	}

	public void spawnGroundHazard(GameObject prefab, int x, int y, float damage, int index) {
		GameObject hazardObject = Instantiate(prefab, grid.GetWorldPos(x, y), Quaternion.identity);
		hazardObject.transform.parent = transform;
		GroundHazard hazard = hazardObject.GetComponent<GroundHazard>();

		if (hazard == null) {
			Destroy(hazardObject);
			return;
		}

		hazard.initialize(rhythm, x, y, damage, groundWarningSubdivisions, groundActiveSubdivisions);
		hazard.spawnSubdivision = index;
		spawnCount++;
	}

	public void spawnJawSquare(int size, int index) {
		int minX = -(grid.width / 2);
		int minY = -(grid.height / 2);
		int maxX = minX + grid.width - size;
		int maxY = minY + grid.height - size;

		int startX = getRandomCoordinate(minX, maxX, player.position.x - (size - 1) / 2);
		int startY = getRandomCoordinate(minY, maxY, player.position.y - (size - 1) / 2);

		for (int x = startX; x < startX + size; x++) {
			for (int y = startY; y < startY + size; y++) {
				spawnGroundHazard(jawPrefab, x, y, jawDamage, index);
			}
		}
	}

	public void spawnThornRow(int index) {
		bool isColumn = Random.Range(0, 2) == 0;

		if (isColumn) {
			int x = getRandomCoordinate(-(grid.width / 2), grid.width / 2, player.position.x);

			for (int y = -(grid.height / 2); y <= grid.height / 2; y++) {
				spawnGroundHazard(thornPrefab, x, y, thornDamage, index);
			}
		}
		else {
			int y = getRandomCoordinate(-(grid.height / 2), grid.height / 2, player.position.y);

			for (int x = -(grid.width / 2); x <= grid.width / 2; x++) {
				spawnGroundHazard(thornPrefab, x, y, thornDamage, index);
			}
		}
	}

	public void spawnThornCross(int index) {
		int row = getRandomCoordinate(-(grid.height / 2), grid.height / 2, player.position.y);
		int column = getRandomCoordinate(-(grid.width / 2), grid.width / 2, player.position.x);

		for (int x = -(grid.width / 2); x <= grid.width / 2; x++) {
			spawnGroundHazard(thornPrefab, x, row, thornDamage, index);
		}

		// Spawn the column, skipping the shared intersection
		for (int y = -(grid.height / 2); y <= grid.height / 2; y++) {
			if (y == row) {
				continue;
			}

			spawnGroundHazard(thornPrefab, column, y, thornDamage, index);
		}
	}

	public void spawnRandomBullet(GameObject prefab, float damage, int subdivisionsPerTile, int index) {
		// Choose one of the four edges
		int edge = Random.Range(0, 4);

		int x;
		int y;
		int dx;
		int dy;

		switch (edge) {
			case 0: // Left
				x = -(grid.width / 2) - 1;
				y = getRandomCoordinate(-(grid.height / 2), grid.height / 2, player.position.y);
				dx = 1;
				dy = 0;
				break;

			case 1: // Right
				x = grid.width / 2 + 1;
				y = getRandomCoordinate(-(grid.height / 2), grid.height / 2, player.position.y);
				dx = -1;
				dy = 0;
				break;

			case 2: // Down
				x = getRandomCoordinate(-(grid.width / 2), grid.width / 2, player.position.x);
				y = -(grid.height / 2) - 1;
				dx = 0;
				dy = 1;
				break;

			default: // Up
				x = getRandomCoordinate(-(grid.width / 2), grid.width / 2, player.position.x);
				y = grid.height / 2 + 1;
				dx = 0;
				dy = -1;
				break;
		}

		GameObject bulletObject = Instantiate(prefab, grid.GetWorldPos(x, y), Quaternion.identity);
		BulletHazard bullet = bulletObject.GetComponent<BulletHazard>();

		if (bullet == null) {
			Destroy(bulletObject);
			return;
		}

		bullet.initialize(rhythm, x, y, damage, dx, dy, subdivisionsPerTile);

		bullet.spawnSubdivision = index;

		spawnCount++;
	}

	public void spawnRandomUnlockedHazard(int index) {
		if (currentStage < 0) {
			return;
		}

		float totalWeight = 0f;

		for (int stage = 0; stage <= currentStage; stage++) {
			totalWeight += 1f + (float)rhythm.songProgress * stage;
		}

		float randomValue = Random.value * totalWeight;

		for (int stage = 0; stage <= currentStage; stage++) {
			float weight = 1f + (float)rhythm.songProgress * stage;

			randomValue -= weight;

			if (randomValue < 0f) {
				spawnStageHazards(stage, index);
				return;
			}
		}

		spawnStageHazards(currentStage, index);
	}
}