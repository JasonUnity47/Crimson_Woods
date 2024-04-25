using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorBar : MonoBehaviour
{
    public GameObject armorPrefab;
    public AddArmor addArmor;
    public PlayerHealth playerHealth;


    List<ArmorUI> armorIcons = new List<ArmorUI>();

    private void OnEnable()
    {
        PlayerHealth.OnArmoredFortitudeChanged += UpdateArmorUI;
    }

    private void OnDisable()
    {
        PlayerHealth.OnArmoredFortitudeChanged -= UpdateArmorUI;
    }

    public void Start()
    {
        DrawArmorUI();
    }

    public void DrawArmorUI()
    {
        ClearArmorIcons();
        int maxArmor = (int)addArmor.maxArmor;
        for (int i = 0; i < maxArmor; i++)
        {
            CreateEmptyArmorIcon();
        }

        UpdateArmorUI();
    }

    public void UpdateArmorUI()
    {
        int remainingArmor = (int)addArmor.armor;
        for (int i = 0; i < armorIcons.Count; i++)
        {
            if (i < remainingArmor)
            {
                armorIcons[i].SetArmorImage(ArmorStatus.Full);
            }
            else
            {
                armorIcons[i].SetArmorImage(ArmorStatus.Empty);
            }
        }
    }

    public void CreateEmptyArmorIcon()
    {
        GameObject newArmorIcon = Instantiate(armorPrefab);
        newArmorIcon.transform.SetParent(transform);

        ArmorUI armorIconComponent = newArmorIcon.GetComponent<ArmorUI>();
        armorIcons.Add(armorIconComponent);
    }


    public void ClearArmorIcons()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        armorIcons.Clear();
    }
}
