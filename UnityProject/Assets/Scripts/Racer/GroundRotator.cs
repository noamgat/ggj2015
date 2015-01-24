using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundRotator : MonoBehaviour {

    public Transform rotatePoint;
    public float speed;
    float currentSpeed;

    public float firstSpeed { get; private set; }

    public Transform rotateToPoint;

    List<Transform> groundObjects = new List<Transform>();
    Vector3 startPos;
    Vector3 endPos;

    void Awake()
    {
        startPos = rotateToPoint.position;
        endPos = rotatePoint.position;
        foreach (Transform child in transform)
        {
            groundObjects.Add(child);
        }
        currentSpeed = 0;
        firstSpeed = speed;
    }

    void Update()
    {
        if (currentSpeed != speed) currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.deltaTime * 10);
        foreach (Transform obj in transform)
        {
            obj.Translate(0, 0, -1 * currentSpeed * Time.deltaTime);
            if (obj.position.z <= endPos.z)
            {
                if (obj.GetComponent<DestroyOnRotate>() != null)
                {
                    Destroy(obj.gameObject);
                }
                else
                {
                    obj.position = startPos - new Vector3(-obj.position.x, -obj.position.y, endPos.z - obj.position.z);
                }
            }
        }
    }
}
