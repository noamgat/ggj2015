using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiGame : MonoBehaviour {

    public MiniGame[] miniGamePrefabs;
    private MiniGame[] miniGames;
    public int numLives = 5;
    public float transitionDuration = 2f;

    [System.Serializable]
    public class StageConfiguration
    {
        public Rect[] minigameViewports;
        public float timeInStage;
    }

    public StageConfiguration[] stageConfigurations;
    private float startTime;
    private float currentStage;

	// Use this for initialization
	void Start () {
        startTime = Time.realtimeSinceStartup;
        miniGames = new MiniGame[miniGamePrefabs.Length];
        currentStage = GetCurrentStage();
        UpdateStagesFromConfiguration();
	}

    private float GetCurrentStage() {
        float timeLeftToCount = Time.realtimeSinceStartup - startTime;
        int baseStage = 0;
        float currentStage = 0;
        while (baseStage < stageConfigurations.Length - 1) {
            if (timeLeftToCount < stageConfigurations[baseStage].timeInStage) {
                break;
            }
            timeLeftToCount -= stageConfigurations[baseStage].timeInStage;
            if (timeLeftToCount > transitionDuration) {
                currentStage += 1f;
                baseStage++;
                timeLeftToCount -= transitionDuration;
            } else {
                currentStage += (timeLeftToCount / transitionDuration);
                break;
            }
        }
        return currentStage;
    }
    void Update() {
        float newCurrentStage = GetCurrentStage();
        if (!Mathf.Approximately(currentStage, newCurrentStage)) {
            currentStage = newCurrentStage;
            UpdateStagesFromConfiguration();
        }
    }

    private void UpdateStagesFromConfiguration() {
        int baseStage = Mathf.FloorToInt(currentStage);
        bool inTransition = !Mathf.Approximately(baseStage, currentStage);
        StageConfiguration config = stageConfigurations[baseStage];
        for (int i = 0; i < config.minigameViewports.Length; i++) {
            if (miniGames[i] == null) {
                miniGames[i] = GameObject.Instantiate(miniGamePrefabs[i]) as MiniGame;
                miniGames[i].mainCamera.rect = config.minigameViewports[i];
                miniGames[i].onLostLife.AddListener(this.OnMiniGameLostLife);
                miniGames[i].NotifyLifeTotalChanged(numLives);
            } else {
                Rect rect;
                if (inTransition) {
                    float relativeProgress = Mathf.Repeat(currentStage, 1f);
                    Rect srcRect = config.minigameViewports[i];
                    Rect dstRect = stageConfigurations[baseStage + 1].minigameViewports[i];
                    rect = new Rect(Mathf.Lerp(srcRect.xMin, dstRect.xMin, relativeProgress),
                            Mathf.Lerp(srcRect.yMin, dstRect.yMin, relativeProgress),
                            Mathf.Lerp(srcRect.width, dstRect.width, relativeProgress),
                            Mathf.Lerp(srcRect.height, dstRect.height, relativeProgress));
                } else {
                    rect = config.minigameViewports[i];
                }
                miniGames[i].mainCamera.rect = rect;
            }
        }
    }

    private void OnMiniGameLostLife(MiniGame miniGame) {
        numLives--;
        Debug.Log("Lost life!");
        for (int i = 0; i < miniGames.Length; i++) {
            if (miniGames[i] != null) miniGames[i].NotifyLifeTotalChanged(numLives);
        }
        if (numLives == 0) {
            Debug.Log("You Lose!");
            Time.timeScale = 0;
        }
    }
}
