using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSound : MonoBehaviour
{
    public AudioSource soundSource;
    private void Update()
    {
        if (!soundSource.isPlaying)
        {
            Destroy(transform.gameObject);
        }
    }
}
