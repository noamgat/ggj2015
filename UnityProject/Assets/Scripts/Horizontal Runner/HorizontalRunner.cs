using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class HorizontalRunner : MonoBehaviour
{
	#region public fields
	public Camera ownCamera = null;
	public MiniGame miniGame = null;
	public RectTransform livesContainer = null;
	public GameObject character = null;
	public Rigidbody characterRigidBody = null;
	public Animator characterAnimator = null;
	public AudioSource characterHitAudioSource = null;
	public AudioSource characterJumpAudioSource = null;
	public GameObject[] obstaclePrefabs = null;
	public float obstacleStart = 26f;
	public float jumpHeightModifier = 200f;
	public float maximumTimeOfJump = 0.3f; // in seconds
	public float minimumTimeOfJump = 0.1f;
	public int obstacleCheckMin = 1000; // in milliseconds
	public int obstacleCheckMax = 5000;
	public int perObstacleMaxDegredationRate = 100;
	public float obstacleSpeed = 5f;
	public float characterFlickerTime = 1f;
	public float characterFlickerFrequency = 0.1f;
	#endregion

	#region private fields
	private float jumpTime;
	private float originalY;
	private bool jumping;
	private bool allowPress = true;
	private bool killJump;
	private long lastTime;
	#endregion

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

	private void HandleJump()
	{
		this.killJump = this.jumpTime < 0 || !InputManager.GetButtonHeld || this.killJump;

		if (InputManager.GetButtonDown)
		{ this.characterJumpAudioSource.Play(); }

		//Debug.Log(string.Format("Allowed: {0}; Jumping: {1}; KillJump: {5}; JumpTime: {2}\n" +
		//						"OriginalY: {3}; Y: {4};", 
		//						this.allowPress, this.jumping, this.jumpTime, this.originalY,
		//						this.character.transform.localPosition.y, killJump));

		// respond to click
		if (InputManager.GetButtonHeld && this.jumpTime > 0 && this.allowPress)
		{
			this.jumping = true;
			this.characterAnimator.SetTrigger("Jump");
		}

		// kill the jump
		if (this.killJump && this.jumpTime + this.minimumTimeOfJump < this.maximumTimeOfJump)
		{
			this.jumping = false;
			this.allowPress = false;
			this.characterAnimator.ResetTrigger("Jump");
		}

		if (Math.Sign(this.characterRigidBody.velocity.y) == -1)
		{ this.characterAnimator.SetBool("Descending", true); }

		// when the character lands, enable jump again
		if (Math.Abs(this.character.transform.localPosition.y - this.originalY) < 0.001)
		{
			this.jumpTime = this.maximumTimeOfJump;
			this.allowPress = true;
			this.killJump = false;
			this.characterAnimator.SetBool("Descending", false);
		}
	}

	private void RandomObstacle()
	{
		if (DateTime.Now.ToFileTimeUtc() - this.lastTime < 0)
		{ return; }

		int obstacleNumber = Random.Range(0, this.obstaclePrefabs.Length - 1);
		GameObject obstacleObject = (GameObject)Instantiate(this.obstaclePrefabs[obstacleNumber]);
		obstacleObject.transform.parent = this.transform;
		obstacleObject.transform.localPosition = new Vector3(this.obstacleStart, obstacleObject.transform.localPosition.y);
		obstacleObject.GetComponent<Rigidbody>().AddForce(Vector3.left * this.obstacleSpeed, ForceMode.VelocityChange);
		Obstacle obstacle = obstacleObject.GetComponent<Obstacle>();
		obstacle.characterCollider = this.character.collider;
		obstacle.miniGame = this.miniGame;
		obstacle.characterAudioSource = this.characterHitAudioSource;

		this.lastTime = DateTime.Now.ToFileTimeUtc() + Random.Range(this.obstacleCheckMin, this.obstacleCheckMax) * 10000;
		this.obstacleCheckMax -= this.perObstacleMaxDegredationRate;
		if (this.obstacleCheckMax < 0)
		{ this.obstacleCheckMax = 0; }
		if (this.obstacleCheckMax < this.obstacleCheckMin)
		{ this.obstacleCheckMin = this.obstacleCheckMax; }
	}

	// ReSharper disable once UnusedMember.Local
	void Update ()
	{
		this.HandleJump();

		this.RandomObstacle();

		this.ownCamera.transform.localPosition = new Vector3((1f - this.ownCamera.rect.height) * 14f, 4f, -10f);
	}

	// ReSharper disable once UnusedMember.Global
	public void OnNumLivesChanged(int numLives)
	{
		for (int i = 0; i < this.livesContainer.childCount; i++)
		{ this.livesContainer.GetChild(i).gameObject.SetActive(i < numLives); }
	}

	private IEnumerator FlickerCharacter()
	{
		SpriteRenderer spriteRenderer = this.character.GetComponentInChildren<SpriteRenderer>();
		float flickerTime = this.characterFlickerTime;

		while (flickerTime > 0)
		{
			spriteRenderer.color = Mathf.RoundToInt(flickerTime / this.characterFlickerFrequency) % 2 == 0 ?
									Color.white : Color.red;
			flickerTime -= Time.deltaTime;
			yield return null;
		}

		this.character.GetComponentInChildren<SpriteRenderer>().color = Color.white;
	}

	// ReSharper disable once UnusedMember.Global
	public void OnLostLife()
	{
		Debug.Log("Horizontal Runner: Lost Life.");
		StopCoroutine(this.FlickerCharacter());
		this.character.GetComponentInChildren<SpriteRenderer>().color = Color.white;
		StartCoroutine(this.FlickerCharacter());
	}
}
