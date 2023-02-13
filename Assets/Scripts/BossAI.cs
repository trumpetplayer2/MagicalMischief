using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : GenericEnemy
{
    public int phase = 1;
    public int totalPhases = 2;
    SpriteRenderer sprite;
    public Sprite king;
    Transform target;
    private float nextUpdate;
    public EnemySpawner[] enemySpawners;
    public Collider2D doorCollider;
    private void Start()
    {
        sprite = this.GetComponentInParent<SpriteRenderer>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
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
        }
        if (phase == 2)
        {
            kingUpdate();
        }
    }

    public void nextPhase()
    {
        Destroy(doorCollider);
        health += 9;
        phase++;
        sprite.sprite = king;
        chargeTime = 5;
        speed = 1;
        //Summon in initial enemies
        foreach(EnemySpawner spawner in enemySpawners)
        {
            spawner.spawnEnemy();
        }
    }

    public void killed()
    {
        //Play the killed sound

        //Finish Level
        GameManager.instance.finishLevel();
    }

    public void kingUpdate()
    {
        Vector3 diff = target.position - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);
        //Move towards target at speed of speed
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        //Roll a Random Number to see if an enemy is spawned
        if (chargeTime == 0)
        {
            int rand = Random.Range(0, 20);
            if(rand < enemySpawners.Length)
            {
                enemySpawners[rand].spawnEnemy();
                chargeTime = 7;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!(collision.tag == "Player")) { return; }
        //Touched player, time to hit
        if (collision.gameObject.GetComponent<PlayerController>() == null) { return; }
        PlayerController controller = collision.gameObject.GetComponent<PlayerController>();
        controller.damagePlayer(2, false);
    }
}
