using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CurrencySystem : MonoBehaviour
{
    // This script is only for each new round.

    // Declaration
    public int bloodCount;
    private int bloodSave;

    private void Start()
    {
        bloodCount = 0;
        LoadBloodSave();
    }

    private void OnApplicationQuit()
    {
        // Save fodSave when application quits
        SaveBloodSave();
    }

    
    private void SaveBloodSave()
    {
        bloodSave += bloodCount;
        StartCoroutine(SaveBloodsToServer());
    }

    private void LoadBloodSave()
    {
        StartCoroutine(LoadBloodsFromServer());
    }



    IEnumerator SaveBloodsToServer()
    {
        // Create form with userId and bloods
        WWWForm form = new WWWForm();
        form.AddField("userId", PlayerPrefs.GetInt("userId"));
        form.AddField("bloods", bloodSave);

        // Send request to PHP script
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/cwgd/save_bloods.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
        }
    }

    IEnumerator LoadBloodsFromServer()
    {
        // URL with userId parameter
        string url = $"http://localhost/cwgd/load_bloods.php?userId={PlayerPrefs.GetInt("userId")}";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                int bloods = int.Parse(www.downloadHandler.text);
                bloodSave = bloods;
            }
            else
            {
                Debug.LogError(www.error);
            }
        }
    }
}
