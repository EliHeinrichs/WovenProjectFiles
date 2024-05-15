using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyezoreAI : MonoBehaviour
{
    public float universalMoveSpeed = 3f; // Universal move speed

    public float detectionRange = 5f;
    public float attackRange = 1f;

    public float rangedAttackRate = 2f;


    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public Animator animator;
    public float projectileSpeed;
    public Transform[] tpSpots;
    public float timerTP;
    public bool tping = false;

    private Transform player;
    public Transform thisGuy;

    private float nextAttackTime = 0f;

    public AudioClip shootAudio;
    public AudioClip tpAudio;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
    }


    void Update()
    {
        timerTP -= Time.deltaTime;
        if (timerTP <= 0 )
        {
            animator.SetTrigger("TP");
            SoundManager.Instance.PlayAudio(tpAudio);
            timerTP = 30f;
        }
        float distanceToPlayer = 0;
   
        // Flip the enemy based on player's position
        if (player.position.x > thisGuy.transform.position.x)
        {
            distanceToPlayer = Vector2.Distance(thisGuy.transform.position, new Vector2(player.position.x - 1.5f, player.position.y - 1.5f));
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            distanceToPlayer = Vector2.Distance(thisGuy.transform.position, new Vector2(player.position.x + 1.5f, player.position.y + 1.5f));
           transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }


        // If player is within detection range, chase player
        if (distanceToPlayer < detectionRange)
        {
        

            if (tping == false)
            {
                // If player is within attack range, attack
                if (distanceToPlayer < attackRange)
                {
                    AttackPlayer();
                }
                else
                {
                    ChasePlayer();
                 
                }
            }
            else
            {
                Idle();
            }
        }
        else
        {
            // Player is not in detection range, remain idle
            Idle();
       
        }
    }

    public void Teleport()
    {
        thisGuy.transform.position = tpSpots[Random.Range(1, 4)].position;
    }
    void ChasePlayer()
    {
        // Calculate direction to move towards the player
        Vector2 direction = (player.position - thisGuy.transform.position).normalized;

        // Check if the player is within attack range
        if (Vector2.Distance(transform.position, player.position) >= attackRange)
        {
            // Move towards the player with universal move speed
            thisGuy.transform.position += (Vector3)direction * universalMoveSpeed * Time.deltaTime;
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
            SoundManager.Instance.PlayAudio(shootAudio);
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
