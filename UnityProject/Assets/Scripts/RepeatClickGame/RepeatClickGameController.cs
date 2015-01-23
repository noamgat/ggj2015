using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RepeatClickGameController : MonoBehaviour {

    public MiniGame miniGame;
    public Text timeLeftText;
    public float initialTimeToLive = 10f;
    public float clickBoost = 3f;
    private float timeUntilLostLife;
    private float selectedTimeToLive;
    public float respawnDuration = 1.5f;
    public RectTransform livesContainer;

    //1 = Safe, 0 = Dead
    public float relativeSafety { get; private set; }

	// Use this for initialization
	void Start () {
        selectedTimeToLive = initialTimeToLive * Random.Range(1f, 2f);
        ResetTimer();
	}

    private bool isRespawning;

	// Update is called once per frame
	void Update () {
        if (isRespawning) {
            UpdateDuringRespawn();
        } else {
            UpdateDuringChase();
        }
        
        timeLeftText.text = (timeUntilLostLife / clickBoost).ToString("0.0");
        relativeSafety = timeUntilLostLife / selectedTimeToLive;
	}

    private void UpdateDuringChase() {
        if (InputManager.GetButtonHeld) {
            timeUntilLostLife -= clickBoost * Time.deltaTime;
        } else {
            timeUntilLostLife += Time.deltaTime;
            timeUntilLostLife = Mathf.Min(timeUntilLostLife, selectedTimeToLive);
        }

        if (timeUntilLostLife <= 0) {
            miniGame.onLostLife.Invoke(miniGame);
            isRespawning = true;
        }
    }

    private void UpdateDuringRespawn() {
        timeUntilLostLife += selectedTimeToLive * (Time.deltaTime / respawnDuration);
        if (timeUntilLostLife >= selectedTimeToLive) {
            isRespawning = false;
        }
    }


    private void ResetTimer() {
        timeUntilLostLife = selectedTimeToLive;	
    }

    public void OnNumLivesChanged(int numLives) {
        for (int i = 0; i < livesContainer.childCount; i++) {
            livesContainer.GetChild(i).gameObject.SetActive(i < numLives);
        }
    }
}
