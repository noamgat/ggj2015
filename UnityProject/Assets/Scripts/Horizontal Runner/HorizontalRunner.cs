using System;
using UnityEngine;

public class HorizontalRunner : MonoBehaviour 
{
	public GameObject character = null;
	public Rigidbody characterRigidBody = null;
	public float jumpHeightModifier = 50f;
	public float maximumTimeOfJump = 1.5f; // in seconds
	public float minimumTimeOfJump = 0.5f;

	private float jumpTime;
	private float originalY;
	private bool jumping;
	private bool allowPress = true;
	private bool killJump;

	// Use this for initialization
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
	
	// Update is called once per frame
	// ReSharper disable once UnusedMember.Local
	void Update () 
	{
		this.killJump = this.jumpTime < 0 || Input.GetButtonUp("Fire1") || killJump;

		//Debug.Log(string.Format("Allowed: {0}; Jumping: {1}; KillJump: {5}; JumpTime: {2}\n" +
		//						"OriginalY: {3}; Y: {4};", 
		//						this.allowPress, this.jumping, this.jumpTime, this.originalY,
		//						this.character.transform.localPosition.y, killJump));

		// respond to click
		if (Input.GetButtonDown("Fire1") && this.jumpTime > 0 && this.allowPress)
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
	}
}
