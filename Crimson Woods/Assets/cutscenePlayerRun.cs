using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cutscenePlayerRun : MonoBehaviour
{
    public AudioSource myAudio;
    public AudioClip CutPlayerRunSFX;
    private bool hasPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasPlayed)
        {
            StartCoroutine(PlayDelayedSound());
            hasPlayed = true;
        }
    }

    IEnumerator PlayDelayedSound()
    {
        yield return new WaitForSeconds(8f); // Wait for 3 seconds
        myAudio.PlayOneShot(CutPlayerRunSFX);
    }
}
