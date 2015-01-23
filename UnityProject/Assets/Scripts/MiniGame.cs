using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class MiniGame : MonoBehaviour {
    public Camera camera;

    [System.Serializable]
    public class MiniGameEvent : UnityEvent<MiniGame> { }
    public MiniGameEvent onLostLife;
}
