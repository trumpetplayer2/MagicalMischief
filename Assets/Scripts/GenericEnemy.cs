using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemy : MonoBehaviour
{
    public int chargeTime = 5;
    public float speed = 1f;
    public static GameObject King = null;
    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
