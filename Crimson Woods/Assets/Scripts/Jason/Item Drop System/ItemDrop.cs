using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    // Declaration
    [SerializeField] private float dropForce;
    private Rigidbody2D itemRb;
    private float yLimit;

    private CurrencySystem currencySystem;

    private void Awake()
    {
        yLimit = transform.position.y - 2f;
    }

    private void Start()
    {
        currencySystem = GameObject.FindWithTag("Game Manager").GetComponent<CurrencySystem>();

        itemRb = GetComponent<Rigidbody2D>();

        itemRb.AddForce(Vector2.up * dropForce, ForceMode2D.Impulse);

        DestroySelf();
    }

    private void Update()
    {
        if (itemRb.position.y <= yLimit)
        {
            itemRb.gravityScale = 0;
            itemRb.velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && this.gameObject.CompareTag("Coin"))
        {
            int randomNumber = Random.Range(1, 7); // 1 - 6
            currencySystem.bloodCount += randomNumber;
            Destroy(this.gameObject);
        }

        else if (collision.CompareTag("Player") && this.gameObject.CompareTag("Food"))
        {
            //Increase 1 hp for player health
            //Destroy(this.gameObject);
        }
    }

    void DestroySelf()
    {
        Destroy(this.gameObject, 5f);
    }
}
