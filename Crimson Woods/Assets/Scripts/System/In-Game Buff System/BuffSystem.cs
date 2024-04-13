using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffSystem : MonoBehaviour
{
    // Class
    [System.Serializable]
    public class BuffUI
    {
        public string name;
        public TMP_Text bId;
        public TMP_Text bName;
        public Image bIcon;
        public TMP_Text bDes;
    }

    // Declaration
    [Header("Object Reference")]
    public GameObject buffPanel;

    [Header("Script Reference")]
    public BuffContent buffContent;

    [Header("Total Buff List")]
    public List<Buff> buffList;

    [Header("Number List")]
    public List<int> numberList;

    [Header("UI")]
    public BuffUI[] UIList;
    public List<TMP_Text> IdList;

    [Header("Variable")]
    private int initialTotalBuffCount = 0;
    public int buffCount = 0;
    private int randomNumber;
    private bool isActive = false;

    private void Start()
    {
        // Get the initial count of the total buffs.
        initialTotalBuffCount = buffList.Count;

        // Initialize the lock status of each buff.
        buffContent.lockStatus = new bool[buffList.Count];

        for (int i = 0; i < buffList.Count; i++)
        {
            numberList.Add(i);
        }
    }

    private void Update()
    {
        //ActivatePanel();

        if (buffPanel.activeSelf && buffCount != 3)
        {
            RandomBuff();
        }
    }

    void ActivatePanel()
    {
        if (Input.GetKeyDown(KeyCode.B) && !buffPanel.activeSelf)
        {
            isActive = !isActive;
            buffPanel.SetActive(isActive);
        }
    }

    void RandomBuff()
    {
        // IF buff list is empty THEN do nothing.
        if (buffList.Count == 0)
        {
            return;
        }

        // Generate a random number
        randomNumber = UnityEngine.Random.Range(0, initialTotalBuffCount); // 0 - (Total buff count - 1)

        // CHECK whether the random number is in the number list OR random number is same as last number.
        // CHECK whether the number appears before.
        if (numberList.Contains(randomNumber) == false)
        {
            return;
        }

        // IF the three buff slot are not full.
        if (buffCount != 3 && buffList.Count >= 3)
        {
            foreach (Buff buff in buffList)
            {
                if (buff.buffId == randomNumber)
                {
                    // Insert buff information into empty buff slot.
                    UIList[buffCount].bId.text = buff.buffId.ToString();
                    UIList[buffCount].bName.text = buff.buffName;
                    UIList[buffCount].bIcon.sprite = buff.buffSprite;
                    UIList[buffCount].bDes.text = buff.buffDescription;

                    // Next empty buff slot.
                    buffCount++;

                    // Temporarily remove the assigned buff.
                    // Avoid reusing the same buff.
                    numberList.Remove(randomNumber);
                }
            }
        }

        else if (buffList.Count < 3 && numberList.Count != 0)
        {
            foreach (Buff buff in buffList)
            {
                if (buff.buffId == randomNumber)
                {
                    // Insert buff information into empty buff slot.
                    UIList[buffCount].bId.text = buff.buffId.ToString();
                    UIList[buffCount].bName.text = buff.buffName;
                    UIList[buffCount].bIcon.sprite = buff.buffSprite;
                    UIList[buffCount].bDes.text = buff.buffDescription;

                    // Next empty buff slot.
                    buffCount++;

                    // Temporarily remove the assigned buff.
                    // Avoid reusing the same buff.
                    numberList.Remove(randomNumber);
                }
            }
        }

        return;
    }

    void ResetSlot()
    {
        // Close buff panel.
        if (buffPanel.activeSelf)
        {
            buffPanel.SetActive(false);
            isActive = false;
        }

        // Clear the buff slot index (Next time will start the first slot).
        buffCount = 0;

        // Clear all the information in the slots.
        for (int i = 0; i < 3; i++)
        {
            UIList[i].bId.text = "";
            UIList[i].bName.text = "";
            UIList[i].bIcon.sprite = null;
            UIList[i].bDes.text = "";
        }

        return;
    }

    // Button Function
    public void ChooseBuff(TMP_Text currentBuffId)
    {
        // IF current buff slot is empty THEN do nothing.
        if (currentBuffId.text == "")
        {
            return;
        }

        // Get the current upgrade id to recognize which upgrade that player chose.
        int deletedId = int.Parse(currentBuffId.text); // Convert the id number from string to int.

        // CHECK whether the chosen buff is in the buff list.
        foreach (Buff buff in buffList.ToList())
        {
            // REMOVE the chosen buff because one buff can only choose once.
            if (buff.buffId == deletedId)
            {
                buffList.Remove(buff);
                buffContent.activeBuffs.Add(buff);

                // Display Buffs
                buffContent.buffDictionary.Add(0, buff);
            }
        }

        // Check whether the chosen number (id) is in the list.
        // IF it is not THEN return back the others (other numbers/buffs that player didn't choose) to the number list.
        foreach (TMP_Text bId in IdList)
        {
            if (bId.text == "")
            {
                continue;
            }

            if (bId.text != "" && deletedId != int.Parse(bId.text))
            {
                numberList.Add(int.Parse(bId.text));
            }
        }

        // Sort the list to avoid error occurs after player chose one of the buff.
        numberList.Sort();

        // Reset all buff slots.
        ResetSlot();
    }
}
