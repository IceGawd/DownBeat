using UnityEngine;

public class HazardManager : MonoBehaviour {
	[Header("References")]
	[SerializeField] public RhythmManager rhythm;
	[SerializeField] public Grid grid;

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
		double progress = rhythm.songProgress;
		bool stageChanged = false;

		while (nextStageIndex < 10 && progress >= nextStageIndex / 10.0) {
			currentStage = nextStageIndex;

			Debug.Log("Hazard stage " + nextStageIndex + " unlocked at " + (progress * 100.0).ToString() + "% song progress.");

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

	public void spawnStageHazards(int stage, int index) {
		switch (stage) {
			case 0:
				// 1x1 jaw ground (1)
				spawnGroundHazard(jawPrefab, Random.Range(-(grid.width / 2), grid.width / 2 + 1), Random.Range(-(grid.height / 2), grid.height / 2 + 1), jawDamage, index);
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
				hazardsPerSpawn = 2;
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
		if (x < -(grid.width / 2) || x > grid.width / 2 || y < -(grid.height / 2) || y > grid.height / 2) {
			return;
		}

		GameObject hazardObject = Instantiate(prefab, grid.GetWorldPos(x, y), Quaternion.identity);
		GroundHazard hazard = hazardObject.GetComponent<GroundHazard>();

		if (hazard == null) {
			Destroy(hazardObject);
			return;
		}

		hazard.initialize(rhythm, x, y, damage, groundWarningSubdivisions, groundActiveSubdivisions);

		// Use the exact subdivision that spawned this hazard
		hazard.spawnSubdivision = index;

		spawnCount++;
	}

	public void spawnJawSquare(int size, int index) {
		if (size <= 0 || size > grid.width || size > grid.height)
			return;

		int minX = -(grid.width / 2);
		int minY = -(grid.height / 2);

		int startX = Random.Range(minX, minX + grid.width - size + 1);
		int startY = Random.Range(minY, minY + grid.height - size + 1);

		for (int x = startX; x < startX + size; x++) {
			for (int y = startY; y < startY + size; y++) {
				spawnGroundHazard(jawPrefab, x, y, jawDamage, index);
			}
		}
	}

	public void spawnThornRow(int index) {
		bool isColumn = Random.Range(0, 2) == 0;

		if (isColumn) {
			int x = Random.Range(-(grid.width / 2), grid.width / 2 + 1);

			for (int y = -(grid.height / 2); y <= grid.height / 2; y++) {
				spawnGroundHazard(thornPrefab, x, y, thornDamage, index);
			}
		}
		else {
			int y = Random.Range(-(grid.height / 2), grid.height / 2 + 1);

			for (int x = -(grid.width / 2); x <= grid.width / 2; x++) {
				spawnGroundHazard(thornPrefab, x, y, thornDamage, index);
			}
		}
	}

	public void spawnThornCross(int index) {
		int row = Random.Range(-(grid.height / 2), grid.height / 2 + 1);
		int column = Random.Range(-(grid.width / 2), grid.width / 2 + 1);

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
				y = Random.Range(-(grid.height / 2), grid.height / 2 + 1);
				dx = 1;
				dy = 0;
				break;

			case 1: // Right
				x = grid.width / 2 + 1;
				y = Random.Range(-(grid.height / 2), grid.height / 2 + 1);
				dx = -1;
				dy = 0;
				break;

			case 2: // Down
				x = Random.Range(-(grid.width / 2), grid.width / 2 + 1);
				y = -(grid.height / 2) - 1;
				dx = 0;
				dy = 1;
				break;

			default: // Up
				x = Random.Range(-(grid.width / 2), grid.width / 2 + 1);
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

		// Randomly choose from all stages unlocked so far
		int stage = Random.Range(0, currentStage + 1);

		spawnStageHazards(stage, index);
	}
}