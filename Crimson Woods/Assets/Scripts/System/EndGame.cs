using Fungus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    // Declaration
    [SerializeField] private PlayerHealth playerHealth;
    public GameObject resultPanel;
    public GameObject win;
    public GameObject lose;
    public TMP_Text amountText;
    public TMP_Text secondText;
    public TMP_Text minuteText;

    private bool once = false;

    private WaveSpawner waveSpawner;
    private CurrencySystem currencySystem;
    private Timer timer;

    private bool finish = false;

    private float startTime = 1f;
    private float timeBtwFrame;

    public AudioSource myAudio;
    public AudioClip WinGameSFX;
    public AudioClip LoseGameSFX;

    private void Start()
    {
        myAudio = GetComponent<AudioSource>();

        waveSpawner = GetComponent<WaveSpawner>();
        currencySystem = GetComponent<CurrencySystem>();
        timer = GetComponent<Timer>();

        timeBtwFrame = startTime;
    }

    private void Update()
    {
        if (waveSpawner.isEnd)
        {            
            CheckLoot();
        }

        if (!waveSpawner.isEnd && playerHealth.dead && !once)
        {           

            DisplayAmount();
            DisplayTime();

            if (!lose.activeSelf)
            {
                lose.SetActive(true);
            }

            StartCoroutine(WaitResult());

            myAudio.Stop();
            myAudio.PlayOneShot(LoseGameSFX);
        }

        if (!once && finish)
        {            

            DisplayAmount();
            DisplayTime();

            if (!win.activeSelf)
            {
                win.SetActive(true);
            }

            StartCoroutine(WaitResult());

            myAudio.Stop();
            myAudio.PlayOneShot(WinGameSFX);
        }
    }

    void CheckLoot()
    {
        if (GameObject.FindGameObjectWithTag("Coin") == null)
        {
            finish = true;
        }
        return;
    }

    void DisplayAmount()
    {
        amountText.text = currencySystem.bloodCount.ToString();
    }

    void DisplayTime()
    {
        secondText.text = timer.secondAmount;
        minuteText.text = timer.minuteAmount;
    }

    IEnumerator WaitResult()
    {
        // Trigger once only.
        once = true;

        yield return new WaitForSeconds(2f);      

            // Freeze the game.
            Time.timeScale = 0;

        if (!resultPanel.activeSelf)
        {
            resultPanel.SetActive(true);
        }
    }
}
