using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class ShopManager : MonoBehaviour
{
    
    public int userId; // Add userId variable


    public int coins;
    public TMP_Text coinUI;
    public ShopItemSO[] shopItemSO;
    public Slider[] slider;
    public GameObject[] shopPanelsGO;
    public ShopTemplate[] shopPanels;
    public Button[] myPurchaseBtns;

    // Start is called before the first frame update
    void Start()
    {
        // Retrieve userId from PlayerPrefs
        userId = PlayerPrefs.GetInt("userId", 0);

     

        for (int i = 0; i < shopItemSO.Length; i++)
        {
            shopPanelsGO[i].SetActive(true);
        }

        // Retrieve user data from the database
        StartCoroutine(GetUserData());

        LoadPanels();
        CheckPurchaseable();
    }

    IEnumerator GetUserData()
    {
        string url = "http://localhost/cwgd/shop.php?userId=" + userId; // Include userId parameter in the URL
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                string userDataJson = www.downloadHandler.text;
                Debug.Log("User Data JSON: " + userDataJson);

                // Parse the JSON data to extract user's data
                UserShopData userData = JsonUtility.FromJson<UserShopData>(userDataJson);

                if (userData != null)
                {
                    // Update progresses of the ability panels
                    shopItemSO[0].progress = userData.dashCD;
                    shopItemSO[1].progress = userData.fireRate;
                    shopItemSO[2].progress = userData.health;
                    shopItemSO[3].progress = userData.moveSpeed;
                    coins = userData.bloods;
                    coinUI.text = "Bloods: " + coins.ToString();
                    CheckPurchaseable();


                    // Update slider values with retrieved data
                    for (int i = 0; i < slider.Length; i++)
                    {
                        slider[i].value = shopItemSO[i].progress;
                    }
                }
                else
                {
                    Debug.Log("Invalid user data format.");
                }
            }
        }
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
        int cost = shopItemSO[btnNo].baseCost; // Get the cost of the ability
        if (coins >= cost && shopItemSO[btnNo].progress < 5)
        {
            coins -= cost;
            if (shopItemSO[btnNo].progress < 5)
            {
                shopItemSO[btnNo].progress++;
                slider[btnNo].value = shopItemSO[btnNo].progress;
                
            }
            LoadPanels();
            coinUI.text = "Bloods: " + coins.ToString();
            CheckPurchaseable();
        }

        // Send request to upgrade ability with cost
        string action = shopItemSO[btnNo].title; // Create action based on ability title
        StartCoroutine(Upgrade(userId, action, cost));
        Debug.Log(action);
    }

    public void LoadPanels()
    {
        for (int i = 0; i < shopItemSO.Length; i++)
        {
            shopPanels[i].titleTxt.text = shopItemSO[i].title;
            shopPanels[i].descriptionTxt.text = shopItemSO[i].description;
            if (shopItemSO[i].progress < 5)
            {
                if (shopItemSO[i].progress == 0)
                {
                    shopItemSO[i].baseCost = 500;
                }
                else if (shopItemSO[i].progress == 1)
                {
                    shopItemSO[i].baseCost = 1500;
                }
                else if (shopItemSO[i].progress == 2)
                {
                    shopItemSO[i].baseCost = 2500;
                }
                else if (shopItemSO[i].progress == 3)
                {
                    shopItemSO[i].baseCost = 3500;
                }
                else if (shopItemSO[i].progress == 4)
                {
                    shopItemSO[i].baseCost = 4500;
                }
                shopPanels[i].costTxt.text = "Bloods: " + shopItemSO[i].baseCost.ToString();
            }
            else
                shopPanels[i].costTxt.text = " MAX ";

            slider[i].value = shopItemSO[i].progress;
        }
    }


    // URL of the PHP script to update 
    private string phpURL = "http://localhost/cwgd/upgrade.php";
    private IEnumerator Upgrade(int userId, string action, int cost)
    {
        // Create form with userId and action
        WWWForm form = new WWWForm();
        form.AddField("userId", userId);
        form.AddField("action", action);
        form.AddField("cost", cost);

        // Send request to PHP script
        using (UnityWebRequest www = UnityWebRequest.Post(phpURL, form))
        {
            yield return www.SendWebRequest();

            // Check for errors
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                // Print response
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    //// Function to upgrade dashCD
    //public void UpgradeDashCD()
    //{
    //    // Retrieve userId from PlayerPrefs
    //    //int userId = PlayerPrefs.GetInt("userId");

    //    // Send request to upgrade dashCD
    //    StartCoroutine(Upgrade(userId, "upgradeDashCD"));
    //}

    //// Function to upgrade fireRate
    //public void UpgradeFireRate()
    //{
    //    // Retrieve userId from PlayerPrefs
    //    //int userId = PlayerPrefs.GetInt("userId");

    //    // Send request to upgrade fireRate
    //    StartCoroutine(Upgrade(userId, "upgradeFireRate"));
    //}

    //// Function to upgrade health
    //public void UpgradeHealth()
    //{
    //    // Retrieve userId from PlayerPrefs
    //    //int userId = PlayerPrefs.GetInt("userId");

    //    // Send request to upgrade health
    //    StartCoroutine(Upgrade(userId, "upgradeHealth"));
    //}

    //// Function to upgrade moveSpeed
    //public void UpgradeMoveSpeed()
    //{
    //    // Retrieve userId from PlayerPrefs
    //    //int userId = PlayerPrefs.GetInt("userId");

    //    // Send request to upgrade moveSpeed
    //    StartCoroutine(Upgrade(userId, "upgradeMoveSpeed"));
    //}
}
