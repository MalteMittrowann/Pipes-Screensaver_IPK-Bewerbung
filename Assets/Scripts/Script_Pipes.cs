using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Pipes : MonoBehaviour
{
    // Public variables:
    public GameObject sphere;
    public GameObject cylinder;

    public int minimalBoundaryX = -20;
    public int maximumBoundaryX = 20;
    public int minimalBoundaryY = -10;
    public int maximumBoundaryY = 10;
    public int minimalBoundaryZ = 0;
    public int maximumBoundaryZ = 20;

    public float secondsToSpawnNewPipe = 0.25f;
    public float secondsToRenewTheScene = 30.0f;

    // Private variables:
    private Vector3 currentPosition = new Vector3(0,0,0);
    private int nextDirection;
    private int previousDirection = -1; // set up as not in range of the "nextDirection"-value

    void Start()
    {
        nextDirection = Random.Range(0,6); // Random Number from 0 (incl.) to 6 (excl.): for the 6 possible directions
        Vector3 startSpawnPosition = new Vector3 (Random.Range(-minimalBoundaryX, maximumBoundaryX), Random.Range(minimalBoundaryY, maximumBoundaryY), Random.Range(minimalBoundaryZ, maximumBoundaryZ)); // Create random startPosition for the first sphere
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
                // Go in the positive X direction
                if(currentPosition.x >= maximumBoundaryX - 2) {
                    // if already at the boudary --> change direction 180°
                    nextDirection = 1;
                    CreatePipes();
                } else {
                    GoPositiveX(Random.Range(3, maximumBoundaryX - (int) currentPosition.x + 1));
                }
                break;
            case 1:
                // Go in the negative X direction
                if(currentPosition.x <= minimalBoundaryX + 2) {
                    // if already at the boudary --> change direction 180°
                    nextDirection = 0;
                    CreatePipes();
                } else {
                    GoNegativeX(Random.Range(minimalBoundaryX - (int) currentPosition.x, -2));
                }
                break;
            case 2:
                // Go in the positive Y direction
                if(currentPosition.y >= maximumBoundaryY - 2) {
                    // if already at the boudary --> change direction 180°
                    nextDirection = 3;
                    CreatePipes();
                } else {
                    GoPositiveY(Random.Range(3, maximumBoundaryY - (int) currentPosition.y + 1));
                }
                break;
            case 3:
                // Go in the negative Y direction
                if(currentPosition.y <= minimalBoundaryY + 2) {
                    // if already at the boudary --> change direction 180°
                    nextDirection = 2;
                    CreatePipes();
                } else {
                    GoNegativeY(Random.Range(minimalBoundaryY - (int) currentPosition.y, -2));
                }
                break;
            case 4:
                // Go in the positive Z direction
                if(currentPosition.z >= maximumBoundaryZ - 2) {
                    // if already at the boudary --> change direction 180°
                    nextDirection = 5;
                    CreatePipes();
                } else {
                    GoPositiveZ(Random.Range(3, maximumBoundaryZ - (int) currentPosition.z + 1));
                }
                break;
            case 5:
                // Go in the negative Z direction
                if(currentPosition.z <= minimalBoundaryZ + 2) {
                    // if already at the boudary --> change direction 180°
                    nextDirection = 4;
                    CreatePipes();
                } else {
                    GoNegativeZ(Random.Range(minimalBoundaryZ - (int) currentPosition.z, -2));
                }
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

        GameObject pipe = Instantiate(cylinder, new Vector3(currentPosition.x + (length / 2.0f), currentPosition.y, currentPosition.z), Quaternion.Euler(new Vector3(0, 0, 90)), this.transform);
        pipe.transform.localScale = new Vector3(1, (length / 2.0f), 1);
        Instantiate(sphere, new Vector3(currentPosition.x + length, currentPosition.y, currentPosition.z), Quaternion.identity, this.transform);
        currentPosition += new Vector3(length, 0, 0);
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

        GameObject pipe = Instantiate(cylinder, new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + (length / 2.0f)), Quaternion.Euler(new Vector3(90, 0, 0)), this.transform);
        pipe.transform.localScale = new Vector3(1, (length / 2.0f), 1);
        Instantiate(sphere, new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + length), Quaternion.identity, this.transform);
        currentPosition += new Vector3(0, 0, length);
        nextDirection = Random.Range(0,4);
    }

    void StartNew ()
    {

    }
}
