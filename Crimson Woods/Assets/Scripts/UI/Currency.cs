using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Currency : MonoBehaviour
{
    public CurrencySystem currencySystem; // Reference to the CurrencySystem script
    public TextMeshProUGUI currencyText; // Reference to the TextMeshPro text object

    void Update()
    {
        UpdateCurrencyText();
    }

    void UpdateCurrencyText()
    {
        // Update the TextMeshPro text with the current currency count
        if (currencyText != null && currencySystem != null)
        {
            currencyText.text = currencySystem.bloodCount.ToString();
        }
    }
}
