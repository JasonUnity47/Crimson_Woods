using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputButton : MonoBehaviour
{

    // Back
    public void BackStep(GameObject panel)
    {
        // Play ui sound.
        FindObjectOfType<AudioManager>().Play("Click");
        if (panel.activeSelf)
        {
            panel.SetActive(false);
            TimeResume();
        }

        return;
    }

    // Login
    public void ShowLogin(GameObject login)
    {
        // Play ui sound.
        FindObjectOfType<AudioManager>().Play("Click");
        if (!login.activeSelf)
        {
            login.SetActive(true);
            TimeStop();
        }

        return;
    }

    // Registration
    public void ShowRegistration(GameObject reg)
    {
        // Play ui sound.
        FindObjectOfType<AudioManager>().Play("Click");
        if (!reg.activeSelf)
        {
            reg.SetActive(true);
            TimeStop();
        }

        return;
    }

    // Settings
    public void ShowSettings(GameObject settings)
    {
        // Play ui sound.
        FindObjectOfType<AudioManager>().Play("Click");
        if (!settings.activeSelf)
        {
            settings.SetActive(true);
            TimeStop();
        }

        return;
    }

    // Control
    public void ShowControl(GameObject control)
    {
        // Play ui sound.
        FindObjectOfType<AudioManager>().Play("Click");
        if (!control.activeSelf)
        {
            control.SetActive(true);
            TimeStop();
        }

        return;
    }

    // Quit
    public void ShowQuit(GameObject quit)
    {
        // Play ui sound.
        FindObjectOfType<AudioManager>().Play("Click");
        if (!quit.activeSelf)
        {
            quit.SetActive(true);
            TimeStop();
        }

        return;
    }

    // Credit
    public void ShowCredit(GameObject credit)
    {
        // Play ui sound.
        FindObjectOfType<AudioManager>().Play("Click");
        if (!credit.activeSelf)
        {
            credit.SetActive(true);
            TimeStop();
        }

        return;
    }

    // In-Game Settings
    public void ShowInGame(GameObject inGame)
    {
        // Play ui sound.
        FindObjectOfType<AudioManager>().Play("Click");
        if (!inGame.activeSelf)
        {
            inGame.SetActive(true);
            TimeStop();
        }

        return;
    }

    // Resume Game
    public void ResumeGame(GameObject inGame)
    {
        // Play ui sound.
        FindObjectOfType<AudioManager>().Play("Click");
        if (inGame.activeSelf)
        {
            inGame.SetActive(false);
            TimeResume();
        }

        return;
    }

    // Cutscene 1
    public void SwitchCut1()
    {

        SceneManager.LoadScene(2);
        TimeResume();
        return;
    }

    public void ShopPanel(GameObject shop)
    {
        // Play ui sound.
        FindObjectOfType<AudioManager>().Play("Click");
        if (!shop.activeSelf)
        {
            shop.SetActive(true);
            TimeResume();
        }

        return;
    }

    public void LB(GameObject LB)
    {
        // Play ui sound.
        FindObjectOfType<AudioManager>().Play("Click");
        if (!LB.activeSelf)
        {
            LB.SetActive(true);
            TimeResume();
        }

        return;
    }

    // Back to Main
    public void BackMain()
    {
        // Play ui sound.
        FindObjectOfType<AudioManager>().Play("Click");
        SceneManager.LoadScene(1);
        TimeResume();
        return;
    }

    // Restart Game
    public void RestartGame()
    {
        // Play ui sound.
        FindObjectOfType<AudioManager>().Play("Click");
        SceneManager.LoadScene(3);
        TimeResume();
        return;
    }

    // Start
    public void StartGame()
    {
        // Play ui sound.
        FindObjectOfType<AudioManager>().Play("Click");
        SceneManager.LoadScene(1);
        TimeResume();
        return;
    }

    // Quit
    public void QuitGame()
    {
        // Play ui sound.
        FindObjectOfType<AudioManager>().Play("Click");
        Application.Quit();
        return;
    }

    // Show Panel For Check, Update, and Deletion Panel
    public void ShowPanel(GameObject panel)
    {
        // Play ui sound.
        FindObjectOfType<AudioManager>().Play("Click");
        if (!panel.activeSelf)
        {
            panel.SetActive(true);
            TimeStop();
        }

        return;
    }

    // Close Panel For Check, Update, and Deletion Panel
    public void ClosePanel(GameObject panel)
    {
        // Play ui sound.
        FindObjectOfType<AudioManager>().Play("Click");
        if (panel.activeSelf)
        {
            panel.SetActive(false);
        }

        return;
    }

    // Freeze Time
    void TimeStop()
    {
        Time.timeScale = 0;
        return;
    }

    // Unfreeze Time
    void TimeResume()
    {
        Time.timeScale = 1;
        return;
    }
}
