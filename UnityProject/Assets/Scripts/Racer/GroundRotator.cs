using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GroundRotator : MonoBehaviour {

    public Transform rotatePoint;
    public float speed;

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
    }

    void Update()
    {
        foreach (Transform obj in groundObjects)
        {
            obj.Translate(0, 0, -1 * speed * Time.deltaTime);
            if (obj.position.z <= endPos.z)
            {
                obj.position = startPos - new Vector3(-obj.position.x,-obj.position.y,endPos.z - obj.position.z);
            }
        }
    }
}
