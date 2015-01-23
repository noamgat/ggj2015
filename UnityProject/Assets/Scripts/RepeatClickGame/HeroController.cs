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
}
