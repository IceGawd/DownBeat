using UnityEngine;

public abstract class Hazard : MonoBehaviour {
	[Header("Hazard State")]
	public RhythmManager rhythm;
	public Grid grid;
	public GridPosition position;
	public float damage = 1f;
	public int spawnSubdivision;

	public bool initialized;

	public virtual void initialize(RhythmManager rhythm, int x, int y, float damage) {
		initialize(rhythm, FindAnyObjectByType<Grid>(), x, y, damage, Mathf.FloorToInt((float)(rhythm.songTime / rhythm.subdivisionDuration)));
	}

	public virtual void initialize(RhythmManager rhythm, Grid grid, int x, int y, float damage, int spawnSubdivision) {
		if (initialized && this.rhythm != null) {
			this.rhythm.onSubdivision -= handleSubdivision;
		}

		this.rhythm = rhythm;
		this.grid = grid;
		this.position = new GridPosition(x, y);
		this.damage = damage;
		this.spawnSubdivision = spawnSubdivision;

		initialized = true;

		rhythm.onSubdivision += handleSubdivision;
	}

	public void handleSubdivision(int index, double targetTime) {
		if (!initialized) {
			return;
		}

		int elapsedSubdivisions = index - spawnSubdivision;

		if (elapsedSubdivisions < 0) {
			return;
		}

		onSubdivision(elapsedSubdivisions, targetTime);
	}

	public bool hitPlayer(int elapsedSubdivisions) {
		PlayerController player = FindAnyObjectByType<PlayerController>();
		PlayerStateManager playerState = FindAnyObjectByType<PlayerStateManager>();

		if (player == null || playerState == null) {
			return false;
		}

		if (!position.equals(player.position)) {
			return false;
		}

		playerState.tryTakeDamage(damage, elapsedSubdivisions);

		return true;
	}

	public void destroyHazard() {
		Destroy(gameObject);
	}

	public virtual void OnDestroy() {
		if (rhythm != null) {
			rhythm.onSubdivision -= handleSubdivision;
		}
	}

	public abstract void onSubdivision(int elapsedSubdivisions, double targetTime);
}