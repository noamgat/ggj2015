using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RepeatClickGameController : MonoBehaviour {

    public MiniGame miniGame;
    public Text timeLeftText;
    public float maxLife = 10f;
    public float clickLoseLifeSpeed = 7f;
    public float regainLifeSpeed = 2f;
    private float timeUntilLostLife;
    public float respawnDuration = 1.5f;
    public RectTransform livesContainer;
    public RectTransform hitContainer;
    public CanvasGroup warningContainer;

    //1 = Safe, 0 = Dead
    public float relativeSafety { get; private set; }

	// Use this for initialization
	void Start () {
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
        
        timeLeftText.text = (timeUntilLostLife / clickLoseLifeSpeed).ToString("0.0");
        relativeSafety = timeUntilLostLife / maxLife;
        warningContainer.alpha = 1 - relativeSafety;
	}

    private void UpdateDuringChase() {
        if (InputManager.GetButtonHeld) {
            timeUntilLostLife -= clickLoseLifeSpeed * Time.deltaTime;
        } else {
            timeUntilLostLife += regainLifeSpeed * Time.deltaTime;
            timeUntilLostLife = Mathf.Min(timeUntilLostLife, maxLife);
        }

        if (timeUntilLostLife <= 0) {
            hitContainer.gameObject.SetActive(true);
            this.ExecuteWithDelay(0.5f, delegate() { hitContainer.gameObject.SetActive(false); });
            miniGame.onLostLife.Invoke(miniGame);
            isRespawning = true;
        }
    }

    private void UpdateDuringRespawn() {
        timeUntilLostLife += maxLife * (Time.deltaTime / respawnDuration);
        if (timeUntilLostLife >= maxLife) {
            isRespawning = false;
        }
    }


    private void ResetTimer() {
        timeUntilLostLife = maxLife;	
    }

    public void OnNumLivesChanged(int numLives) {
        for (int i = 0; i < livesContainer.childCount; i++) {
            livesContainer.GetChild(i).gameObject.SetActive(i < numLives);
        }
    }
}
