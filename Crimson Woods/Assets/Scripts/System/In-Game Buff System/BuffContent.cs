using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuffContent : MonoBehaviour
{
    // Declaration
    [Header("Script Reference")]
    private PlayerController playerController;
    private PlayerHealth playerHealth;
    private Shooting shooting;

    public HealthHeartBar healthHeartBar;
    public DashBar dashBar;
    public ArmorBar armorBar;

    [Header("Object Reference")]
    private Transform playerPos;
    public GameObject bowAvatar;
    public GameObject costVFX;
    public GameObject evadeVFX;
    [SerializeField] private GameObject bloodVFX;

    [Header("Active Buff")]
    public List<Buff> activeBuffs;
    public Dictionary<int, Buff> buffDictionary = new Dictionary<int, Buff>();
    public GameObject[] buffSlots;
    public GameObject[] frameSlots;
    private int frameCount = 0;

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
    public int dashChance = 25;
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

    public void DisplayBuff()
    {
        List<int> keys = new List<int>(buffDictionary.Keys);

        if (buffDictionary.Count > 0)
        {
            foreach (int key in keys)
            {
                if (buffDictionary[key] != null)
                {
                    foreach (GameObject buffSlot in buffSlots)
                    {
                        if (!buffSlot.activeSelf)
                        {
                            buffSlot.SetActive(true);
                            frameSlots[frameCount].SetActive(true);

                            Image buffImage = buffSlot.GetComponent<Image>();
                            
                            if (buffImage != null)
                            {
                                buffImage.sprite = buffDictionary[key].buffSprite;
                            }

                            buffDictionary.Remove(key);
                            frameCount++;

                            break;
                        }
                    }
                }
            }
        }

        return;
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
        dashBar.DrawDashUI();

        return;
    }

    void EtherealDash()
    {
        // Turn on Ethereal Dash.
        // 25% chance for dash to not consume charges.
        onEtherealDash = true;

        return;
    }

    void BowAvatars()
    {
        // Play spawn sound.
        FindObjectOfType<AudioManager>().Play("Bow Show");

        // Summons two bow avatars to engage enemies.
        Vector2 leftPlayer = playerPos.position + new Vector3(-2.1f, 0, 0);

        Instantiate(bowAvatar, leftPlayer, Quaternion.identity, playerPos);

        Vector2 rightPlayer = playerPos.position + new Vector3(2.7f, 0, 0);

        Instantiate(bowAvatar, rightPlayer, Quaternion.identity, playerPos);

        return;
    }

    void VitalVelocity()
    {
        // Increases health by 2 and movement speed by 10%.
        moveSpeedIncrement = playerController.moveSpeed * (10 / 100f);

        playerHealth.maxHealth += healthIncrement;
        playerController.moveSpeed += moveSpeedIncrement;
        playerController.startingMoveSpeed += moveSpeedIncrement;

        healthHeartBar.DrawHearts();
        return;
    }

    void BlitzSurge()
    {
        // Increases attack speed and movement speed by 10%.
        atkSpeedIncrement = shooting.timeBetweenFiring * (10 / 100f);
        moveSpeedIncrement = playerController.moveSpeed * (10 / 100f);

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
        // Grants +3 Armor (+1 Armor every 15 seconds).
        onArmoredFortitude = true;
        armorBar.ActivateArmorBar();

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
                if (playerHealth.health < playerHealth.maxHealth)
                {
                    // Play sound effect.
                    FindObjectOfType<AudioManager>().Play("Suck Blood");

                    playerHealth.health++;
                    healthHeartBar.DrawHearts();
                }
                Destroy(bloodEffect, 0.5f);
            }

            return;
        }
    }
}
