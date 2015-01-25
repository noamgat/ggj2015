using UnityEngine;
using System.Collections;

public class SoundPlayer : MonoBehaviour {

    public AudioSource introSource;
    public AudioSource loopSource;
    public float sharedBufferTime = 0.1f;
	// Update is called once per frame
	void Start () {
        introSource.Play();
	}

    void Update() {
        if (!loopSource.isPlaying && (!introSource.isPlaying || introSource.time > introSource.clip.length - sharedBufferTime)) {
            loopSource.Play();
        }
    }
}
