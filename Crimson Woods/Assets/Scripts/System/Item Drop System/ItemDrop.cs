using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    // Declaration
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
    private Rigidbody2D itemRb;
    private SpriteRenderer spriteRenderer;
    private HealthHeartBar healthHeartBar;

    // Breathing Effect
    [Header("Breathing Effect")]
    private float alpha;
    private bool isEnd = false;

    public AudioSource myAudio;
    public AudioClip BloodDropSFX;
    public AudioClip FoodDropSFX;

    private bool isCollected = false;

    private void Awake()
    {
        myAudio = GetComponent<AudioSource>();
        // Set the off variable with random value between -4 and 4.
        off = new Vector3(Random.Range(-4, 4), off.y, off.z);
        off = new Vector3(off.x, Random.Range(-4, 4), off.z);
    }

    private void Start()
    {
        currencySystem = GameObject.FindWithTag("Game Manager").GetComponent<CurrencySystem>();
        player = GameObject.FindGameObjectWithTag("Player");
        healthHeartBar = GameObject.FindObjectOfType<HealthHeartBar>();

        itemRb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Get the current color of the alpha value of this gameobject.
        alpha = spriteRenderer.color.a;

        // Destroy the current item with a predefined time.
        Destroy(this.gameObject, 60f);
    }

    private void Update()
    {
        // Not sure how to implement yet... Need time to learn.
        // Allows the loot will move to a predefined area around the enemy.
        if (when >= delay)
        {
            pastTime = Time.deltaTime;
            objTransfrom.position += off * Time.deltaTime;
            delay += pastTime;
        }

        // Breathing Effect to show the animation of the loots.
        // The alpha value of the color of the current gameobject will keep changing between 0.5 and 1.
        BreathingEffect();

        // The current gameobject will move toward slowly to the player to allow player to collect the gameobject easily.
        // Demonstrate magnetic game mechanics.
        CollectLoot();
    }

    void CollectLoot()
    {
        isNearby = Physics2D.OverlapCircle(collectArea.position, collectRadius, whatIsPlayer);

        if (isNearby)
        {
            once = true;
        }

        if (once)
        {
            Vector3 playerPos = Vector3.MoveTowards(transform.position, player.transform.position + new Vector3(0, -0.3f, 0), 6 * Time.deltaTime);
            itemRb.MovePosition(playerPos);
        }

        return;
    }

    void BreathingEffect()
    {
        if (alpha <= 0.5f)
        {
            alpha = 0.5f;
            isEnd = true;
        }

        if (alpha >= 1)
        {
            alpha = 1;
            isEnd = false;
        }

        if (!isEnd)
        {
            alpha -= Time.deltaTime;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
        }

        else
        {
            alpha += Time.deltaTime;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
        }

        return;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the item collides with the player and the item is a coin then increase the currency by 1 and destroy the item.
        if (collision.CompareTag("Player") && this.gameObject.CompareTag("Coin") && !isCollected)
        {
            isCollected = true;

            StartCoroutine(PlayAudioAndDestroy(BloodDropSFX));

            int randomNumber = Random.Range(1, 7); // 1 - 6
            currencySystem.bloodCount += randomNumber;
        }

        // Else if the item collides with the player and the item is a food then increase the health by 1 and destroy the item.
        else if (collision.CompareTag("Player") && this.gameObject.CompareTag("Food") && !isCollected)
        {
            isCollected = true;

            StartCoroutine(PlayAudioAndDestroy(FoodDropSFX));

            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth.health < playerHealth.maxHealth)
            {
                playerHealth.health++;
                healthHeartBar.DrawHearts();
            }
        }
    }

    IEnumerator PlayAudioAndDestroy(AudioClip clip)
    {
        // Play the audio clip
        myAudio.PlayOneShot(clip);

        // Wait for a short delay before destroying the game object
        yield return new WaitForSeconds(0.2f);

        // Destroy the game object
        Destroy(this.gameObject);
               
    }

}

