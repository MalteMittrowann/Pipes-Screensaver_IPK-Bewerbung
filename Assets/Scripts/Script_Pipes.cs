using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Pipes : MonoBehaviour
{
    public GameObject sphere;
    public GameObject cylinder;

    public int minimalTransform = -20;
    public int maximumTransform = 0;

    void Start()
    {
        Vector3 startSpawnPosition = new Vector3 (Random.Range(-10, 10), Random.Range(-5, 5), Random.Range(0, 20));
        Instantiate(sphere, startSpawnPosition, Quaternion.identity);
    }

    void Update()
    {
        
    }
}
