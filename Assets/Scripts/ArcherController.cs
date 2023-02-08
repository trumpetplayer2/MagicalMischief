using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherController : GenericEnemy
{
    Transform target;
    public Transform aim;
    private float nextUpdate;
    public GameObject arrow;
    // Start is called before the first frame update
    void Start()
    {
        nextUpdate = Time.time + 1;
        GameObject temp = GameObject.FindGameObjectWithTag("Player");
        if (temp != null)
        {
            target = temp.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (chargeTime >= 0)
        {
            if (nextUpdate <= Time.time)
            {
                chargeTime -= 1;
            }
            return;
        }
        //Head up when no target. We will be setting target to null after shooting an arrow
        if (target == null)
        {
            transform.TransformVector(Vector3.up);
            return;
        }
        transform.rotation = Quaternion.FromToRotation(transform.position, target.position);
        //Shoot
        Instantiate(arrow, aim.position, transform.rotation);
        if(King == null)
        {
            target = null;
        }
        chargeTime = 5;
    }
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
