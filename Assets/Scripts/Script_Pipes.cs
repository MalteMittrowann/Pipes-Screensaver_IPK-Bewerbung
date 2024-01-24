using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Pipes : MonoBehaviour
{
    // Public variables:
    public GameObject sphere;
    public GameObject cylinder;

    public int minimalBoundary = -20;
    public int maximumBoundary = 20;

    public float secondsToSpawnNewPipe = 1.0f;
    public float secondsToRenewTheScene = 30.0f;

    // Private variables:
    private Vector3 currentPosition = new Vector3(0,0,0);
    private int nextDirection;
    private int previousDirection = -1; // set up as not in range of the "nextDirection"-value

    void Start()
    {
        nextDirection = Random.Range(0,6); // Random Number from 0 (incl.) to 6 (excl.): for the 6 possible directions
        Vector3 startSpawnPosition = new Vector3 (Random.Range(-10, 11), Random.Range(-5, 6), Random.Range(0, 21));
        currentPosition = startSpawnPosition;
        GameObject startSphere = Instantiate(sphere, startSpawnPosition, Quaternion.identity, this.transform);
        InvokeRepeating("CreatePipes", 0.5f, secondsToSpawnNewPipe);
        InvokeRepeating("StartNew", 0, secondsToRenewTheScene);
    }

    void CreatePipes ()
    {
        switch (nextDirection)
        {
            case 0:
                GoPositiveX(Random.Range(1, maximumBoundary - (int) currentPosition.x + 1));
                break;
            case 1:
                GoNegativeX(Random.Range(1, minimalBoundary - (int) currentPosition.x + 1));
                break;
            case 2:
                GoPositiveY(Random.Range(1, maximumBoundary - (int) currentPosition.y + 1));
                break;
            case 3:
                GoNegativeY(Random.Range(1, minimalBoundary - (int) currentPosition.y + 1));
                break;
            case 4:
                GoPositiveZ(Random.Range(1, maximumBoundary - (int) currentPosition.z + 1));
                break;
            case 5:
                GoNegativeZ(Random.Range(1, minimalBoundary - (int) currentPosition.z + 1));
                break;
        }
    }

    void GoPositiveX (int length)
    {
        // nextDirection = 0

        GameObject pipe = Instantiate(cylinder, new Vector3(currentPosition.x + (length / 2.0f), currentPosition.y, currentPosition.z), Quaternion.Euler(new Vector3(0, 0, 270)), this.transform) as GameObject;
        pipe.transform.localScale = new Vector3(1, (length / 2.0f), 1);
        Instantiate(sphere, new Vector3(currentPosition.x + length, currentPosition.y, currentPosition.z), Quaternion.identity, this.transform);
        currentPosition += new Vector3(length, 0, 0);
        nextDirection = Random.Range(2,6);
    }

    void GoNegativeX (int length)
    {
        // nextDirection = 1

        GameObject pipe = Instantiate(cylinder, new Vector3(currentPosition.x - (length / 2.0f), currentPosition.y, currentPosition.z), Quaternion.Euler(new Vector3(0, 0, 90)), this.transform);
        pipe.transform.localScale = new Vector3(1, (length / 2.0f), 1);
        Instantiate(sphere, new Vector3(currentPosition.x - length, currentPosition.y, currentPosition.z), Quaternion.identity, this.transform);
        currentPosition += new Vector3(-length, 0, 0);
        nextDirection = Random.Range(2,6);
    }

    void GoPositiveY (int length)
    {
        // nextDirection = 2
        
        GameObject pipe = Instantiate(cylinder, new Vector3(currentPosition.x, currentPosition.y + (length / 2.0f), currentPosition.z), Quaternion.identity, this.transform);
        pipe.transform.localScale = new Vector3(1, (length / 2.0f), 1);
        Instantiate(sphere, new Vector3(currentPosition.x, currentPosition.y + length, currentPosition.z), Quaternion.identity, this.transform);
        currentPosition += new Vector3(0, length, 0);
        int randomNumber = Random.Range(0,2);
        if (randomNumber == 0) {
            nextDirection = Random.Range(0,2);
        } else {
            nextDirection = Random.Range(4,6);
        }
    }

    void GoNegativeY (int length)
    {
        // nextDirection = 3

        GameObject pipe = Instantiate(cylinder, new Vector3(currentPosition.x, currentPosition.y - (length / 2.0f), currentPosition.z), Quaternion.identity, this.transform);
        pipe.transform.localScale = new Vector3(1, (length / 2.0f), 1);
        Instantiate(sphere, new Vector3(currentPosition.x, currentPosition.y - length, currentPosition.z), Quaternion.identity, this.transform);
        currentPosition += new Vector3(0, -length, 0);
        int randomNumber = Random.Range(0,2);
        if (randomNumber == 0) {
            nextDirection = Random.Range(0,2);
        } else {
            nextDirection = Random.Range(4,6);
        }
    }

    void GoPositiveZ (int length)
    {
        // nextDirection = 4

        GameObject pipe = Instantiate(cylinder, new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + (length / 2.0f)), Quaternion.Euler(new Vector3(270, 0, 0)), this.transform);
        pipe.transform.localScale = new Vector3(1, (length / 2.0f), 1);
        Instantiate(sphere, new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + length), Quaternion.identity, this.transform);
        currentPosition += new Vector3(0, 0, length);
        nextDirection = Random.Range(0,4);
    }

    void GoNegativeZ (int length)
    {
        // nextDirection = 5

        GameObject pipe = Instantiate(cylinder, new Vector3(currentPosition.x, currentPosition.y, currentPosition.z - (length / 2.0f)), Quaternion.Euler(new Vector3(90, 0, 0)), this.transform);
        pipe.transform.localScale = new Vector3(1, (length / 2.0f) + 1, 1);
        Instantiate(sphere, new Vector3(currentPosition.x, currentPosition.y, currentPosition.z - length), Quaternion.identity, this.transform);
        currentPosition += new Vector3(0, 0, -length);
        nextDirection = Random.Range(0,4);
    }

    void StartNew ()
    {

    }
}
