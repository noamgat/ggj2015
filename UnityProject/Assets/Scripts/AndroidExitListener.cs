using UnityEngine;
using System.Collections;

public class AndroidExitListener : MonoBehaviour {

    private static AndroidExitListener instance = null;

    void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
    }
}
