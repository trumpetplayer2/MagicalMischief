using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Swordsperson : GenericEnemy
{
    Vector3 target;
    private float nextUpdate;
    // Start is called before the first frame update
    void Start()
    {
        nextUpdate = Time.time + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(chargeTime >= 0)
        {
            if(chargeTime == 0)
            {
                GameObject temp = GameObject.FindGameObjectWithTag("Player");
                if (temp != null)
                {
                    target = (temp.transform.position - transform.position) * 10;
                }
            }
            if(nextUpdate <= Time.time)
            {
                chargeTime -= 1;
            }
            return;
        }
        //Charge time is over, attack
        if(target == null)
        {
            transform.position = Vector3.up;
        }
        else
        {
            //Face enemy
            Vector3 diff = target - transform.position;
            diff.Normalize();

            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z+90);
            //Move towards target at speed of speed
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
    }

    //Get if the player comes into contact with the swordsperson
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!(collision.tag == "Player")) { return; }
        //Touched player, time to hit
        if (collision.gameObject.GetComponent<PlayerController>() == null) { return; }
        PlayerController controller = collision.gameObject.GetComponent<PlayerController>();
        controller.damagePlayer(1, false);
        //Play enemy death noise

        //Enemy hit, now it dies. Whoops
        Destroy(this.gameObject);
    }
}
