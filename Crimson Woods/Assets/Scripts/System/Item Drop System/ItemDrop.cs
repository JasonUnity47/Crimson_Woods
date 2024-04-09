using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    // Declaration
    // Value
    [SerializeField] private float dropForce;
    private Rigidbody2D itemRb;
    private float yLimit;

    // Random Splash
    [Header("Random Splash")]
    public Transform objTransfrom;
    private float delay = 0;
    private float pastTime = 0;
    private float when = 1f;
    private Vector3 off;

    // Collect Area
    [Header("Collect Area")]
    [SerializeField] private Transform collectArea;
    [SerializeField] private float collectRadius;
    [SerializeField] private LayerMask whatIsPlayer;
    private bool isNearby = false;
    private bool once = false;

    // Script Reference
    private CurrencySystem currencySystem;
    private GameObject player;

    private void Awake()
    {
        off = new Vector3(Random.Range(-4, 4), off.y, off.z);
        off = new Vector3(off.x, Random.Range(-4, 4), off.z);
    }

    private void Start()
    {
        currencySystem = GameObject.FindWithTag("Game Manager").GetComponent<CurrencySystem>();
        player = GameObject.FindGameObjectWithTag("Player");

        itemRb = GetComponent<Rigidbody2D>();

        // Destroy the current item.
        Destroy(this.gameObject, 60f);
    }

    private void Update()
    {
        if (when >= delay)
        {
            pastTime = Time.deltaTime;
            objTransfrom.position += off * Time.deltaTime;
            delay += pastTime;
        }

        MagnetFunction();

        if (once)
        {
            Vector3 playerPos = Vector3.MoveTowards(transform.position, player.transform.position + new Vector3(0, -0.3f, 0), 6 * Time.deltaTime);
            itemRb.MovePosition(playerPos);
        }
    }

    void MagnetFunction()
    {
        isNearby = Physics2D.OverlapCircle(collectArea.position, collectRadius, whatIsPlayer);

        if (isNearby)
        {
            once = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(collectArea.position, collectRadius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the item collides with the player and the item is a coin then increase the currency by 1 and destroy the item.
        if (collision.CompareTag("Player") && this.gameObject.CompareTag("Coin"))
        {
            int randomNumber = Random.Range(1, 7); // 1 - 6
            currencySystem.bloodCount += randomNumber;
            Destroy(this.gameObject);
        }

        // Else if the item collides with the player and the item is a food then increase the health by 1 and destroy the item.
        else if (collision.CompareTag("Player") && this.gameObject.CompareTag("Food"))
        {
            //Increase 1 hp for player health.
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            playerHealth.health++;
            Destroy(this.gameObject);
        }
    }
}
