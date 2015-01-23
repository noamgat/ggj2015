using UnityEngine;
using System.Collections;

public class MultiGame : MonoBehaviour {

    public MiniGame[] miniGamePrefabs;
    private MiniGame[] miniGames;
    public int numLives = 5;
    public Rect[] minigameViewports;

	// Use this for initialization
	void Start () {
        if (miniGamePrefabs.Length != minigameViewports.Length) {
            Debug.LogError("Minigame <-> viewport config mismatch");
        }
        miniGames = new MiniGame[miniGamePrefabs.Length];
        for (int i = 0; i < miniGames.Length; i++) {
            miniGames[i] = GameObject.Instantiate(miniGamePrefabs[i]) as MiniGame;
            miniGames[i].mainCamera.rect = minigameViewports[i];
            miniGames[i].onLostLife.AddListener(this.OnMiniGameLostLife);
            miniGames[i].NotifyLifeTotalChanged(numLives);
        }
	}

    private void OnMiniGameLostLife(MiniGame miniGame) {
        numLives--;
        Debug.Log("Lost life!");
        for (int i = 0; i < miniGames.Length; i++) {
            miniGames[i].NotifyLifeTotalChanged(numLives);
        }
        if (numLives == 0) {
            Debug.Log("You Lose!");
            Time.timeScale = 0;
        }
    }
}
