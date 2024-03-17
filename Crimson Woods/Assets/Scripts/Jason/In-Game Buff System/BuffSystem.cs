using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
    // Declaration
    public List<Buff> buffList = new List<Buff>();
    public List<int> idList = new List<int>();

    private int initialTotalBuffs;
    private int count = 0;

    private void Start()
    {
        initialTotalBuffs = buffList.Count;

        foreach (Buff buff in buffList)
        {
            idList.Add(buff.buffId);
        }
    }

    private void Update()
    {
        
    }

    void RandomBuff()
    {
        if (buffList.Count == 0)
        {
            return;
        }

        int randomNumber = Random.Range(0, initialTotalBuffs);

        if (idList.Contains(randomNumber) == false)
        {
            return;
        }

        if (count != 3 && buffList.Count >= 3)
        {
            foreach (Buff buff in buffList)
            {
                if (buff.buffId == randomNumber)
                {
                    idList.Remove(randomNumber);
                    count++;
                }
            }
        }

        else if (buffList.Count < 3 && idList.Count != 0)
        {
            foreach (Buff buff in buffList)
            {
                if (buff.buffId == randomNumber)
                {
                    idList.Remove(randomNumber);
                    count++;
                }
            }
        }
        return;
    }

    public void ChooseBuff()
    {
        int deletedId = 0;

        foreach (Buff buff in buffList)
        {
            if (buff.buffId == deletedId)
            {
                buffList.Remove(buff);
            }
        }

        idList.Sort();

        ResetCurrent();
    }

    void ResetCurrent()
    {
        count = 0;
    }
}
