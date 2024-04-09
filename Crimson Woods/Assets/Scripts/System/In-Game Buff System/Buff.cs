using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buff", menuName = "Buffs/In-Game Buffs")]
public class Buff : ScriptableObject
{
    public int buffId;
    public Sprite buffSprite;
    public string buffName;
    [TextArea(3,3)] public string buffDescription;
    public Action ApplyBuff;
}
