using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour {
    public ParticleSystem explosionPrefab;
    public RacerManager manager;

    void OnTriggerEnter(Collider collision)
    {
        explosionPrefab.transform.position = transform.position;
        if (explosionPrefab.isPlaying) explosionPrefab.Stop();
        explosionPrefab.Play();
        manager.miniGame.NotifyLostLife();
    }
}
