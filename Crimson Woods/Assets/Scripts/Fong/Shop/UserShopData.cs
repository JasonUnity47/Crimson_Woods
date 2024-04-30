using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class UserShopData
{
    public int dashCD;
    public int fireRate;
    public int health;
    public int moveSpeed;
    public int bloods;

    public UserShopData(int dashCD, int fireRate, int health, int moveSpeed)
    {
        this.dashCD = dashCD;
        this.fireRate = fireRate;
        this.health = health;
        this.moveSpeed = moveSpeed;
        this.bloods = bloods;
    }
}
