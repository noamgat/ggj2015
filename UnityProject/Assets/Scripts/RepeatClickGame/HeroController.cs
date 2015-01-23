using UnityEngine;
using System.Collections;

public class HeroController : MonoBehaviour {

    public Transform hero;
    public float safeX;
    public float deadX;
    public RepeatClickGameController gameController;

	// Update is called once per frame
	void LateUpdate () {
        Vector3 localPos = hero.localPosition;
        localPos.x = Mathf.Lerp(deadX, safeX, gameController.relativeSafety);
        hero.localPosition = localPos;
	}

    public void OnLostLife() {
        for (int i = 0; i < 8; i++) {
            float delay = i * 0.12f;
            bool isVisible = (i % 2) == 1;
            ExecuteWithDelay(delay, delegate() { hero.gameObject.SetActive(isVisible); });
        }
    }

    private void ExecuteWithDelay(float seconds, System.Action action) {
        StartCoroutine(ExecuteWithDelayCorutine(seconds, action));
    }

    private IEnumerator ExecuteWithDelayCorutine(float seconds, System.Action action) {
        yield return new WaitForSeconds(seconds);
        action();
    }
}
