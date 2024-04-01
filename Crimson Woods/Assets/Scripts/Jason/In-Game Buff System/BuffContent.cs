using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BuffContent : MonoBehaviour
{
    // Declaration
    [Header("Script Reference")]
    private PlayerController playerController;
    private PlayerHealth playerHealth;
    private Shooting shooting;

    [Header("Object Reference")]
    private Transform playerPos;
    public GameObject bowAvatar;
    [SerializeField] private GameObject costVFX;
    public GameObject evadeVFX;
    [SerializeField] private GameObject bloodVFX;

    [Header("Active Buff")]
    public List<Buff> activeBuffs;

    [Header("Locker")]
    public bool[] lockStatus;

    [Header("Check")]
    public bool onEtherealDash = false;
    public bool onPiercingArrows = false;
    public bool onEvasiveManeuvers = false;
    public bool onArmoredFortitude = false;
    public bool onVampiricEssence = false;
    public bool canCheck = true;

    [Header("Stats")]
    [SerializeField] private float dashReduction = 1 - (30 / 100f);
    [SerializeField] private int dashIncrement = 1;
    [SerializeField] private int dashChance = 25;
    public int dodgeChance = 25;
    [SerializeField] private int healthIncrement = 2;
    private float moveSpeedIncrement;
    private float atkSpeedIncrement;
    [SerializeField] private int healChance = 25;

    private void Start()
    {
        playerController = GameObject.FindAnyObjectByType<PlayerController>().GetComponent<PlayerController>();
        playerHealth = GameObject.FindAnyObjectByType<PlayerHealth>().GetComponent<PlayerHealth>();
        shooting = GameObject.FindAnyObjectByType<Shooting>().GetComponent<Shooting>();
        playerPos = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        if (onEtherealDash)
        {
            CostDash();
        }
    }

    public void BuffDetect()
    {
        foreach (Buff buff in activeBuffs)
        {
            switch (buff.buffId)
            {
                case 0:
                    {
                        buff.ApplyBuff = VitalVelocity;
                        break;
                    }

                case 1:
                    {
                        buff.ApplyBuff = BlitzSurge;
                        break;
                    }

                case 2:
                    {
                        buff.ApplyBuff = ArmoredFortitude;
                        break;
                    }

                case 3:
                    {
                        buff.ApplyBuff = SwiftSurge;
                        break;
                    }

                case 4:
                    {
                        buff.ApplyBuff = EtherealDash;
                        break;
                    }

                case 5:
                    {
                        buff.ApplyBuff = VampiricEssence;
                        break;
                    }

                case 6:
                    {
                        buff.ApplyBuff = SwiftDash;
                        break;
                    }

                case 7:
                    {
                        buff.ApplyBuff = PiercingArrows;
                        break;
                    }

                case 8:
                    {
                        buff.ApplyBuff = EvasiveManeuvers;
                        break;
                    }

                case 9:
                    {
                        buff.ApplyBuff = BowAvatars;
                        break;
                    }
            }
        }

        foreach (Buff buff in activeBuffs)
        {
            if (lockStatus[buff.buffId])
            {
                continue;
            }

            lockStatus[buff.buffId] = true;

            if (buff.ApplyBuff != null)
            {
                buff.ApplyBuff.Invoke(); // Invoke the assigned method
            }

        }
    }

    void SwiftSurge()
    {
        // Dash cooldown reduced by 30%.
        playerController.dashRestoreTime *= dashReduction;

        return;
    }

    void SwiftDash()
    {
        // Grants an additional dash.
        playerController.maxDashes += dashIncrement;

        return;
    }

    void EtherealDash()
    {
        // Turn on Ethereal Dash.
        // 25% chance for dash to not consume charges.
        onEtherealDash = true;

        return;
    }

    void CostDash()
    {
        // 25% chance for dash to not consume charges (Increase back the dash count if the dash count decrease by 1).
        if (!playerController.isDashing)
        {
            canCheck = true;
        }

        if (playerController.isDashing && canCheck)
        {
            canCheck = false;

            int randomNumber = Random.Range(0, 101);
            Debug.Log(randomNumber);

            if (randomNumber <= dashChance)
            {
                GameObject costEffect = Instantiate(costVFX, playerPos.position, playerPos.rotation, playerPos);
                playerController.dashCount++;
                Destroy(costEffect, 0.6f);
            }
        }

        return;
    }

    void BowAvatars()
    {
        // Summons two bow avatars to engage enemies.
        Vector2 leftPlayer = playerPos.position + new Vector3(-2.1f, 0, 0);

        Instantiate(bowAvatar, leftPlayer, Quaternion.identity, playerPos);

        Vector2 rightPlayer = playerPos.position + new Vector3(2.7f, 0, 0);

        Instantiate(bowAvatar, rightPlayer, Quaternion.identity, playerPos);

        return;
    }

    void VitalVelocity()
    {
        // Increases health by 2 and movement speed by 20%.
        moveSpeedIncrement = playerController.moveSpeed * (20 / 100f);

        playerHealth.maxHealth += healthIncrement;
        playerController.moveSpeed += moveSpeedIncrement;
        playerController.startingMoveSpeed += moveSpeedIncrement;

        return;
    }

    void BlitzSurge()
    {
        // Increases attack speed and movement speed by 20%.
        atkSpeedIncrement = shooting.timeBetweenFiring * (20 / 100f);
        moveSpeedIncrement = playerController.moveSpeed * (20 / 100f);

        shooting.timeBetweenFiring -= atkSpeedIncrement;
        playerController.moveSpeed += moveSpeedIncrement;
        playerController.startingMoveSpeed += moveSpeedIncrement;

        return;
    }

    void PiercingArrows()
    {
        // Turn on Penetrating Arrows.
        // Arrows penetrate through up to 3 enemies.
        onPiercingArrows = true;

        return;
    }

    void EvasiveManeuvers()
    {
        // Turn on Evasive Maneuvers.
        // 25% chance to evade damage when attacked by enemies.
        onEvasiveManeuvers = true;

        return;
    }

    void ArmoredFortitude()
    {
        // Turn on Armored Fortitude.
        // Grants +5 Armor (+1 Armor every 8 seconds).
        onArmoredFortitude = true;

        return;
    }

    void VampiricEssence()
    {
        // Turn on Vampiric Essence.
        // 25% chance to regain 1 HP upon killing an enemy.
        onVampiricEssence = true;

        return;
    }

    public void DetectDead()
    {
        int randomNumber = Random.Range(0, 101);

        if (randomNumber <= healChance)
        {
            if (playerHealth.health < playerHealth.maxHealth)
            {
                GameObject bloodEffect = Instantiate(bloodVFX, playerPos.position, playerPos.rotation, playerPos);
                playerHealth.health++;
                Destroy(bloodEffect, 0.5f);
            }

            return;
        }
    }
}
