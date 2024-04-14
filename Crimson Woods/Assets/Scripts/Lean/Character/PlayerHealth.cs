using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static event Action OnPlayerDamaged;
    public static event Action OnPlayerDied;  
    public float health, maxHealth;
    [SerializeField] BuffContent buffContent;
    [SerializeField] private int numberOfFlashes;
    [SerializeField] private float iFramesDuration;

    public GameObject bow;

    private Animator myAnimator;
    public bool dead;
    private SpriteRenderer mySpriteRender;
    public Collider2D[] myCollider;

    private AddArmor addArmor;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
        addArmor = GetComponent<AddArmor>();
    }

    private void Start()
    {
        health = maxHealth;
    }

    private void OnParticleCollision(GameObject other)
    {
        TakeDamage(1);
    }

    public void TakeDamage(float amount)
    {
        // If Evasive Maneuvers Buff is on then can dodge damage.
        if (buffContent.onEvasiveManeuvers)
        {
            int randomNumber = UnityEngine.Random.Range(0, 101);

            // If meet 25% chance then dodge damage.
            if (randomNumber <= buffContent.dodgeChance)
            {
                // Dodge Effect.
                GameObject evadeEffect = Instantiate(buffContent.evadeVFX, transform.position, transform.rotation, transform);
                Destroy(evadeEffect, 0.5f);
                return;
            }
        }

        // If Armored Fortitude Buff is on and armor is more than 0 then can block damage.
        if (buffContent.onArmoredFortitude && addArmor.armor > 0)
        {
            // Reduce armor value by 1.
            addArmor.armor--;

            // Block Effect.
            GameObject blockEffect = Instantiate(addArmor.blockVFX, transform.position, transform.rotation, transform);
            Destroy(blockEffect, 1f);
            return;
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
            
            for (int i = 0; i < myCollider.Length; i++)
            {
                if (myCollider[i] != null)
                {
                    myCollider[i].enabled = false;
                }
            }

            dead = true;
            bow.SetActive(false);
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
