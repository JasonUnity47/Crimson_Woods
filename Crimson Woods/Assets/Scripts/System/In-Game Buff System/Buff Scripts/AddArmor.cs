using System;
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

    private BuffContent buffContent;

    public ArmorBar armorBar;

    public GameObject blockVFX;

    public static event Action OnArmoredFortitudeChanged;

    private void Start()
    {
        buffContent = GameObject.FindWithTag("Game Manager").GetComponent<BuffContent>();

        armor = maxArmor;

        timeBtwFrame = startTime;
    }

    private void Update()
    {
        if (buffContent.onArmoredFortitude)
        {
            // Restore armor over time.
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

        if (armor <= 0)
        {
            armor = 0;
        }

        if (timeBtwFrame <= 0)
        {
            armor++;
            armorBar.DrawArmorUI();
            OnArmoredFortitudeChanged?.Invoke();
            timeBtwFrame = startTime;
        }

        else
        {
            timeBtwFrame -= Time.deltaTime;
        }

        return;
    }
}
