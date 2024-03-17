using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public List<Loot> lootList = new List<Loot>();

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
            }
        }

        if (possibleItems.Count > 0)
        {
            Loot droppedItem;

            // IF the number of loots are more than 1 THEN drop the rarest loot.
            if (possibleItems.Count > 1)
            {
                foreach (Loot item in possibleItems)
                {
                    if (item.lootName == "Food")
                    {
                        droppedItem = item;
                        return droppedItem;
                    }
                }
            }

            // ELSE drop the only loot.
            else
            {
                droppedItem = possibleItems[0];
                return droppedItem;
            }
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
            
            if (droppedItem.lootName == "Food")
            {
                lootGameObject.tag = "Food";
            }

            else if (droppedItem.lootName == "Blood Coin")
            {
                lootGameObject.tag = "Coin";
            }
        }
    }
}
