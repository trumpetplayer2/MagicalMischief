using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : GenericEnemy
{
    public float nextUpdate;
    public GameObject cannonBall;
    public Transform aim;
    // Start is called before the first frame update
    void Start()
    {
        King = GameObject.FindGameObjectWithTag("King");
        nextUpdate = Time.time + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (chargeTime >= 0)
        {
            if (nextUpdate <= Time.time)
            {
                chargeTime -= 1;
                nextUpdate = Time.time + 1;
            }
            return;
        }
        else
        {
            Instantiate(cannonBall, aim.position, transform.rotation);
            chargeTime = 3;
        }
    }
}
