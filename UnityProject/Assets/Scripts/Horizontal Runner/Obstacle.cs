using UnityEngine;

public class Obstacle : MonoBehaviour 
{
	public Collider characterCollider;
	public MiniGame miniGame;
	public AudioSource characterAudioSource;
	public ParticleSystem particles = null;
	public float leftEdgeDestructionThreshold = -11f;

	// Update is called once per frame
	// ReSharper disable once UnusedMember.Local
	void Update () 
	{
		if (this.transform.localPosition.x < this.leftEdgeDestructionThreshold)
		{ Destroy(this.gameObject); }
	}

	// ReSharper disable once UnusedMember.Local
	void OnTriggerEnter(Collider collider)
	{
		if (collider == this.characterCollider)
		{
			this.particles.Play(); 
			this.miniGame.NotifyLostLife();
			this.collider.enabled = false;
			this.characterAudioSource.Play();
		}
	}
}
