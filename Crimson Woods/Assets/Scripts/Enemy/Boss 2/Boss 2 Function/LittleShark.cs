using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleShark : MonoBehaviour
{
    private Collider2D player;
    public Transform attackArea;
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask whatIsPlayer;

    public GameObject arrow;

    public float attackTime;
    public float attackCD;

    private Vector2 directionToPlayer;

    private Boss2 boss2;

    private void Start()
    {
        boss2 = GetComponentInParent<Boss2>();

        attackCD = attackTime;
    }

    private void Update()
    {
        if (!boss2.isDead)
        {
            DetectEnemy();

            if (player != null)
            {
                AttackEnemy();
            }
        }
    }

    void DetectEnemy()
    {
        player = Physics2D.OverlapCircle(attackArea.position, attackRadius, whatIsPlayer);

        if (player != null)
        {
            Vector2 playerPos = player.transform.position;
            directionToPlayer = playerPos - (Vector2)transform.position;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void AttackEnemy()
    {
        if (attackCD <= 0)
        {
            attackCD = attackTime;
            fire();
        }

        else
        {
            attackCD -= Time.deltaTime;
        }
    }

    void fire()
    {
        GameObject new_bullet = Instantiate(arrow, transform.position, Quaternion.identity);
        //rotate the bullet as the gameobject
        new_bullet.transform.right = transform.right.normalized;
    }
}
