using UnityEngine;

public class RhythmManager : MonoBehaviour {
	[Header("Song")]
	[SerializeField] public AudioSource musicSource;
	[SerializeField] public AudioClip song;
	[SerializeField] public double bpm = 67.89;

	[Header("Scheduling")]
	[SerializeField] public double startDelay = 1.0;

	public double songStartDspTime;
	public bool isPlaying;

	public double BeatDuration => 60.0 / bpm;
	public double SubdivisionDuration => BeatDuration / 12;
	public double SongTime => isPlaying ? AudioSettings.dspTime - songStartDspTime : 0.0;
	public int CurrentBeat => isPlaying ? Mathf.FloorToInt((float)(SongTime / BeatDuration)) : -1;

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
		return songStartDspTime + beatIndex * BeatDuration;
	}

	public double getSubdivisionTime(int subdivisionIndex) {
		return songStartDspTime + subdivisionIndex * SubdivisionDuration;
	}

	public bool isBeatDue(int beatIndex) {
		return isPlaying && AudioSettings.dspTime >= getBeatTime(beatIndex);
	}
}