using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBar : MonoBehaviour
{
    public GameObject dashPrefab;
    public PlayerController playerController;
    List<Dash> dashIcons = new List<Dash>();

    private void OnEnable()
    {
        PlayerController.OnDashCountChanged += UpdateDashUI;
    }

    private void OnDisable()
    {
        PlayerController.OnDashCountChanged -= UpdateDashUI;
    }

    public void Start()
    {
        DrawDashUI();
    }

    public void DrawDashUI()
    {
        ClearDashIcons();
        int maxDashes = playerController.maxDashes;
        for (int i = 0; i < maxDashes; i++)
        {
            CreateEmptyDashIcon();
        }

        UpdateDashUI();
    }

    public void UpdateDashUI()
    {
        int remainingDashes = playerController.dashCount;
        for (int i = 0; i < dashIcons.Count; i++)
        {
            if (i < remainingDashes)
            {
                dashIcons[i].SetDashImage(DashStatus.Full);
            }
            else
            {
                dashIcons[i].SetDashImage(DashStatus.Empty);
            }
        }
    }

    public void CreateEmptyDashIcon()
    {
        GameObject newDashIcon = Instantiate(dashPrefab);
        newDashIcon.transform.SetParent(transform);

        Dash dashIconComponent = newDashIcon.GetComponent<Dash>();
        dashIcons.Add(dashIconComponent);
    }


    public void ClearDashIcons()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        dashIcons.Clear();
    }
}


