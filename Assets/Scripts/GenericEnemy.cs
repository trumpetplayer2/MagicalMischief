using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenericEnemy : MonoBehaviour
{
    
    public int chargeTime = 5;
    public float speed = 1f;
    public int health = 1;
    public GameObject King;
    public GameObject soundObject;
    public DeathType dead = DeathType.OFFSCREEN;
    private void Start()
    {
        King = GameObject.FindGameObjectWithTag("King");
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        if(dead == DeathType.MAGIC)
        {
            Instantiate(soundObject, transform.position, Quaternion.identity);
        }
    }
}

public enum DeathType{
    OFFSCREEN, MAGIC
}
