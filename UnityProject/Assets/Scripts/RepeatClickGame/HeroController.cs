using UnityEngine;
using System.Collections;

public class HeroController : MonoBehaviour {

    public Transform hero;
    public float safeX;
    public float deadX;
    public RepeatClickGameController gameController;
    public float safeScale = 3f;
    public float deadScale = 1.25f;
    public Transform heroAvatar;

	// Update is called once per frame
	void LateUpdate () {
        Vector3 localPos = hero.localPosition;
        float relativePosition = gameController.relativeSafety;
        localPos.x = Mathf.Lerp(deadX, safeX, relativePosition);
        hero.localPosition = localPos;
        float scale = Mathf.Lerp(deadScale, safeScale, relativePosition * relativePosition);
        heroAvatar.localScale = new Vector3(scale, scale, scale);
	}

    public void OnLostLife() {
        for (int i = 0; i < 12; i++) {
            float delay = i * 0.12f;
            bool isVisible = (i % 2) == 1;
            this.ExecuteWithDelay(delay, delegate() { hero.gameObject.SetActive(isVisible); });
        }
    }

    
}
