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

    [Header("Object Reference")]


    [Header("Active Buff")]
    public List<Buff> activeBuffs;

    [Header("Locker")]
    public bool[] lockStatus;

    [Header("Check")]
    public bool onEtherealDash = false;
    public bool canCheck = true;

    [Header("Stats")]
    [SerializeField] private float reduction = 1 - (30 / 100f);
    [SerializeField] private int dashIncrement = 1;
    [SerializeField] private int dashChance = 25;

    private void Start()
    {
        playerController = GameObject.FindAnyObjectByType<PlayerController>();
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
                        //buff.ApplyBuff =
                        break;
                    }

                case 1:
                    {
                        //buff.ApplyBuff =
                        break;
                    }

                case 2:
                    {
                        //buff.ApplyBuff =
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
                        //buff.ApplyBuff =
                        break;
                    }

                case 6:
                    {
                        buff.ApplyBuff = SwiftDash;
                        break;
                    }

                case 7:
                    {
                        //buff.ApplyBuff =
                        break;
                    }

                case 8:
                    {
                        //buff.ApplyBuff =
                        break;
                    }

                case 9:
                    {
                        //buff.ApplyBuff =
                        break;
                    }

                case 10:
                    {
                        //buff.ApplyBuff =
                        break;
                    }

                case 11:
                    {
                        //buff.ApplyBuff =
                        break;
                    }

                case 12:
                    {
                        //buff.ApplyBuff =
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
        playerController.dashRestoreTime *= reduction;

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
                playerController.dashCount++;
            }
        }

        return;
    }
}
