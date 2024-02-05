using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Camera : MonoBehaviour
{
    public int minimalRotation = -10;
    public int maximumRotation = 10;

    /*
    Change the rotation of the camera to have a different angle with the start of a new scene
    */
    public void ChangeRotation () {
        transform.rotation = Quaternion.Euler(Random.Range(minimalRotation,maximumRotation), Random.Range(minimalRotation,maximumRotation), 0);
    }
}
