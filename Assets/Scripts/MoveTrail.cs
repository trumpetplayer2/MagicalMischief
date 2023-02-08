using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrail : MonoBehaviour
{
    public AudioClip killEnemy;
    public int moveSpeed = 170;
    public string EnemyTag = "Enemy";
    public float lifeTime = 1f;
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

        //Kill Enemy
        if (collision.gameObject.tag.Equals(EnemyTag))
        {
            if (EnemyTag != "Player")
            {
                if(collision.gameObject.GetComponent<AudioSource>() != null)
                {
                    collision.gameObject.GetComponent<AudioSource>().PlayOneShot(killEnemy);
                }
                Destroy(collision.gameObject);
            }
            else
            {
                //Damage Player
                if (collision.gameObject.GetComponent<PlayerController>() != null)
                {
                    collision.gameObject.GetComponent<PlayerController>().damagePlayer(1, false);
                }
            }
        }
        //Despawn
        Destroy(this.gameObject);
    }
}
