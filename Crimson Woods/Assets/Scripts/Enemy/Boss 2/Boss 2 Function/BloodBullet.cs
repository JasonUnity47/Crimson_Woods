using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBullet : MonoBehaviour
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
        if (collision.CompareTag("Player"))
        {
            // Player
            if (collision.GetComponent<PlayerHealth>() == true)
            {
                PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
                playerHealth.TakeDamage(1);
                Destroy(this.gameObject);
            }
        }
    }
}
