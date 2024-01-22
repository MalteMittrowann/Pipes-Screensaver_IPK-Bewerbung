using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Camera : MonoBehaviour
{
    public int minimalRotation = -10;
    public int maximumRotation = 10;
    public int secondsToRepeat = 30;

    void Start () {
        InvokeRepeating("ChangeRotation", 0, secondsToRepeat);
    }

    void ChangeRotation () {
        transform.rotation = Quaternion.Euler(Random.Range(minimalRotation,maximumRotation), Random.Range(minimalRotation,maximumRotation), 0);
    }
}
