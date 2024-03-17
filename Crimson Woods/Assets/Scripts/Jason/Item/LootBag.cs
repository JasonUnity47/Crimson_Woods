using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public List<Loot> lootList = new List<Loot>();

    public int rareDropRate = int.MinValue;

    Loot GetDroppedItem()
    {
        int randomNumber = Random.Range(1, 101); // 1 - 100
        List<Loot> possibleItems = new List<Loot>();
        Debug.Log(randomNumber);

        foreach (Loot item in lootList)
        {
            if (randomNumber <= item.dropChance)
            {
                possibleItems.Add(item);


                //if (item.dropChance > rareDropRate)
                //{
                    rareDropRate = item.dropChance;
                //}
            }
        }

        if (possibleItems.Count > 0)
        {
            //foreach (Loot item in possibleItems)
            //{
                //if (rareDropRate == item.dropChance)
                //{
                    //Loot droppedItem = item;

                    //return droppedItem;
                //}
            //}

            Loot droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
            return droppedItem;
        }

        return null;
    }

    public void InstantiateLoot(Vector3 spawnPosition)
    {
        Loot droppedItem = GetDroppedItem();

        if (droppedItem != null)
        {
            GameObject lootGameObject = Instantiate(droppedItemPrefab, spawnPosition + new Vector3(0, 1, 0), Quaternion.identity);
            lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedItem.lootSprite;
            lootGameObject.GetComponent<SpriteRenderer>().material = droppedItem.lootMaterial;
            
            //if (droppedItem.lootName == "Food")
            //{
                //lootGameObject.tag = "Food";
            //}

            //else
            //{
                //lootGameObject.tag = "Blood Coin";
            //}
        }
    }
}
