﻿using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class IntroSceneLogic : MonoBehaviour {

    public RectTransform textHider;
    public static bool isAuthenticated = false;

    public float idleBlinkTime = 1f;
    public float selectBlinkTime = 0.12f;
    public int numBlinks = 16;
    private bool didPressButton;
    
	// Use this for initialization
	void Start () {
        didPressButton = false;
        #if UNITY_ANDROID //&& !UNITY_EDITOR
        if (!isAuthenticated)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();

            Social.localUser.Authenticate(delegate(bool success) { Debug.Log("Authentication: " + success); isAuthenticated = true; });
        }
#endif
	}
	
	// Update is called once per frame
	void Update () {
        if (!didPressButton) {
            bool shouldShowTextHider = Mathf.FloorToInt(Time.realtimeSinceStartup / idleBlinkTime) % 2 == 0;
            textHider.gameObject.SetActive(shouldShowTextHider);
            if (InputManager.GetButtonDown) {
                didPressButton = true;
                audio.Play();
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
