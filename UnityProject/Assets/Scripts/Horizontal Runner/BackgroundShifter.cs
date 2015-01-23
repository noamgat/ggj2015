using UnityEngine;

// ReSharper disable once UnusedMember.Global
public class BackgroundShifter : MonoBehaviour 
{
	[Tooltip("This is the one on the right.")]
	public Transform leadBackground;
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
		this.followingBackground.localPosition += Vector3.left * Time.deltaTime * this.transitSpeedModifier;

		if (this.leadBackground.localPosition.x < this.leadBackgroundXThreshold)
		{
			this.followingBackground.localPosition += new Vector3(this.shiftJumpLength, 0f);
			Transform tempHolder = this.followingBackground;
			this.followingBackground = this.leadBackground;
			this.leadBackground = tempHolder;
		}
	}
}
