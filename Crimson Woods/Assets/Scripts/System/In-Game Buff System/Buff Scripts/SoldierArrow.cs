using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SoldierArrow : MonoBehaviour
{
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
                boar.TakeDamage(1);
                Destroy(this.gameObject);
            }

            // Dire Boar
            else if (collision.GetComponent<DireBoar>() == true)
            {
                DireBoar direBoar = collision.GetComponent<DireBoar>();
                direBoar.TakeDamage(1);
                Destroy(this.gameObject);
            }
            
            // Goblin
            else if (collision.GetComponent<Goblin>() == true)
            {
                Goblin goblin = collision.GetComponent<Goblin>();
                goblin.TakeDamage(1);
                Destroy(this.gameObject);
            }

            // Slime
            else if (collision.GetComponent<Slime>() == true)
            {
                Slime slime = collision.GetComponent<Slime>();
                slime.TakeDamage(1);
                Destroy(this.gameObject);
            }

            // Boss 1
            else if (collision.GetComponent<Boss1Data>() == true)
            {
                Boss1Data boss1Stats = collision.GetComponent<Boss1Data>();
                boss1Stats.health--;
            }

            // Boss 2
            else if (collision.GetComponent<Boss2>() == true)
            {
                Boss2 boss2 = collision.GetComponent<Boss2>();
                boss2.TakeDamage(1);
                Destroy(this.gameObject);
            }
        }
    }
}
