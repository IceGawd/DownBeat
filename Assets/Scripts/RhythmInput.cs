using UnityEngine;
using UnityEngine.InputSystem;

public class RhythmInput : MonoBehaviour {
	[Header("References")]
	[SerializeField] public RhythmManager rhythm;

	[Header("Timing")]
	[SerializeField] public double toleranceFrames = 3.0;	// 0 = Perfect, 1 = Great, 2 = Good, 3 = Ok
	[SerializeField] public double lockoutFrames = 20.0;
	[SerializeField] public double FPS = 60.0;

	public double tolerance => toleranceFrames / FPS;
	public double lockoutDuration => lockoutFrames / FPS;

	// The earliest DSP time at which each button can be used again.
	public double moveLockoutUntil;
	public double zLockoutUntil;
	public double xLockoutUntil;
	public double cLockoutUntil;

	public void Awake() {
		if (rhythm == null)
			rhythm = GetComponent<RhythmManager>();
	}

	public void Update() {
		if (rhythm == null || !rhythm.isPlaying)
			return;

		double now = AudioSettings.dspTime;

		// Movement - Half Note
		if (anyMovementKeyPressed()) {
			checkInput("Movement", now, 12, ref moveLockoutUntil);
		}

		// Z - Quarter Note
		if (Keyboard.current.zKey.wasPressedThisFrame) {
			checkInput("Z", now, 6, ref zLockoutUntil);
		}

		// X - Triplet
		if (Keyboard.current.xKey.wasPressedThisFrame) {
			checkInput("X", now, 4, ref xLockoutUntil);
		}

		// C - Eighth Note
		if (Keyboard.current.cKey.wasPressedThisFrame) {
			checkInput("C", now, 3, ref cLockoutUntil);
		}
	}

	public bool anyMovementKeyPressed() {
		return
            Keyboard.current.wKey.wasPressedThisFrame ||
            Keyboard.current.aKey.wasPressedThisFrame ||
            Keyboard.current.sKey.wasPressedThisFrame ||
            Keyboard.current.dKey.wasPressedThisFrame ||
            Keyboard.current.upArrowKey.wasPressedThisFrame ||
            Keyboard.current.downArrowKey.wasPressedThisFrame ||
            Keyboard.current.leftArrowKey.wasPressedThisFrame ||
            Keyboard.current.rightArrowKey.wasPressedThisFrame;
	}

	public void checkInput(string inputName, double now, int subdivisionInterval, ref double lockoutUntil) {
		if (now < lockoutUntil) {
			Debug.Log(inputName + " LOCKOUT! Available at: " + lockoutUntil.ToString());
			return;
		}

		// Debug.Log((now - rhythm.songStartDspTime - rhythm.delaySec) / rhythm.beatDuration);

		if (tryGetNearestSubdivision(now, subdivisionInterval, out int subdivisionIndex, out double timingError)) {
			Debug.Log(inputName + " HIT! Subdivision: " + subdivisionIndex.ToString() + ", Timing error: " + (timingError * 1000).ToString() + " ms");
		}
		else {
			Debug.Log(inputName + " MISSED! Locked out. Timing error: " + (timingError * 1000).ToString() + " ms");
		}
	}

	public bool tryGetNearestSubdivision(double now, int subdivisionInterval, out int nearestIndex, out double timingError) {
		nearestIndex = -1;
		timingError = double.MaxValue;

		double elapsed = now - rhythm.songStartDspTime - rhythm.delaySec;
		int approximateIndex = Mathf.RoundToInt((float) (elapsed / rhythm.subdivisionDuration));

		for (int offset = -1; offset <= 1; offset++) {
			int index = approximateIndex + offset;

			if (index < 0 || index % subdivisionInterval != 0) {
				continue;
			}

			double targetTime = rhythm.songStartDspTime + index * rhythm.subdivisionDuration;
			double error = now - targetTime - rhythm.delaySec;

			if (System.Math.Abs(error) < System.Math.Abs(timingError)) {
				nearestIndex = index;
				timingError = error;
			}
		}

		return nearestIndex >= 0 && System.Math.Abs(timingError) <= tolerance;
	}
}