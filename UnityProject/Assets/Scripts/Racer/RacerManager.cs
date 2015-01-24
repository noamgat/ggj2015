﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    public List<GameObject> obstaclePrefabs;
    public float minTimeToSpawn;
    public float maxTimeToSpawn;
    float lastTimeOfSpawn;
    float nextTimeOfSpawnInterval;
    public Transform leftSpawnBorder;
    public Transform rightSpawnBorder;

    public GameObject ground;

    void Start()
    {
        car.transform.position = rightPosition.position;
        targetPosition = car.transform.position;
        lastTimeOfSpawn = Time.time;
        nextTimeOfSpawnInterval = GetNextIntervalTime();
    }

	void Update () {
        // Set new position on click
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

        // Move towards new position
        car.transform.position = new Vector3(Mathf.Lerp(car.transform.position.x, targetPosition.x, Time.deltaTime * switchSpeed), car.transform.position.y, car.transform.position.z);

        // Generate new obstacles
        if (Time.time - lastTimeOfSpawn >= nextTimeOfSpawnInterval)
        {
            lastTimeOfSpawn = Time.time;
            nextTimeOfSpawnInterval = GetNextIntervalTime();
            GenerateNewObstacle();
        }
	}

    float GetNextIntervalTime()
    {
        return Random.Range(minTimeToSpawn, maxTimeToSpawn);
    }

    void GenerateNewObstacle()
    {
        Vector3 newPosition = new Vector3(
            Random.Range(leftSpawnBorder.position.x, rightSpawnBorder.position.x),
            Random.Range(leftSpawnBorder.position.y, rightSpawnBorder.position.y),
            Random.Range(leftSpawnBorder.position.z, rightSpawnBorder.position.z)
            );
        
        (Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)], newPosition, Quaternion.identity) as GameObject).transform.parent = ground.transform;
    }
}
