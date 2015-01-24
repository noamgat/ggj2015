using UnityEngine;

// ReSharper disable once UnusedMember.Global
public class BackgroundShifter : MonoBehaviour 
{
	[Tooltip("This is the one on the right.")]
	public Transform leadBackground;
	public Transform midBackground;
	[Tooltip("This is the one on the left.")]
	public Transform followingBackground;
	[Tooltip("At one point of the lead is the following out of view.")]
	public float leadBackgroundXThreshold = 5f;
	public float transitSpeedModifier = 1f;
	public float shiftJumpLength = 27.93f;

	// ReSharper disable once UnusedMember.Local
	void Update () 
	{
		this.leadBackground.localPosition += Vector3.left * Time.deltaTime * this.transitSpeedModifier;
		this.midBackground.localPosition += Vector3.left * Time.deltaTime * this.transitSpeedModifier;
		this.followingBackground.localPosition += Vector3.left * Time.deltaTime * this.transitSpeedModifier;

		if (this.midBackground.localPosition.x < this.leadBackgroundXThreshold)
		{
			this.followingBackground.localPosition += Vector3.right * this.shiftJumpLength * 3f;
			Transform tempHolder = this.followingBackground;
			this.followingBackground = this.midBackground;
			this.midBackground = this.leadBackground;
			this.leadBackground = tempHolder;
		}
	}
}
