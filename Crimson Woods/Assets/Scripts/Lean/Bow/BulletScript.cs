using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    public float force;
    public float maxRange = 10f; // Maximum range the bullet can travel
    public int damage = 1;
    private float traveledDistance = 0f;
    private string playerTag = "Player";
    private string wallTag = "Wall";

    private BuffContent buffContent;
    private int penetrationNumber = 3;

    private SpriteRenderer spriteRenderer;

    private float startTime = 0.05f;
    private float timeBtwFrame;

    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        buffContent = GameObject.FindWithTag("Game Manager").GetComponent<BuffContent>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        rb = GetComponent<Rigidbody2D>();

        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 direction = mousePos - transform.position;

        Vector3 rotation = transform.position - mousePos;

        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rot + 90);

        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
        }

        timeBtwFrame = startTime;
    }

    void Update()
    {
        // Calculate the distance traveled by the bullet
        traveledDistance += force * Time.deltaTime;

        // If the bullet reaches its maximum range, destroy it
        if (traveledDistance >= maxRange)
        {
            DestroyBullet();
        }

        if (buffContent.onPiercingArrows && timeBtwFrame > 0)
        {
            timeBtwFrame -= Time.deltaTime;
        }

        // IF Piercing Arrows is ON and Color is not red THEN change color.
        // Change color to red.
        // Red piercing arrows.
        if (buffContent.onPiercingArrows && spriteRenderer.color != Color.red)
        {
            spriteRenderer.color = Color.red;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the bullet collides with an object tagged as a wall
        if (collision.gameObject.CompareTag(wallTag))
        {
            DestroyBullet();
        }

        // Check if the bullet collides with an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.GetComponent<Boar>() != null)
            {
                Boar boarHealth = collision.gameObject.GetComponent<Boar>();

                // Apply damage to the enemy
                if (boarHealth != null)
                {
                    boarHealth.TakeDamage(damage);
                }
            }

            else if (collision.GetComponent<DireBoar>() != null)
            {
                DireBoar direBoarHealth = collision.gameObject.GetComponent<DireBoar>();

                // Apply damage to the enemy
                if (direBoarHealth != null)
                {
                    direBoarHealth.TakeDamage(damage);
                }
            }

            else if (collision.GetComponent<Goblin>() != null)
            {
                Goblin goblinHealth = collision.gameObject.GetComponent<Goblin>();

                // Apply damage to the enemy
                if (goblinHealth != null)
                {
                    goblinHealth.TakeDamage(damage);
                }
            }

            else if (collision.GetComponent<Slime>() != null)
            {
                Slime slimeHealth = collision.gameObject.GetComponent<Slime>();

                // Apply damage to the enemy
                if (slimeHealth != null)
                {
                    slimeHealth.TakeDamage(damage);
                }
            }

            else if (collision.GetComponent<Boss1>() != null)
            {
                Boss1 boss1Health = collision.gameObject.GetComponent<Boss1>();

                // Apply damage to the enemy
                if (boss1Health != null)
                {
                    boss1Health.health--;
                }
            }

            else if (collision.GetComponent<Boss2>() != null)
            {
                Boss2 boss2Health = collision.gameObject.GetComponent<Boss2>();

                // Apply damage to the enemy
                if (boss2Health != null)
                {
                    boss2Health.TakeDamage(damage);
                }
            }

            // IF Piercing Arrows is ON THEN can penetrate enemy.
            if (buffContent.onPiercingArrows)
            {
                if (timeBtwFrame <= 0)
                {
                    timeBtwFrame = startTime;
                    penetrationNumber--;
                }

                else
                {
                    timeBtwFrame -= Time.deltaTime;
                }

                if (penetrationNumber <= 0)
                {
                    // Destroy the bullet
                    DestroyBullet();
                }
            }

            else
            {
                // Destroy the bullet
                DestroyBullet();
            }
        }
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}



