using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WeaponThrow : MonoBehaviour
{
    // Damage
    [Header("Damage")]
    public float damage;

    public float throwSpeed;
    public float rotationSpeedFactor = 16.0f;
    private Rigidbody2D rb;
    private BloodGoblin bloodGoblin;
    private Vector3 throwDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bloodGoblin = GameObject.FindAnyObjectByType<BloodGoblin>();

        throwDirection = ((Vector3)bloodGoblin.lastTargetPosForThrow - transform.position).normalized; // Calculate the normalized direction

        Destroy(this.gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = throwDirection * throwSpeed;
        float rotationSpeed = 180 * rotationSpeedFactor * Time.deltaTime;
        transform.Rotate(Vector3.forward, rotationSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            playerHealth.TakeDamage(damage);
        }
    }
}
