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

	public void Awake() {
		musicSource = GetComponent<AudioSource>();
		musicSource.playOnAwake = false;
		musicSource.clip = song;
		startSong();
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
}