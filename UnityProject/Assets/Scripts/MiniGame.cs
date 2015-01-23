using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class MiniGame : MonoBehaviour {
    public Camera mainCamera;

    [System.Serializable]
    public class MiniGameEvent : UnityEvent<MiniGame> { }
    public MiniGameEvent onLostLife;

    [System.Serializable]
    public class LifeChangeEvent : UnityEvent<int> { }
    public LifeChangeEvent onNumLivesChanged;

    public void NotifyLifeTotalChanged(int numLivesLeft) {
        onNumLivesChanged.Invoke(numLivesLeft);
    }

    public void NotifyLostLife() {
        onLostLife.Invoke(this);
    }


}
