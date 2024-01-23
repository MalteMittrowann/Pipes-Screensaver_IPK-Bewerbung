using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Pipes : MonoBehaviour
{
    // Public variables:
    public GameObject sphere;
    public GameObject cylinder;

    public int minimalTransform = -20;
    public int maximumTransform = 20;

    public int secondsToSpawnNewPipe = 1;
    public int secondsToRenewTheScene = 30;

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
        InvokeRepeating("CreatePipes", 0, secondsToSpawnNewPipe);
        InvokeRepeating("StartNew", 0, secondsToRenewTheScene);
    }

    void CreatePipes ()
    {
        switch (nextDirection)
        {
            case 0:
                GoPositiveX();
                break;
            case 1:
                GoNegativeX();
                break;
            case 2:
                GoPositiveY();
                break;
            case 3:
                GoNegativeY();
                break;
            case 4:
                GoPositiveZ();
                break;
            case 5:
                GoNegativeZ();
                break;
        }
    }

    void GoPositiveX ()
    {
        // nextDirection = 0
        Instantiate(cylinder, new Vector3(currentPosition.x + 1, currentPosition.y, currentPosition.z), Quaternion.Euler(new Vector3(0, 0, 270)), this.transform);
        nextDirection = Random.Range(2,6);
    }

    void GoNegativeX ()
    {
        // nextDirection = 1
        Instantiate(cylinder, new Vector3(currentPosition.x - 1, currentPosition.y, currentPosition.z), Quaternion.Euler(new Vector3(0, 0, 90)), this.transform);
        nextDirection = Random.Range(2,6);
    }

    void GoPositiveY ()
    {
        // nextDirection = 2
        Instantiate(cylinder, new Vector3(currentPosition.x, currentPosition.y + 1, currentPosition.z), Quaternion.identity, this.transform);
        int randomNumber = Random.Range(0,2);
        if (randomNumber == 0) {
            nextDirection = Random.Range(0,2);
        } else {
            nextDirection = Random.Range(4,6);
        }
    }

    void GoNegativeY ()
    {
        // nextDirection = 3
        Instantiate(cylinder, new Vector3(currentPosition.x, currentPosition.y - 1, currentPosition.z), Quaternion.identity, this.transform);
        int randomNumber = Random.Range(0,2);
        if (randomNumber == 0) {
            nextDirection = Random.Range(0,2);
        } else {
            nextDirection = Random.Range(4,6);
        }
    }

    void GoPositiveZ ()
    {
        // nextDirection = 4
        Instantiate(cylinder, new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + 1), Quaternion.Euler(new Vector3(270, 0, 0)), this.transform);
        nextDirection = Random.Range(0,4);
    }

    void GoNegativeZ ()
    {
        // nextDirection = 5
        Instantiate(cylinder, new Vector3(currentPosition.x, currentPosition.y, currentPosition.z - 1), Quaternion.Euler(new Vector3(90, 0, 0)), this.transform);
        nextDirection = Random.Range(0,4);
    }

    void StartNew ()
    {

    }
}
