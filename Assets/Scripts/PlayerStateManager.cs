using UnityEngine;

public class PlayerStateManager : MonoBehaviour {
	[SerializeField] public RhythmManager rhythm;
	[SerializeField] public FlowMeter meter;
	[SerializeField] public HealthManager health;
	[SerializeField] public GridPosition position;
	[SerializeField] public int invincibility = 12;
	[SerializeField] public int parry = 6;

	public int parryUntil = -1;

	int lastHitSubdivision = -12;

	public bool tryTakeDamage(float amount, int currentSubdivision) {
		if (health == null) {
			Debug.LogError("PlayerStateManager: HealthManager is not assigned.");
			return false;
		}

		Debug.Log(currentSubdivision);

		// Reject damage during the invincibility window.
		if (currentSubdivision < lastHitSubdivision + invincibility) {
			return false;
		}

		health.takeDamage(amount);

		lastHitSubdivision = currentSubdivision;

		Debug.Log("Player took damage at subdivision " + currentSubdivision);

		return true;
	}
}
