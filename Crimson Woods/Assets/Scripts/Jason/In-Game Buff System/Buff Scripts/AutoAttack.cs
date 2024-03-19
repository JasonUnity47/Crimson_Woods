using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AutoAttack : MonoBehaviour
{
    public GameObject closestEnemy;
    public GameObject[] enemies;

    public GameObject arrow;

    public float attackTime;
    public float attackCD;

    private float distanceToEnemy;
    private float distanceToClosestEnemy;
    private Vector2 direction;

    private void Start()
    {
        attackCD = attackTime;
    }

    private void Update()
    {
        DetectEnemy();

        if (enemies.Length > 0)
        {
            AttackEnemy();
        }
    }

    void DetectEnemy()
    {
        distanceToClosestEnemy = Mathf.Infinity; // Definitely will replace by any distance.
        closestEnemy = null;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            // Use sqrMagnitude is for performance issue.
            distanceToEnemy = (enemy.transform.position - this.transform.position).sqrMagnitude; // Calculate the distance between this object and each enemy to find the closest enemy.

            if (distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = enemy;
            }

            
        }

        direction = closestEnemy.transform.position - this.transform.position;

        Quaternion rotation = Quaternion.LookRotation(transform.position - (Vector3)direction, transform.TransformDirection(Vector3.forward));

        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
    }

    void AttackEnemy()
    {
        if (attackCD <= 0)
        {
            attackCD = attackTime;
            Instantiate(arrow, transform.position, Quaternion.identity);
        }

        else
        {
            attackCD -= Time.deltaTime;
        }
    }
}
