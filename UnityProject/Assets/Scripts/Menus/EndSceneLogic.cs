using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndSceneLogic : MonoBehaviour {

    public static int score;

    public RectTransform textHider;
    public Text scoreText;

    public float idleBlinkTime = 1f;
    public float selectBlinkTime = 0.12f;
    public int numBlinks = 16;
    private bool didPressButton;
    public float timeUntilCanPress = 3f;

    private float startMenuTime;
	// Use this for initialization
	void Start () {
        didPressButton = false;
        scoreText.text = score.ToString();
        startMenuTime = Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.realtimeSinceStartup < startMenuTime + timeUntilCanPress) {
            return;
        }
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
                this.ExecuteWithDelay(startDelay, delegate() { Application.LoadLevel("IntroScene"); });
            }
        }
	}
}
