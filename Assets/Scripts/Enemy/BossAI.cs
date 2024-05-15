using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BossAI : MonoBehaviour
{
 



    public float rangedAttackRate = 2f;


    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public Animator animator;
    public float projectileSpeed;


    public GameObject summon;
    public int summonAmt;
    public float timerSummon;
    public bool summoning;
    

    private Transform player;


    private float nextAttackTime = 0f;

    public bool startEnded = false;
    private bool started = false;

    private float distanceToPlayer = 0f;


    public AudioClip shootSound;
    public AudioClip summonSFX;

    public Sprite dialogueSprite;

    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameObject.GetComponent<BossRotate>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
     

    }

    public IEnumerator StartSequence()
    {
        started = true;

        DialogueManager.Instance.StartDialogue("Ah we meet at last!", dialogueSprite);

        yield return new WaitForSeconds(2.8f);
        DialogueManager.Instance.StartDialogue("With my Life Orb I will rule with my mushroom overgrowth forever!", dialogueSprite);
     
        yield return new WaitForSeconds(6.5f);
        DialogueManager.Instance.StartDialogue("You cannot defeat me, for the Orb grants the power to create and destroy!", dialogueSprite);

        yield return new WaitForSeconds(6.5f);
        DialogueManager.Instance.StartDialogue("NOW DIE!!!", dialogueSprite);
      
        yield return new WaitForSeconds(2.3f);
        DialogueManager.Instance.EndDialogue();
        gameObject.GetComponent<BossRotate>().enabled = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        startEnded = true;
    
    }


    void Update()
    {


       

        if(startEnded == true)
        {
            timerSummon -= Time.deltaTime;
            if (timerSummon <= 0)
            {

                animator.SetTrigger("Summon");
                Summon();
                timerSummon = 30f;
            }


            AttackPlayer();
        }
        else
        {
            distanceToPlayer = Vector2.Distance(transform.position, new Vector2(player.position.x, player.position.y));

            if (distanceToPlayer < 3 && started == false)
            {
                StartCoroutine(StartSequence());
            }
        }
      


    }

    public void Summon()
    {

        for(int i = 0; i <= summonAmt; i++)
        {
            Vector3 randomDir = Random.onUnitSphere;
            Instantiate(summon, transform.position + randomDir * 10f, Quaternion.identity);
            SoundManager.Instance.PlayAudio(summonSFX);
        }

    }


    void AttackPlayer()
    {


        RangedAttack();



    }

    void Idle()
    {
        // Do nothing when player is not in detection range
    }

    void RangedAttack()
    {
        if (Time.time >= nextAttackTime)
        {
            animator.SetTrigger("Attack");
            SoundManager.Instance.PlayAudio(shootSound);
            // Instantiate projectile at the spawn point
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

            // Calculate the direction to shoot the projectile
            Vector2 direction = (player.position - projectileSpawnPoint.position).normalized;

            // Apply force to the projectile
            projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;

            // Set the cooldown for the next attack
            nextAttackTime = Time.time + 1f / rangedAttackRate;
        }
     
    }

}
