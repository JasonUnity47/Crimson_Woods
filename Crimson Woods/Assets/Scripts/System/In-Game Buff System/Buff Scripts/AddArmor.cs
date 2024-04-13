using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddArmor : MonoBehaviour
{
    // Declaration
    [Header("Armor")]
    public float armor;
    public float maxArmor;

    [Header("Timer")]
    public float startTime;
    private float timeBtwFrame;

    [Header("Check")]
    public float tempHealth;

    private BuffContent buffContent;
    private PlayerHealth playerHealth;

    [SerializeField] private GameObject blockVFX;

    private void Start()
    {
        buffContent = GameObject.FindWithTag("Game Manager").GetComponent<BuffContent>();

        playerHealth = GetComponent<PlayerHealth>();

        armor = maxArmor;

        tempHealth = playerHealth.maxHealth;

        timeBtwFrame = startTime;
    }

    private void Update()
    {
        if (buffContent.onArmoredFortitude)
        {
            BlockDamage();

            // Check whether player's health have changed.
            tempHealth = playerHealth.health;

            RestoreArmor();
        }
    }

    void RestoreArmor()
    {
        if (armor >= maxArmor)
        {
            armor = maxArmor;
            return;
        }

        if (timeBtwFrame <= 0)
        {
            armor++;
            timeBtwFrame = startTime;
        }

        else
        {
            timeBtwFrame -= Time.deltaTime;
        }

        return;
    }

    void BlockDamage()
    {
        if (tempHealth != playerHealth.health && tempHealth > playerHealth.health && armor > 0)
        {
            GameObject blockEffect = Instantiate(blockVFX, transform.position, transform.rotation, transform);

            armor -= tempHealth - playerHealth.health;

            playerHealth.health = tempHealth;

            Destroy(blockEffect, 1f);
        }

        if (armor <= 0)
        {
            armor = 0;
        }

        return;
    }
}
