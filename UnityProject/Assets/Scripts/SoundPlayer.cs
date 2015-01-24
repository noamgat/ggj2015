using UnityEngine;
using System.Collections;

public class SoundPlayer : MonoBehaviour {

    public AudioSource introSource;
    public AudioSource loopSource;

	// Update is called once per frame
	void Start () {
        introSource.Play();
        this.ExecuteWithDelay(introSource.clip.length, delegate() { loopSource.Play(); });
	}
}
