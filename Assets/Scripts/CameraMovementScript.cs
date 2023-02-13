using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    public Transform StartLocation;
    public Transform[] endLocations;
    public int currentPoint = 0;
    public float speed = 0.5f;

    private void Start()
    {
        transform.position = StartLocation.position;
    }

    private void Update()
    {
        float distance = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, endLocations[currentPoint].position, distance);
        if(transform.position.normalized == endLocations[currentPoint].position.normalized && (endLocations.Length - 1) > currentPoint)
        {
            currentPoint += 1;
        }
        else
        {
            //End Level
            if(transform.position.normalized == endLocations[currentPoint].position.normalized)
            {
                GameManager.instance.finishLevel();
            }
        }
    }
}
