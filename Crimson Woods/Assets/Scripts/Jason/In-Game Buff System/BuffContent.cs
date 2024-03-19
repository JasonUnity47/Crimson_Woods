using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuffContent : MonoBehaviour
{
    // Declaration
    [Header("Reference")]


    [Header("Object")]


    [Header("Active Buff")]
    public List<Buff> activeBuffs = new List<Buff>();

    [Header("Locker")]
    public bool[] lockStatus;

    private void Start()
    {
        
    }

    private void Update()
    {

    }
}
