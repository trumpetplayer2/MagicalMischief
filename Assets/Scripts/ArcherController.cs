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
        King = GameObject.FindGameObjectWithTag("King");
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
                nextUpdate = Time.time + 1;
            }
            return;
        }
        //Head up when no target. We will be setting target to null after shooting an arrow
        if (target == null)
        {
            transform.forward = Vector3.forward;
            transform.rotation = (Quaternion.Euler(0, 0, 90));
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            return;
        }
        Vector3 diff = target.position - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
        //Shoot
        Instantiate(arrow, aim.position, transform.rotation);
        if(King == null)
        {
            target = null;
        }
        chargeTime = 2;
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
