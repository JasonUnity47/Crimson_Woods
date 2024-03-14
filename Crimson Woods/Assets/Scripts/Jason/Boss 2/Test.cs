using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss2"))
        {
            Boss2 boss2 = collision.GetComponent<Boss2>();

            boss2.TakeDamage(2);
        }    
    }
}
