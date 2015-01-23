using UnityEngine;
using System.Collections;

public class RacerManager : MonoBehaviour {

    public enum Position { Right, Left }

    Position carPosition = Position.Right;
    Position lastCarPosition = Position.Right;

    public GameObject car;

    public Vector3 targetPosition;
    public Transform leftPosition;
    public Transform rightPosition;

    public float switchSpeed;

    public MiniGame miniGame;

    void Start()
    {
        car.transform.position = rightPosition.position;
        targetPosition = car.transform.position;
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
                targetPosition = leftPosition.position;
            else
                targetPosition = rightPosition.position;
            lastCarPosition = carPosition;
        }

        car.transform.position = new Vector3(Mathf.Lerp(car.transform.position.x, targetPosition.x, Time.deltaTime * switchSpeed), car.transform.position.y, car.transform.position.z);
	}
}
