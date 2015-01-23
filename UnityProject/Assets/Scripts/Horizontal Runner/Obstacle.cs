using UnityEngine;

public class Obstacle : MonoBehaviour 
{
	public Collider characterCollider = null;
	public ParticleSystem particles = null;
	public float leftEdgeDestructionThreshold = -11f;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (this.transform.localPosition.x < this.leftEdgeDestructionThreshold)
		{ Destroy(this.gameObject); }
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider == this.characterCollider)
		{ this.particles.Play(); }
	}
}
