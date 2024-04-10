using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static event Action OnPlayerDamaged;
    public static event Action OnPlayerDied;
    //public static event Action OnMaxHealthChanged;
    public float health, maxHealth;
    [SerializeField] BuffContent buffContent;
    [SerializeField] private int numberOfFlashes;
    [SerializeField] private float iFramesDuration;

    private Animator myAnimator;
    private bool dead;
    private SpriteRenderer mySpriteRender;
    private Collider2D myCollider;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        health = maxHealth;
    }

    //public void IncreaseMaxHealth(float amount)
    //{
    //    maxHealth += amount;
    //    OnMaxHealthChanged?.Invoke(); // Invoke the event when max health changes
    //}

    public void TakeDamage(float amount)
    {
        // IF Evasive Maneuvers Buff is ON THEN can dodge damage.
        if (buffContent.onEvasiveManeuvers)
        {
            int randomNumber = UnityEngine.Random.Range(0, 101);

            Debug.Log(randomNumber);

            // IF meet 25% chance THEN dodge damage.
            if (randomNumber <= buffContent.dodgeChance)
            {
                // Dodge Effect.
                GameObject evadeEffect = Instantiate(buffContent.evadeVFX, transform.position, transform.rotation, transform);
                Destroy(evadeEffect, 0.5f);
                return;
            }

        }

        health -= amount;
        health = Mathf.Max(0, health);
        OnPlayerDamaged?.Invoke();
        myAnimator.SetTrigger("HurtTrigger");
        StartCoroutine(Invulnerability());

        if (health == 0 && !dead)
        {
            OnPlayerDied?.Invoke();
            myAnimator.SetTrigger("DeadTrigger");            
            GetComponent<PlayerController>().enabled = false;
            myCollider.enabled = false;
            dead = true;
        }
    }

    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(6, 7, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            mySpriteRender.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            mySpriteRender.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(6, 7, false);
    }
}
