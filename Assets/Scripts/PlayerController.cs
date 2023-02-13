using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.PlayerLoop.PreLateUpdate;

public class PlayerController : MonoBehaviour
{
    public float baseSpeed;
    public float movementSpeed;
    public Rigidbody2D Player;
    private float inX = 0f;
    private float inY = 0f;
    public AudioClip shootSound;
    public AudioClip damageSound;
    public AudioSource shootSource;
    public AudioSource damageAudioSource;
    public int range = 10;
    bool reloading = false;
    public SpriteRenderer colorFlash;
    public Color defaultColor;
    public Color flashColor;
    public Color transparentColor;
    private int timeBetweenFlash = 1;
    private int timeSinceLastFlash = 0;
    public float iframes = 0;
    public int health = 3;
    public ManaBar mana;

    //public ParticleSystem muzzleFlash;

    private float nextUpdate = 0f;

    public SpriteRenderer gun;
    public Transform pistolPivot;
    public Transform pistolBarrel;
    public LineRenderer bulletLine;


    private void Start()
    {
        //gameManager = GameManager.instance;
        bulletLine.startColor = transparentColor;
        nextUpdate = Time.time + 1f;
    }

    private void Update()
    {
        inX = Input.GetAxisRaw("Horizontal");
        inY = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Fire1"))
        {
            if (!reloading)
            {
                shootGun();
            }
        }
    }

    private void FixedUpdate()
    {
        if (Time.timeScale <= 0) { return; }
        if (iframes > 0)
        {
            timeSinceLastFlash -= 1;
            if (colorFlash.color.a > 0.5f && timeSinceLastFlash < 1)
            {
                colorFlash.color = flashColor;
                timeSinceLastFlash = timeBetweenFlash;
            }
            else if (timeSinceLastFlash < 1)
            {
                colorFlash.color = defaultColor;
                timeSinceLastFlash = timeBetweenFlash;
            }
        }
        else
        {
            colorFlash.color = defaultColor;
        }

        Vector3 mousePos = Input.mousePosition;
        Vector3 gunPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - gunPos.x;
        mousePos.y = mousePos.y - gunPos.y;
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        pistolPivot.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        if (angle > 90 || angle <= -90)
        {
            gun.flipY = true;
            pistolBarrel.localPosition = new Vector3(0.06f, -0.02f, 0f);
        }
        else
        {
            gun.flipY = false;
            pistolBarrel.localPosition = new Vector3(0.06f, 0.02f, 0f);
        }

        Vector2 moveX = new Vector2(inX * movementSpeed, inY * movementSpeed);
        Player.velocity = moveX;
        Debug.DrawLine(pistolBarrel.position, mousePos, Color.green);
    }

    public void damagePlayer(int amount, bool ignoreIFrames)
    {
        if (iframes <= 0 || ignoreIFrames)
        {
            if (!damageAudioSource.isPlaying)
            {
                damageAudioSource.PlayOneShot(damageSound);
            }
            health -= amount;
            if (health < 0)
            {
                health = 0;
            }
            iframes = 2;
            if (health > 0)
            {
                
            }
        }
        if (health == 0)
        {
            //Player is dead
            Time.timeScale = 0;
            Die();
        }
    }

    private void shootGun()
    {
        if (Time.timeScale == 0) { return; }
        if (!mana.GetMana().TrySpendMana(20))
        {
            return;
        }
        //Vector3 mousePos = Input.mousePosition;
        //Vector3 gunPos = Camera.main.WorldToScreenPoint(transform.position);
        //mousePos.x = mousePos.x - gunPos.x;
        //mousePos.y = mousePos.y - gunPos.y;
        ////Shoot
        //RaycastHit2D hit = Physics2D.Raycast(pistolBarrel.position, mousePos, range);
        effect();
        //if (hit)
        //{
        //    if (hit.collider.gameObject.tag.Equals("Enemy")){
        //        Destroy(hit.collider.gameObject);
        //    }
        //}
    }

    private void effect()
    {
        if(Time.timeScale == 0) { return; }
       // muzzleFlash.time = 0;
       // muzzleFlash.Play();
        shootSource.PlayOneShot(shootSound);
        Instantiate(bulletLine, pistolBarrel.position, pistolPivot.rotation);

        
    }

    public void updateIFrames()
    {
        if(iframes > 0)
        {
            iframes -= 1;
        }
    }

    public void Die()
    {
        //Play death animation if time

        GameManager.instance.Death();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetButton("Fire1") || Input.GetButton("Vertical"))
        {
            if(collision.gameObject.tag == "Door")
            {
                //Finish Level
                GameManager.instance.finishLevel();
            }
        }
    }
}
