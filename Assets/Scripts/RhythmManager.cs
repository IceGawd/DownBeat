using System;
using UnityEngine;

public class RhythmManager : MonoBehaviour {
	[Header("Song")]
	[SerializeField] public AudioSource musicSource;
	[SerializeField] public AudioClip song;
	[SerializeField] public double bpm = 60;
	[SerializeField] public double delaySec = 0.2;

	[Header("Scheduling")]
	[SerializeField] public double startDelay = 1.0;

	public double songStartDspTime;
	public bool isPlaying;

	public double beatDuration => 60.0 / bpm;
	public double subdivisionDuration => beatDuration / 12;
	public double songTime => isPlaying ? AudioSettings.dspTime - songStartDspTime : 0.0;
	public int currentBeat => isPlaying ? Mathf.FloorToInt((float)(songTime / beatDuration)) : -1;

	private int[] beatClock = new int[5];
	public void Awake() {
		musicSource = GetComponent<AudioSource>();
		musicSource.playOnAwake = false;
		musicSource.clip = song;
		startSong();
	}

	public void Update()
	{
		int currentSubDivision = Mathf.FloorToInt((float)(songTime / subdivisionDuration));
		beatClock[0] = currentBeat;
		beatClock[1] = currentSubDivision % 12;
		beatClock[2] = currentSubDivision % 6;
		beatClock[3] = currentSubDivision % 4;
		beatClock[4] = currentSubDivision % 3;
	}

	public void startSong() {
		if (isPlaying) {
			return;
		}

		songStartDspTime = AudioSettings.dspTime + startDelay;
		musicSource.PlayScheduled(songStartDspTime);
		isPlaying = true;
	}

	public double getBeatTime(int beatIndex) {
		return songStartDspTime + beatIndex * beatDuration;
	}

	public double getSubdivisionTime(int subdivisionIndex) {
		return songStartDspTime + subdivisionIndex * subdivisionDuration;
	}

	public bool isBeatDue(int beatIndex) {
		return isPlaying && AudioSettings.dspTime >= getBeatTime(beatIndex);
	}

	public int[] getBeatClock()
	{
		return beatClock;
	}
}