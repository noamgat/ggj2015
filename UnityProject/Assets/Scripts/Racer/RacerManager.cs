using UnityEngine;
using System.Collections;

public class RacerManager : MonoBehaviour {

    public enum Position { Right, Left }

    Position carPosition = Position.Right;
    Position lastCarPosition = Position.Right;

    public GameObject car;

    public Transform leftPosition;
    public Transform rightPosition;

    void Start()
    {
        car.transform.position = rightPosition.position;
    }

	// Update is called once per frame
	void Update () {
        if (InputManager.GetButtonDown)
        {
            if (carPosition == Position.Left) carPosition = Position.Right; else carPosition = Position.Left;
        }

        if (carPosition != lastCarPosition)
        {
            if (carPosition == Position.Left)
                car.transform.position = leftPosition.position;
            else
                car.transform.position = rightPosition.position;
            lastCarPosition = carPosition;
        }
	}
}
