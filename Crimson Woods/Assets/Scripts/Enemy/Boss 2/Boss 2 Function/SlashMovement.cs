using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SlashMovement : MonoBehaviour
{
    public float slashSpeed;

    private Boss2 boss2;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        boss2 = GameObject.FindAnyObjectByType<Boss2>();

        Quaternion rotation = Quaternion.LookRotation(transform.position - (Vector3)boss2.lastTargetPosForSlash, transform.TransformDirection(Vector3.forward));

        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);

        Destroy(this.gameObject, 1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.position = Vector2.Lerp((Vector2)rb.position, boss2.lastTargetPosForSlash, slashSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            playerHealth.TakeDamage(1f);
        }
    }
}
