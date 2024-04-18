using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinBow1 : MonoBehaviour
{
    private Goblin1 goblin1;
    public GameObject arrowPrefab1; // Reference to the arrow prefab
    public Transform arrowSpawnPoint1; // Point where the arrow spawns when shooting
    public float shootForce1 = 10f; // Force with which the arrow is shot
    public float shootRange1 = 100f; // Maximum distance at which the bow can shoot
    public Animator bowAnimator1; // Reference to the bow's Animator component
    public float cooldownDuration1 = 5f; // Cooldown duration between shots
    
    
    public float arrowLifetime1 = 4f; // Time until the arrow is destroyed after shooting
    public float arrowSpeed1 = 20f; // Speed of the arrow

    private Transform player; // Reference to the player object
    private bool canShoot = true; // Flag to indicate if the goblin can shoot

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Assuming the player has the tag "Player"
        goblin1 = GetComponentInParent<Goblin1>();
    }

    private void Update()
    {
        if (player != null && canShoot && IsPlayerInRange() && !goblin1.isDead)
        {

            Shoot();
        }
    }

    private bool IsPlayerInRange()
    {
        return Vector2.Distance(transform.position, player.position) <= shootRange1;
    }

    private void AimAtPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void Shoot()
    {
        // Trigger animation to transition to pulling bow animation
        bowAnimator1.SetTrigger("PullBow");

        if (arrowPrefab1 != null && arrowSpawnPoint1 != null)
        {
            // Instantiate the arrow at the spawn point
            GameObject arrow = Instantiate(arrowPrefab1, arrowSpawnPoint1.position, Quaternion.identity); // Set rotation to identity initially

            // Apply force to the arrow
            Vector2 direction = (player.position - transform.position).normalized;
            Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();
            arrowRb.velocity = direction * arrowSpeed1;

            // Rotate arrow to face its direction of movement
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            // Configure arrow's collision settings
            Collider2D arrowCollider = arrow.GetComponent<Collider2D>();
            
            

            // Start cooldown
            StartCoroutine(ShootCooldown());

            // Destroy the arrow after arrowLifetime seconds
            Destroy(arrow, arrowLifetime1);
        }
        else
        {
            Debug.Log("Arrow Prefab or Arrow Spawn Point is not assigned in the Bow script.");
        }
    }

    private IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(cooldownDuration1);
        canShoot = true;
    }

    

}
