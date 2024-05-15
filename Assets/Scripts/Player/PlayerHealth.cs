using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth = 5;
    public Animator animator;

    private float damageCooldownDefault = 0.5f;
    private float damageCooldown = 0f;
    private float lerpSpeed = 0.02f;

    public Slider healthSlider;
    public Slider easeHealthSlider;
 
    public Slider staminaSlider;
    public Slider hungerSlider;


    public float staminaRecoveryRate;

    public float hunger;
    public float hungerDepreciationRate;

    public AudioClip hitAudio;
    public AudioClip rezAudio;
    public AudioClip deathAudio;

    public GameObject deathScreen;
    public AudioClip mainSoundTrack;


    private void Start()
    {

        currentHealth = GameManager.Instance.currentHp;

        hunger = GameManager.Instance.hunger;
   

        if(currentHealth > GameManager.Instance.maxHp)
        {
            currentHealth = GameManager . Instance . maxHp;
        }

        if (GameManager.Instance.currentHp < currentHealth)
        {
            currentHealth = GameManager.Instance.currentHp;
        }

        GameManager.Instance.playerIsDead = false;


    }

    float hungryTimer = 0;

    void Update()
    {
        UpdateHearts();
        UpdateStamina();
        UpdateHunger();

        staminaSlider.maxValue = GameManager.Instance.maxStamina;
        hungerSlider.maxValue = GameManager.Instance.maxHunger;
     
        healthSlider.maxValue = GameManager.Instance.maxHp;

        easeHealthSlider .maxValue = healthSlider.maxValue;
     


        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, currentHealth, lerpSpeed);
        }

        if (damageCooldown > 0)
        {
            StartDamageCooldown();
        }

        if(hunger <= 0)
        {

            hungryTimer -= Time.deltaTime;

            if (hungryTimer <= 0)
            {
                TakeDamage(1);
                hungryTimer = 5f;
            }
        }
    }


    void LateUpdate()
    {

        if (currentHealth <= 0 && GameManager.Instance.rez <= 0)
        {
            GameManager.Instance.playerIsDead = true;
            SoundManager.Instance.PlayAudio(deathAudio);
            StartCoroutine(deathSequence()); 
            


        }
        else if(currentHealth <= 0 && GameManager.Instance.rez > 0)
        {
          
            GameManager.Instance.rez -= 1;
            currentHealth = 1 + GameManager.Instance.rez;
            GameManager.Instance.rez =0;
            GameManager.Instance.resourceList.Clear();
            SoundManager.Instance.PlayAudio(rezAudio);

        }

 
    }

    private IEnumerator deathSequence()
    {
        deathScreen.SetActive(true);
        
        yield return new WaitForSeconds(3f);
        GameManager.Instance.currentHp = 1;
        GameManager.Instance.playerIsDead = false;

        GameManager.Instance.ResetGame();
        SoundManager.Instance.ChangeMusic(mainSoundTrack);
        SceneManager.LoadScene("Town");

    }

    public void TakeDamage(int damage)
    {
        if (damageCooldown > 0)
        {
            return;
        }

        damageCooldown = damageCooldownDefault;

        damage -= GameManager.Instance.armor;
        if ( damage < 1 )
        {
            damage = 1;
        }
        currentHealth -= damage;
        animator.SetTrigger("Hit");
        SoundManager.Instance.PlayAudio(hitAudio);
        //Save the current health in game manager
        GameManager.Instance.currentHp = currentHealth;
    }





   private void UpdateHearts()
   {
        GameManager.Instance.currentHp = currentHealth;
        if(currentHealth > GameManager.Instance.maxHp)
        {
            currentHealth = GameManager.Instance.maxHp;
        }
     

        if (healthSlider.value != currentHealth)
        {
            healthSlider.value = currentHealth;
        }

   }

    private void UpdateStamina()
    {
        staminaSlider.value = GameManager.Instance.stamina;

        if (staminaSlider.value != GameManager.Instance.stamina)
        {
            staminaSlider.value = Mathf.Lerp(staminaSlider.value, GameManager.Instance.stamina, lerpSpeed);
        }
        if (GameManager.Instance.stamina > GameManager.Instance.maxStamina)
        {
            GameManager.Instance.stamina = GameManager.Instance.maxStamina;
        }


        if (staminaSlider.value <= GameManager.Instance.maxStamina)
        {
            GameManager.Instance.stamina += staminaRecoveryRate * Time.deltaTime;
            GameManager.Instance.stamina = Mathf.Clamp(GameManager.Instance.stamina, 0f, GameManager.Instance.maxStamina);
        }

    }

    void UpdateHunger()
    {
        if (hunger > GameManager.Instance.maxHunger)
        {
            hunger = GameManager.Instance.maxHunger;
        }

        hunger -= Time.deltaTime * hungerDepreciationRate;
        GameManager.Instance.hunger = hunger;

        hungerSlider.value = hunger;

    }




    private void StartDamageCooldown()
    {
        if (damageCooldown <= 0)
        {
            return;
        }

        damageCooldown -= Time.deltaTime;
    }
}
