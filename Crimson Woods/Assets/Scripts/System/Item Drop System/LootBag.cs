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

            // If the number of loots are more than 1 then drop the rarest loot.
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

            // Else drop the only loot.
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
                lootGameObject.GetComponent<TrailRenderer>().startColor = Color.green;
            }

            else if (droppedItem.lootName == "Blood Coin")
            {
                lootGameObject.tag = "Coin";
                lootGameObject.GetComponent<TrailRenderer>().startColor = Color.red;
            }
        }

        return;
    }
}
