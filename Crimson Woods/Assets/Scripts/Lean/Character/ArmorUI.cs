using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorUI : MonoBehaviour
{
    public Sprite fullArmor, emptyArmor;
    Image ArmorImage;

    private void Awake()
    {
        ArmorImage = GetComponent<Image>();
    }

    public void SetArmorImage(ArmorStatus status)
    {
        switch (status)
        {
            case ArmorStatus.Empty:
                ArmorImage.sprite = emptyArmor;
                break;
            case ArmorStatus.Full:
                ArmorImage.sprite = fullArmor;
                break;
        }
    }
}


public enum ArmorStatus
{
    Empty = 0,
    Full = 1
}