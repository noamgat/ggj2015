using UnityEngine;
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

    public RectTransform livesContainer;
    public AudioSource audioFwoosh;
    public AudioSource audioHit;

    public Animator characterAnimation;
    public GroundRotator groundRotator;

    bool isBeingHit;
    float hitStarted;
    public float timeToStand;

    void Start()
    {
        car.transform.position = rightPosition.position;
        targetPosition = car.transform.position;
        GenerateNewObstacle();
    }

	void Update () {
        if (isBeingHit) HitUpdate(); else RunUpdate();
	}

    void RunUpdate()
    {
        // Set new position on click
        if (InputManager.GetButtonDown)
        {
            if (carPosition == Position.Left) carPosition = Position.Right; else carPosition = Position.Left;
            audioFwoosh.Play();
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
            GenerateNewObstacle();
        }
    }

    void HitUpdate()
    {
        if (timeToStand < (Time.time - hitStarted))
        {
            isBeingHit = false;
            groundRotator.speed = groundRotator.firstSpeed;
            characterAnimation.SetBool("Fall", false);
        }
    }

    float GetNextIntervalTime()
    {
        return Random.Range(minTimeToSpawn, maxTimeToSpawn);
    }

    void GenerateNewObstacle()
    {
        lastTimeOfSpawn = Time.time;
        nextTimeOfSpawnInterval = GetNextIntervalTime();
        Vector3 newPosition = new Vector3(
            (Random.Range(0,2) == 0 ? leftSpawnBorder.position.x : rightSpawnBorder.position.x),
            leftSpawnBorder.position.y, leftSpawnBorder.position.z
            );
        
        (Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)], newPosition, Quaternion.identity) as GameObject).transform.parent = ground.transform;
    }

    public void OnNumLivesChanged(int numLives)
    {
        for (int i = 0; i < livesContainer.childCount; i++)
        {
            livesContainer.GetChild(i).gameObject.SetActive(i < numLives);
        }
    }

    public void GotHit()
    {
        audioHit.Play();
        miniGame.NotifyLostLife();
        groundRotator.speed = 0;
        characterAnimation.SetBool("Fall", true);
        isBeingHit = true;
        hitStarted = Time.time;
    }
}
