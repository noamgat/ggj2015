using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndSceneLogic : MonoBehaviour {

    public static float score;

    public GameObject LeaderboardIcon;

    public RectTransform[] textHiders;
    public Text scoreText;
    public Text highScoreText;

    public float idleBlinkTime = 1f;
    public float selectBlinkTime = 0.12f;
    public int numBlinks = 16;
    private bool didPressButton;
    public float timeUntilCanPress = 3f;

    public RectTransform[] parts;
    private int currentPart;

    private float startMenuTime;
	// Use this for initialization
	void Start () {
        int finalscore = ((int)(score * 10));
        scoreText.text = finalscore.ToString();
        if (PlayerPrefs.HasKey("highscore") &&
            PlayerPrefs.GetInt("highscore") > finalscore)
        {
            highScoreText.text = PlayerPrefs.GetInt("highscore").ToString();
        }
        else
        {
            PlayerPrefs.SetInt("highscore", finalscore);
            highScoreText.text = finalscore.ToString();
        }
#if UNITY_ANDROID
        Social.ReportScore(finalscore,"CgkIqbal4rgKEAIQAA", (bool success) => {
        Debug.Log("Score of " + finalscore + " was posted successfully? " + success);
        LeaderboardIcon.SetActive(true);
    });
#endif
        SetCurrentPart(0);
	}

    void SetCurrentPart(int newCurrentPart) {
        startMenuTime = Time.realtimeSinceStartup;
        didPressButton = false;
        currentPart = newCurrentPart;
        for (int i = 0; i < parts.Length; i++) {
            parts[i].gameObject.SetActive(i == currentPart);
        }
    }

    void SetTextHidersVisible(bool visible) {
        for (int i = 0; i < textHiders.Length; i++) {
            textHiders[i].gameObject.SetActive(visible);
        }
    }

    // Update is called once per frame
    void Update() {
        if (Time.realtimeSinceStartup < startMenuTime + timeUntilCanPress) {
            SetTextHidersVisible(true);
            return;
        }
        if (!didPressButton) {
            bool shouldShowTextHider = Mathf.FloorToInt(Time.realtimeSinceStartup / idleBlinkTime) % 2 == 0;
            SetTextHidersVisible(shouldShowTextHider);
            if (InputManager.GetButtonUp) {
                didPressButton = true;
                for (int i = 0; i < numBlinks; i++) {
                    float delay = i * selectBlinkTime;
                    bool isVisible = (i % 2) == 1;
                    this.ExecuteWithDelay(delay, delegate() { SetTextHidersVisible(isVisible); });
                }
                float startDelay = numBlinks * selectBlinkTime;
                if (currentPart + 1 < parts.Length) {
                    this.ExecuteWithDelay(startDelay, delegate() { SetCurrentPart(currentPart+1); });
                } else {
                    this.ExecuteWithDelay(startDelay, delegate() { Application.LoadLevel("IntroScene"); });
                }
            }
        }
	}

    void OpenLeaderboardUI ()
    {
        Debug.Log("Opening leaderboards");
         Social.ShowAchievementsUI();
    }
}
