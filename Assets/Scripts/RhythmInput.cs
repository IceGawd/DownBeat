using UnityEngine;
using UnityEngine.InputSystem;

public class RhythmInput : MonoBehaviour {
	[Header("References")]
	[SerializeField] public RhythmManager rhythm;
	[SerializeField] public PlayerController controller;

	[Header("Timing")]
	[SerializeField] public double tolerance = 0.2;
	[SerializeField] public double lockoutDuration = 0.3;

	// The earliest DSP time at which each button can be used again.
	public double moveLockoutUntil;
	public double zLockoutUntil;
	public double xLockoutUntil;
	public double cLockoutUntil;
	public int parryRefresh;
	private int[] beatClock;
	private int parryCD;
	private int lastBeat;
	public void Awake() {
		if (rhythm == null)
			rhythm = GetComponent<RhythmManager>();
	}

	public void Update() {
		if (rhythm == null || !rhythm.isPlaying)
			return;

		// if (controller == null)
		// 	Debug.Log("Controller not detected");

		double now = AudioSettings.dspTime;
		
		beatClock = rhythm.getBeatClock();

		// WASD / Arrows : Movement - Half Note
		if (anyMovementKeyPressed()) {
			if(checkInput("Movement", now, 12, ref moveLockoutUntil))
			{
				Debug.Log("attempting movement");
				controller.attemptMove();
			}
			// if(beatClock[1] == 0)
			// {
			// 	Debug.Log("attempting movement");
			// 	controller.attemptMove();
			// }

		}

		// Parry: Z - Quarter Note
		if (parryCD > 0)
		{
			if(beatClock[0] > lastBeat)
			{
				parryCD--;
				Debug.Log("Parry coolDown: " + parryCD);
			}
		}
		
		if (Keyboard.current.zKey.wasPressedThisFrame) {
			
			if(parryCD != 0)
			{
				Debug.Log("Parry on cooldown");
			}
			else
			{
				if(checkInput("Z", now, 6, ref zLockoutUntil))
				{
					Debug.Log("attempting parry");
					controller.attepmtParry();
					parryCD = parryRefresh;
				}
				
				// if(beatClock[2] == 0)
				// {
				// 	Debug.Log("attempting parry");
				// 	controller.attepmtParry();
				// 	parryCD = parryRefresh;
				// }
			}
			

		}

		// X - Triplet
		if (Keyboard.current.xKey.wasPressedThisFrame) {
			checkInput("X", now, 4, ref xLockoutUntil);
		}

		// C - Eighth Note
		if (Keyboard.current.cKey.wasPressedThisFrame) {
			if (checkInput("C", now, 3, ref cLockoutUntil)) {
				Debug.Log("attempting flutterstep");
				controller.attemptMove();
			}
		}

		lastBeat = beatClock[0];
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

	public bool checkInput(string inputName, double now, int subdivisionInterval, ref double lockoutUntil) {
		if (now < lockoutUntil) {
			Debug.Log(inputName + " LOCKOUT! Available at: " + lockoutUntil.ToString());
			lockoutUntil = now + lockoutDuration * subdivisionInterval / 12;
			return false;
		}

		lockoutUntil = now + lockoutDuration * subdivisionInterval / 12;
		Debug.Log((now - rhythm.songStartDspTime - rhythm.delaySec) / rhythm.beatDuration);

		if (tryGetNearestSubdivision(now, subdivisionInterval, out int subdivisionIndex, out double timingError)) {
			//Debug.Log(inputName + " HIT! Subdivision: " + subdivisionIndex.ToString() + ", Timing error: " + (timingError * 1000).ToString() + " ms");
			return true;
		}
		else {
			Debug.Log(inputName + " MISSED! Locked out. Timing error: " + (timingError * 1000).ToString() + " ms");
			return false;
		}
	}

	public bool tryGetNearestSubdivision(double now, int subdivisionInterval, out int nearestIndex, out double timingError) {
		nearestIndex = -1;
		timingError = double.MaxValue;

		double elapsed = now - rhythm.songStartDspTime - rhythm.delaySec;
		int approximateIndex = Mathf.RoundToInt((float) (elapsed / rhythm.subdivisionDuration));

		for (int offset = -6; offset <= 6; offset++) {
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

		return nearestIndex >= 0 && System.Math.Abs(timingError) <= tolerance * 12 / subdivisionInterval;
	}
}