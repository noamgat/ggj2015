using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class HorizontalRunner : MonoBehaviour 
{
	public GameObject character = null;
	public Rigidbody characterRigidBody = null;
	public MiniGame miniGame = null;
	public GameObject obstaclePrefab = null;
	public Vector3 obstacleStart = new Vector3(4f, 0.98f, 0f);
	public float jumpHeightModifier = 200f;
	public float maximumTimeOfJump = 0.3f; // in seconds
	public float minimumTimeOfJump = 0.1f;
	public int obstacleCheck = 1000; // in milliseconds
	public float obstacleThreshold = 0.8f;
	public float obstacleSpeed = 5f;

	private float jumpTime;
	private float originalY;
	private bool jumping;
	private bool allowPress = true;
	private bool killJump;

	private long lastTime = 0;

	// ReSharper disable once UnusedMember.Local
	void Start () 
	{
		this.originalY = this.character.transform.localPosition.y;
		this.jumpTime = this.maximumTimeOfJump;
	}

	// ReSharper disable once UnusedMember.Local
	void FixedUpdate()
	{
		if (jumping)
		{
			this.jumpTime -= Time.deltaTime;
			this.characterRigidBody.AddForce(Vector3.up * this.jumpHeightModifier * this.jumpTime, ForceMode.Force);
		}
	}
	
	void RandomObstacle()
	{
		float randomChance = Random.Range(0f, 1f);
		if (DateTime.Now.ToFileTimeUtc() - this.lastTime < this.obstacleCheck * 10000 || randomChance < this.obstacleThreshold)
		{ return; }
		//Debug.Log("Random: " + randomChance + "; Time Difference: " + (DateTime.Now.ToFileTimeUtc() - this.lastTime) / 10000);

		GameObject obstacle = (GameObject)Instantiate(this.obstaclePrefab);
		float randomSize = Random.Range(0.5f, 2f);
		obstacle.transform.localScale = new Vector3(randomSize, randomSize);
		obstacle.transform.localPosition = this.obstacleStart - new Vector3(0f, (1f - randomSize) / 2);
		obstacle.GetComponent<Rigidbody>().AddForce(Vector3.left * this.obstacleSpeed, ForceMode.VelocityChange);
		obstacle.GetComponent<Obstacle>().characterCollider = this.character.collider;

		this.lastTime = DateTime.Now.ToFileTimeUtc();
	}

	// ReSharper disable once UnusedMember.Local
	void Update () 
	{
		this.killJump = this.jumpTime < 0 || InputManager.GetButtonUp || killJump;

		//Debug.Log(string.Format("Allowed: {0}; Jumping: {1}; KillJump: {5}; JumpTime: {2}\n" +
		//						"OriginalY: {3}; Y: {4};", 
		//						this.allowPress, this.jumping, this.jumpTime, this.originalY,
		//						this.character.transform.localPosition.y, killJump));

		// respond to click
		if (InputManager.GetButtonDown && this.jumpTime > 0 && this.allowPress)
		{ this.jumping = true; }

		// kill the jump
		if (this.killJump && this.jumpTime + this.minimumTimeOfJump < this.maximumTimeOfJump)
		{
			this.jumping = false;
			this.allowPress = false;
		}
		
		// when the character lands, enable jump again
		if (Math.Abs(this.character.transform.localPosition.y - this.originalY) < 0.001)
		{
			this.jumpTime = this.maximumTimeOfJump;
			this.allowPress = true;
			this.killJump = false;
		}

		this.RandomObstacle();
	}
}
