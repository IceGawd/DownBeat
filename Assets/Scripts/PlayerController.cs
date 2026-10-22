using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
	public float moveSpeed = 10f;
	public float moveDist = 1;
	public GridPosition position = new GridPosition(0, 0);

	private int bearing;
	[SerializeField] Animator playerAnim;
	[SerializeField] Transform movePoint;
	[SerializeField] Grid grid;

	void Start() {
		movePoint.parent = null;
		bearing = 2;
		playerAnim.SetInteger("Bearing", 2);
		transform.position = grid.GetWorldPos(position.x, position.y);
	}

	// Update is called once per frame
	void Update() {
		transform.position = (transform.position + movePoint.position) / 2;
		if( transform.GetChild(0).GetComponent<SpriteRenderer>().color != Color.white) {
			transform.GetChild(0).GetComponent<SpriteRenderer>().color += new Color(0f, 0.4f, 0.4f, 0f) * Time.deltaTime;
		}
		playerAnim.SetBool("Moving", false);
		playerAnim.SetBool("Parrying", false);
	}


	public void attemptMove() {
		Keyboard kb = Keyboard.current;

		if (kb == null || grid == null) {
			return;
		}

		if (Vector3.Distance(transform.position, movePoint.position) > 0.05f)
			return;

		int dx = 0;
		int dy = 0;
		int newBearing = bearing;

		if (kb.wKey.wasPressedThisFrame || kb.upArrowKey.wasPressedThisFrame) {
			dy = 1;
			newBearing = 0;
		}
		else if (kb.aKey.wasPressedThisFrame || kb.leftArrowKey.wasPressedThisFrame) {
			dx = -1;
			newBearing = 3;
		}
		else if (kb.sKey.wasPressedThisFrame || kb.downArrowKey.wasPressedThisFrame) {
			dy = -1;
			newBearing = 2;
		}
		else if (kb.dKey.wasPressedThisFrame || kb.rightArrowKey.wasPressedThisFrame) {
			dx = 1;
			newBearing = 1;
		}
		else if (kb.cKey.wasPressedThisFrame) {
			dx = (bearing % 2) * (2 - bearing);
			dy = (1 - (bearing % 2)) * (1 - bearing) ;

			Debug.Log("bearing: " + bearing);
			Debug.Log("dx: " + dx);
			Debug.Log("dy: " + dy);
		}
		else {
			return;
		}

		GridPosition destination = new GridPosition(position.x + dx, position.y + dy);

		// Stop movements out of grid
		if (!grid.inGrid(destination)) {
			return;
		}

		// The move is valid, so update both logical and visual positions.
		position.move(dx, dy);
		movePoint.position += new Vector3(
			dx * moveDist,
			dy * moveDist,
			0f
		);

		bearing = newBearing;
		playerAnim.SetInteger("Bearing", bearing);
		playerAnim.SetBool("Moving", true);
	}

	public void attepmtParry() {
		playerAnim.SetBool("Parrying", true);

		RhythmManager rhythmManager = FindAnyObjectByType<RhythmManager>();
		PlayerStateManager playerState = FindAnyObjectByType<PlayerStateManager>();
		playerState.parryUntil = rhythmManager.index + playerState.parry;
	}

	public int getBearing() {
		return bearing;
	}
}
