using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SoldierArrow : MonoBehaviour
{
    // Damage
    [Header("Damage")]
    public int damage;

    public float velocity = 10;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Destroy(this.gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.right * velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the bullet collides with an object tagged as a wall
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }

        if (collision.CompareTag("Enemy"))
        {
            // Boar
            if (collision.GetComponent<Boar>() == true)
            {
                Boar boar = collision.GetComponent<Boar>();
                boar.TakeDamage(damage);
                Destroy(this.gameObject);
            }

            // Dire Boar
            else if (collision.GetComponent<DireBoar>() == true)
            {
                DireBoar direBoar = collision.GetComponent<DireBoar>();
                direBoar.TakeDamage(damage);
                Destroy(this.gameObject);
            }
            
            // Goblin
            else if (collision.GetComponent<Goblin>() == true)
            {
                Goblin goblin = collision.GetComponent<Goblin>();
                goblin.TakeDamage(damage);
                Destroy(this.gameObject);
            }

            // Blood Goblin
            else if (collision.GetComponent<Goblin>() == true)
            {
                Goblin1 bloodGoblin = collision.GetComponent<Goblin1>();
                bloodGoblin.TakeDamage(damage);
                Destroy(this.gameObject);
            }

            // Slime
            else if (collision.GetComponent<Slime>() == true)
            {
                Slime slime = collision.GetComponent<Slime>();
                slime.TakeDamage(damage);
                Destroy(this.gameObject);
            }

            // Blood Slime
            else if (collision.GetComponent<Slime>() == true)
            {
                Slime1 bloodSlime = collision.GetComponent<Slime1>();
                bloodSlime.TakeDamage(damage);
                Destroy(this.gameObject);
            }

            // Boss 1
            else if (collision.GetComponent<Boss1Data>() == true)
            {
                Boss1 boss1 = collision.GetComponent<Boss1>();
                boss1.TakeDamage(damage);
                Destroy(this.gameObject);
            }

            // Blood Boss 1
            else if (collision.GetComponent<Boss1Data>() == true)
            {
                BloodGoblin bloodBoss = collision.GetComponent<BloodGoblin>();
                bloodBoss.TakeDamage(damage);
                Destroy(this.gameObject);
            }

            // Boss 2
            else if (collision.GetComponent<Boss2>() == true)
            {
                Boss2 boss2 = collision.GetComponent<Boss2>();
                boss2.TakeDamage(damage);
                Destroy(this.gameObject);
            }
        }
    }
}
