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
	// Use this for initialization
	void Start () {
        selectedTimeToLive = initialTimeToLive * Random.Range(1f, 2f);
        ResetTimer();
	}
	

	// Update is called once per frame
	void Update () {
        if (InputManager.GetButtonDown) {
            timeUntilLostLife += clickBoost;
            timeUntilLostLife = Mathf.Min(timeUntilLostLife, selectedTimeToLive);
            return;
        }
        timeUntilLostLife -= Time.deltaTime;
        if (timeUntilLostLife < 0) {
            miniGame.onLostLife.Invoke(miniGame);
            ResetTimer();
        }
        timeLeftText.text = timeUntilLostLife.ToString("0.0");
	}


    private void ResetTimer() {
        timeUntilLostLife = selectedTimeToLive;	
    }
}
