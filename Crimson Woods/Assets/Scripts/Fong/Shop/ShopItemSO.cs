using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopMenu", menuName = "Scriptable Object/New Shop Item", order = -1)]
public class ShopItemSO : ScriptableObject
{
    public string title;
    public string description;
    public int baseCost;
    public int progress = 0;

    void Start()
    {
        
    }

    //public void AddProgress()
    //{
    //    if (progress < 5)
    //    {
    //        progress++;
    //        baseCost += 1000;
    //    }
    //}
}
