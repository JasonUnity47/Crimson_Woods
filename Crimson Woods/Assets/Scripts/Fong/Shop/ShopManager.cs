using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public int coins;
    public TMP_Text coinUI;
    public ShopItemSO[] shopItemSO;
    public GameObject[] shopPanelsGO;
    public ShopTemplate[] shopPanels;
    public Button[] myPurchaseBtns;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < shopItemSO.Length; i++)
            shopPanelsGO[i].SetActive(true);
        coinUI.text = "Bloods: " + coins.ToString();
        LoadPanels();
        CheckPurchaseable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCoins()
    {
        coins+=1000;
        coinUI.text = "Bloods: " + coins.ToString();
        CheckPurchaseable();
    }

    public void CheckPurchaseable()
    {
        for (int i = 0; i < shopItemSO.Length; i++)
        {
            if (coins >= shopItemSO[i].baseCost && shopItemSO[i].progress < 5)
                myPurchaseBtns[i].interactable = true;
            else
                myPurchaseBtns[i].interactable = false;
        }
    }

    public void PurchaseAbility(int btnNo)
    {
        if (coins >= shopItemSO[btnNo].baseCost && shopItemSO[btnNo].progress < 5)
        {
            coins = coins - shopItemSO[btnNo].baseCost;
            shopItemSO[btnNo].AddProgress();
            LoadPanels();
            coinUI.text = "Bloods: " + coins.ToString();
            CheckPurchaseable();
        }
    }

    public void LoadPanels()
    {
        for (int i = 0; i < shopItemSO.Length; i++)
        {
            shopPanels[i].titleTxt.text = shopItemSO[i].title;
            shopPanels[i].descriptionTxt.text = shopItemSO[i].description;
            if (shopItemSO[i].progress < 5)
                shopPanels[i].costTxt.text = "Bloods: " + shopItemSO[i].baseCost.ToString();
            else
                shopPanels[i].costTxt.text = " MAX ";
        }
    }
}