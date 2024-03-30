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

        Destroy(this.gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.right * velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.GetComponent<Boar>() == true)
            {
                Boar boar = collision.GetComponent<Boar>();
                boar.TakeDamage(1);
                Destroy(this.gameObject);
            }

            else if (collision.GetComponent<DireBoar>() == true)
            {
                DireBoar direBoar = collision.GetComponent<DireBoar>();
                direBoar.TakeDamage(1);
                Destroy(this.gameObject);
            }

            else if (collision.GetComponent<GoblinStats>() == true)
            {
                GoblinStats goblinStats = collision.GetComponent<GoblinStats>();
                goblinStats.health--;
                Destroy(this.gameObject);
            }

            else if (collision.GetComponent<SlimeStats>() == true)
            {
                SlimeStats slimeStats = collision.GetComponent<SlimeStats>();
                slimeStats.health--;
                Destroy(this.gameObject);
            }

            // Boss 1
            //else if (collision.GetComponent<Boss1>() == true)
            //{
            //BoarStats boarStats = collision.GetComponent<BoarStats>();
            //boarStats.health--;
            //}

            else if (collision.GetComponent<Boss2Stats>() == true)
            {
                Boss2Stats boss2Stats = collision.GetComponent<Boss2Stats>();
                boss2Stats.health--;
                Destroy(this.gameObject);
            }
        }
    }
}
