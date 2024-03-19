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

    private void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
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
                        //buff.ApplyBuff =
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
        float reduction = 1 - (30 / 100f);
        //playerController.dashCD *= reduction;

        return;
    }

    void SwiftDash()
    {

        return;
    }
}
