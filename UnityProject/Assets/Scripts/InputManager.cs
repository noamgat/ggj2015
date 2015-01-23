using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    public static bool GetButtonDown { get; private set; }
    public static bool GetButtonUp { get; private set; }
    public static bool GetButtonHeld { get; private set; }

	// Update is called once per frame
	void Update () {
        
        GetButtonDown = false;
        GetButtonUp = false;
        GetButtonHeld = false;


        if (Input.GetButtonDown("Fire1") || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            GetButtonDown = true;
        }

        if (Input.GetButtonUp("Fire1") || (Input.touchCount > 0 && ((Input.GetTouch(0).phase == TouchPhase.Ended) || (Input.GetTouch(0).phase == TouchPhase.Canceled))))
        {
            GetButtonUp = true;
        }

        if (Input.GetButton("Fire1") || (Input.touchCount > 0 && ((Input.GetTouch(0).phase == TouchPhase.Stationary) || (Input.GetTouch(0).phase == TouchPhase.Moved))))
        {
            GetButtonHeld = true;
        }
	}
}
