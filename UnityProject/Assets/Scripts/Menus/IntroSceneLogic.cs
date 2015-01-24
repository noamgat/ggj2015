using UnityEngine;
using System.Collections;

public class IntroSceneLogic : MonoBehaviour {

    public RectTransform textHider;

    public float idleBlinkTime = 1f;
    public float selectBlinkTime = 0.12f;
    public int numBlinks = 16;
    private bool didPressButton;
    
	// Use this for initialization
	void Start () {
        didPressButton = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!didPressButton) {
            bool shouldShowTextHider = Mathf.FloorToInt(Time.realtimeSinceStartup / idleBlinkTime) % 2 == 0;
            textHider.gameObject.SetActive(shouldShowTextHider);
            if (InputManager.GetButtonDown) {
                didPressButton = true;
                for (int i = 0; i < numBlinks; i++) {
                    float delay = i * selectBlinkTime;
                    bool isVisible = (i % 2) == 1;
                    this.ExecuteWithDelay(delay, delegate() { textHider.gameObject.SetActive(isVisible); });
                }
                float startDelay = numBlinks * selectBlinkTime;
                this.ExecuteWithDelay(startDelay, delegate() { Application.LoadLevel("MultiGameScene"); });
            }
        }
	}
}
