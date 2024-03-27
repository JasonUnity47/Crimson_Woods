using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash : MonoBehaviour
{
    public Sprite fullDash, emptyDash;
    Image DashImage;

    private void Awake()
    {
        DashImage = GetComponent<Image>();
    }

    public void SetDashImage(DashStatus status)
    {
        switch (status)
        {
            case DashStatus.Empty:
                DashImage.sprite = emptyDash;
                break;          
            case DashStatus.Full:
                DashImage.sprite = fullDash;
                break;
        }
    }
}


public enum DashStatus
{
    Empty = 0,
    Full = 2
}