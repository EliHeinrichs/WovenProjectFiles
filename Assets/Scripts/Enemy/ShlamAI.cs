using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShlamAI : MonoBehaviour
{
    public float universalMoveSpeed = 3f; // Universal move speed
    private float tempMoveSpeed;
    public float detectionRange = 5f;
    public float attackRange = 1f;
    public float meleeAttackDuration = 1f;


   public float meleeAttackRate = 2f;


    public bool rolling;
    public List<Transform> rollPoints;
    public float rollTimer;
    private int rollIndex =1;
    private Transform activeSpot;
    public float detectRangeRoll;

    public Animator animator;



    private Transform player;
    private bool isMeleeAttacking = false;

    private float nextAttackTime = 0f;

    public AudioClip atkAudio;
    public AudioClip rollAudio;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        tempMoveSpeed = universalMoveSpeed;
    }



    void Update()
    {

        rollTimer -= Time.deltaTime;

        if(rollTimer <= 0)
        {
            animator.SetBool("Roll",true);
            SoundManager.Instance.PlayAudio(rollAudio);
            rolling = true;
            rollTimer = 40f;

        }
        
        if(rollIndex >= 8)
        {
            animator.SetBool("Roll", false);
            rolling = false;
            rollIndex = 1;
        }
        // Calculate distance between AI and player

        float distanceToPlayer = 0;
        // Flip the enemy based on player's position
        if (player.position.x > transform.position.x)
        {
            distanceToPlayer = Vector2.Distance(transform.position, new Vector2(player.position.x - 1.5f, player.position.y - 1.5f));
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            distanceToPlayer = Vector2.Distance(transform.position, new Vector2(player.position.x + 1.5f, player.position.y + 1.5f));
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }


        // If player is within detection range, chase player
        if (distanceToPlayer < detectionRange)
        {

            if (rolling == false)
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
                Roll();

            }
        }
        else
        {
            // Player is not in detection range, remain idle
            Idle();
     
        }
    }
    void ChasePlayer()
    {
        // Calculate direction to move towards the player
        Vector2 direction = (player.position - transform.position).normalized;

        // Check if the player is within attack range
        if (Vector2.Distance(transform.position, player.position) >= attackRange)
        {
            // Move towards the player with universal move speed
            transform.position += (Vector3)direction * universalMoveSpeed * Time.deltaTime;
        }
    }

    void Roll()
    {
        activeSpot = rollPoints[rollIndex];
        // Calculate direction to move towards the player
        Vector2 direction = (activeSpot.position - transform.position).normalized;

        // Check if the player is within attack range
        if (Vector2.Distance(transform.position, activeSpot.position) >= detectRangeRoll)
        {
            // Move towards the player with universal move speed
            transform.position += (Vector3)direction * universalMoveSpeed * 6f * Time.deltaTime;
        }
        else
        {
            rollIndex += 1;
        }
    
 
    }
    void AttackPlayer()
    {

       
                MeleeAttack();
              
           

        
    }

    void Idle()
    {
        // Do nothing when player is not in detection range
    }

    void MeleeAttack()
    {
        if (!isMeleeAttacking && Time.time >= nextAttackTime)
        {
            animator.SetTrigger("Attack");
            SoundManager.Instance.PlayAudio(atkAudio);

            // Start the melee attack
            isMeleeAttacking = true;
            //hitbox . SetActive (true);

            // Stop the melee attack after a specified duration
            Invoke("StopMeleeAttack", meleeAttackDuration);

            // Set the cooldown for the next attack
            nextAttackTime = Time.time + 1f / meleeAttackRate;
        }
    }

    void StopMeleeAttack()
    {
        // Stop the melee attack
        isMeleeAttacking = false;
        // hitbox . SetActive (false);
    }



}
