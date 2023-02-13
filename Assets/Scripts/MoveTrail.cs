using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrail : MonoBehaviour
{
    public AudioClip killEnemy;
    public int moveSpeed = 170;
    public string EnemyTag = "Enemy";
    public string[] ignoreTags = new string[0];
    public float lifeTime = 1f;
    public int damage = 1;
    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        Destroy(gameObject, lifeTime);
    }

    public void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Spawn hit particle
        if(ignoreTags.Length > 0)
        {
            foreach(string tag in ignoreTags)
            {
                if (collision.gameObject.tag.Equals(tag))
                {
                    return;
                }
            }
        }
        //Kill Enemy
        if (collision.gameObject.tag.Equals(EnemyTag))
        {
            if (EnemyTag != "Player")
            {
                if(collision.gameObject.GetComponent<GenericEnemy>() != null)
                {
                    GenericEnemy enemy = collision.gameObject.GetComponent<GenericEnemy>();
                    if(enemy.health > 1)
                    {
                        enemy.health -= 1;
                    }
                    else
                    {
                        enemy.dead = DeathType.MAGIC;
                        Destroy(collision.gameObject);
                    }
                }
            }
            else
            {
                //Damage Player
                if (collision.gameObject.GetComponent<PlayerController>() != null)
                {
                    collision.gameObject.GetComponent<PlayerController>().damagePlayer(damage, false);
                }
            }
        }
        if (collision.gameObject.GetComponent<GenericEnemy>() != null)
        {
            BossAI enemy = collision.gameObject.GetComponent<BossAI>();
            if (EnemyTag == "Enemy" && collision.tag == "King")
            {
                if (enemy.health > 1)
                {
                    enemy.health -= 1;
                }
                else
                {
                    if(enemy.phase >= enemy.totalPhases)
                    {
                        enemy.killed();
                    }
                    else
                    {
                        enemy.nextPhase();
                    }
                }
            }

        }
        //Despawn
        Destroy(this.gameObject);
    }
}
