using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public float secondsToRenewTheScene = 20.0f;

    // Private variables:
    private Vector3 currentPosition = new Vector3(0,0,0);
    private int nextDirection;
    private int previousDirection = -1;
    private Color materialColor;

    /*
     - Random Direction
     - Random Start-Position
     - Random Color
     - Create the starting sphere and give its Material the color
     - Call CreatePipes() in regular short intervals to create the pipes
     - Call StartNew() in regular to start the scene anew
    */
    IEnumerator Start()
    {
        nextDirection = Random.Range(0,6); // Random Number from 0 (incl.) to 6 (excl.): for the 6 possible directions
        Vector3 startSpawnPosition = new Vector3 (Random.Range(minimalBoundaryX, maximumBoundaryX), Random.Range(minimalBoundaryY, maximumBoundaryY), Random.Range(minimalBoundaryZ, maximumBoundaryZ)); // Create random startPosition for the first sphere
        currentPosition = startSpawnPosition;
        materialColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
        GameObject startSphere = Instantiate(sphere, startSpawnPosition, Quaternion.identity, this.transform) as GameObject;
        startSphere.GetComponent<Renderer>().material.SetColor("_Color", materialColor);
        InvokeRepeating("CreatePipes", 0.5f, secondsToSpawnNewPipe);
        yield return StartCoroutine(StartNew(secondsToRenewTheScene));
    }

    /*
     - Spawn a new pipe in the next direction
     - Check if already to close to the boundary
     - Decide on a random number for the size of the pipe and call the RaycastForCollision()-function to check if it would be too long
    */
    void CreatePipes ()
    {
        switch (nextDirection)
        {
            case 0:
                // Go in the positive X direction
                if(currentPosition.x >= maximumBoundaryX - 2) {
                    // if already at the boundary --> change direction 180°
                    nextDirection = 1;
                    CreatePipes();
                } else {
                    GoPositiveX(RaycastForCollision(new Vector3(1,0,0), Random.Range(3, maximumBoundaryX - (int) currentPosition.x + 1)));
                    previousDirection = 0;
                }
                break;
            case 1:
                // Go in the negative X direction
                if(currentPosition.x <= minimalBoundaryX + 2) {
                    // if already at the boundary --> change direction 180°
                    nextDirection = 0;
                    CreatePipes();
                } else {
                    GoNegativeX(RaycastForCollision(new Vector3(-1,0,0), Random.Range(minimalBoundaryX - (int) currentPosition.x, -2)));
                    previousDirection = 1;
                }
                break;
            case 2:
                // Go in the positive Y direction
                if(currentPosition.y >= maximumBoundaryY - 2) {
                    // if already at the boundary --> change direction 180°
                    nextDirection = 3;
                    CreatePipes();
                } else {
                    GoPositiveY(RaycastForCollision(new Vector3(0,1,0), Random.Range(3, maximumBoundaryY - (int) currentPosition.y + 1)));
                    previousDirection = 2;
                }
                break;
            case 3:
                // Go in the negative Y direction
                if(currentPosition.y <= minimalBoundaryY + 2) {
                    // if already at the boundary --> change direction 180°
                    nextDirection = 2;
                    CreatePipes();
                } else {
                    GoNegativeY(RaycastForCollision(new Vector3(0,-1,0), Random.Range(minimalBoundaryY - (int) currentPosition.y, -2)));
                    previousDirection = 3;
                }
                break;
            case 4:
                // Go in the positive Z direction
                if(currentPosition.z >= maximumBoundaryZ - 2) {
                    // if already at the boundary --> change direction 180°
                    nextDirection = 5;
                    CreatePipes();
                } else {
                    GoPositiveZ(RaycastForCollision(new Vector3(0,0,1), Random.Range(3, maximumBoundaryZ - (int) currentPosition.z + 1)));
                    previousDirection = 4;
                }
                break;
            case 5:
                // Go in the negative Z direction
                if(currentPosition.z <= minimalBoundaryZ + 2) {
                    // if already at the boundary --> change direction 180°
                    nextDirection = 4;
                    CreatePipes();
                } else {
                    GoNegativeZ(RaycastForCollision(new Vector3(0,0,-1), Random.Range(minimalBoundaryZ - (int) currentPosition.z, -2)));
                    previousDirection = 5;
                }
                break;
        }
    }

    /*
     - Check for collisions with already existing pipes
     - Shorten the pipe if it would otherwise collide
     - If too short in that case --> Spawn a new pipe at a free spot
    */
    int RaycastForCollision(Vector3 direction, int length)
    {
        if (Physics.Raycast(currentPosition, direction, out RaycastHit hit, Mathf.Abs(length))) {
            if (hit.distance <= 1) {
                Debug.Log("Start new!");
                CancelInvoke();
                bool collisionCheck = true;
                while (collisionCheck) {
                    Vector3 vector = new Vector3 (Random.Range(minimalBoundaryX, maximumBoundaryX), Random.Range(minimalBoundaryY, maximumBoundaryY), Random.Range(minimalBoundaryZ, maximumBoundaryZ));
                    if (!Physics.SphereCast(vector, 2, transform.forward, out RaycastHit hitParameter, 2.0f)) {
                        collisionCheck = false;
                        currentPosition = vector;
                    }
                }
                materialColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
                InvokeRepeating("CreatePipes", 0.2f, secondsToSpawnNewPipe);
                return 0;
            } else {
                Debug.Log("Collision, but can be shortened!");
                if (direction ==  new Vector3(-1,0,0) || direction ==  new Vector3(0,-1,0) || direction ==  new Vector3(0,0,-1)) {
                    return (int) (-hit.distance + 1.0f);
                } else {
                    return (int) (hit.distance - 1.0f);
                }
                
            }
        } else {
            Debug.Log("No collision!");
            return length;
        }
    }

    /*
     - After a regular long stretch of time start the scene anew, so that the scene is cleared and a new pipe is spawned
    */
    IEnumerator StartNew(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        CancelInvoke();
        foreach(Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
        currentPosition = new Vector3 (Random.Range(minimalBoundaryX, maximumBoundaryX), Random.Range(minimalBoundaryY, maximumBoundaryY), Random.Range(minimalBoundaryZ, maximumBoundaryZ));
        GameObject startSphere = Instantiate(sphere, currentPosition, Quaternion.identity, this.transform) as GameObject;
        materialColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
        startSphere.GetComponent<Renderer>().material.SetColor("_Color", materialColor);
        InvokeRepeating("CreatePipes", 0.2f, secondsToSpawnNewPipe);
        yield return StartCoroutine(StartNew(secondsToRenewTheScene));
    }

    /*
     - Spawn a new pipe in the correct direction
     - Assign it the length and the color
     - Spawn a sphere as a corner
     - Assign the sphere the color
     - Shift the currentPosition to the new spot
     - Get a random new direction, which is not backwards and forward
    */
    void GoPositiveX (int length)
    {
        // nextDirection = 0

        GameObject pipe = Instantiate(cylinder, new Vector3(currentPosition.x + (length / 2.0f), currentPosition.y, currentPosition.z), Quaternion.Euler(new Vector3(0, 0, 270)), this.transform) as GameObject;
        pipe.transform.localScale = new Vector3(1, (length / 2.0f), 1);
        pipe.GetComponent<Renderer>().material.SetColor("_Color", materialColor);
        GameObject corner = Instantiate(sphere, new Vector3(currentPosition.x + length, currentPosition.y, currentPosition.z), Quaternion.identity, this.transform) as GameObject;
        corner.GetComponent<Renderer>().material.SetColor("_Color", materialColor);
        currentPosition += new Vector3(length, 0, 0);
        nextDirection = Random.Range(2,6);
    }

    void GoNegativeX (int length)
    {
        // nextDirection = 1

        GameObject pipe = Instantiate(cylinder, new Vector3(currentPosition.x + (length / 2.0f), currentPosition.y, currentPosition.z), Quaternion.Euler(new Vector3(0, 0, 90)), this.transform) as GameObject;
        pipe.transform.localScale = new Vector3(1, (length / 2.0f), 1);
        pipe.GetComponent<Renderer>().material.SetColor("_Color", materialColor);
        GameObject corner = Instantiate(sphere, new Vector3(currentPosition.x + length, currentPosition.y, currentPosition.z), Quaternion.identity, this.transform) as GameObject;
        corner.GetComponent<Renderer>().material.SetColor("_Color", materialColor);
        currentPosition += new Vector3(length, 0, 0);
        nextDirection = Random.Range(2,6);
    }

    void GoPositiveY (int length)
    {
        // nextDirection = 2
        
        GameObject pipe = Instantiate(cylinder, new Vector3(currentPosition.x, currentPosition.y + (length / 2.0f), currentPosition.z), Quaternion.identity, this.transform) as GameObject;
        pipe.transform.localScale = new Vector3(1, (length / 2.0f), 1);
        pipe.GetComponent<Renderer>().material.SetColor("_Color", materialColor);
        GameObject corner = Instantiate(sphere, new Vector3(currentPosition.x, currentPosition.y + length, currentPosition.z), Quaternion.identity, this.transform) as GameObject;
        corner.GetComponent<Renderer>().material.SetColor("_Color", materialColor);
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

        GameObject pipe = Instantiate(cylinder, new Vector3(currentPosition.x, currentPosition.y + (length / 2.0f), currentPosition.z), Quaternion.identity, this.transform) as GameObject;
        pipe.transform.localScale = new Vector3(1, (length / 2.0f), 1);
        pipe.GetComponent<Renderer>().material.SetColor("_Color", materialColor);
        GameObject corner = Instantiate(sphere, new Vector3(currentPosition.x, currentPosition.y + length, currentPosition.z), Quaternion.identity, this.transform) as GameObject;
        corner.GetComponent<Renderer>().material.SetColor("_Color", materialColor);
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

        GameObject pipe = Instantiate(cylinder, new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + (length / 2.0f)), Quaternion.Euler(new Vector3(270, 0, 0)), this.transform) as GameObject;
        pipe.transform.localScale = new Vector3(1, (length / 2.0f), 1);
        pipe.GetComponent<Renderer>().material.SetColor("_Color", materialColor);
        GameObject corner = Instantiate(sphere, new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + length), Quaternion.identity, this.transform) as GameObject;
        corner.GetComponent<Renderer>().material.SetColor("_Color", materialColor);
        currentPosition += new Vector3(0, 0, length);
        nextDirection = Random.Range(0,4);
    }

    void GoNegativeZ (int length)
    {
        // nextDirection = 5

        GameObject pipe = Instantiate(cylinder, new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + (length / 2.0f)), Quaternion.Euler(new Vector3(90, 0, 0)), this.transform) as GameObject;
        pipe.transform.localScale = new Vector3(1, (length / 2.0f), 1);
        pipe.GetComponent<Renderer>().material.SetColor("_Color", materialColor);
        GameObject corner = Instantiate(sphere, new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + length), Quaternion.identity, this.transform) as GameObject;
        corner.GetComponent<Renderer>().material.SetColor("_Color", materialColor);
        currentPosition += new Vector3(0, 0, length);
        nextDirection = Random.Range(0,4);
    }
}
