using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonAI : MonoBehaviour
{
    public float universalMoveSpeed = 3f; // Universal move speed

    public float detectionRange = 5f;
    public float attackRange = 1f;


    public float rangedAttackRate = 2f;
    public ParticleSystem damageParticle;


     public GameObject projectilePrefab;
     public Transform projectileSpawnPoint;
    public Animator animator;
    public float projectileSpeed;

    public Transform spider;


    private float nextAttackTime = 0f;

    private Transform player;
    bool spiderFollow;

    public AudioClip atkAudio;


    void Start()
    {
        spider = GameObject.FindGameObjectWithTag("Player").transform;
        GameManager.Instance.currentSummons += 1;
        universalMoveSpeed = Random.Range(1f, 4f);
        rangedAttackRate = Random.Range(1f, 3f);
    }
    public void PlayParticle()
    {
        damageParticle.Play();
    }

    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= detectionRange && distance < nearestDistance)
            {
                spiderFollow = false;
                nearestEnemy = enemy;
                nearestDistance = distance;
                player = nearestEnemy.transform;
            }
          

     
      
        }

     

        if (nearestEnemy == null)
        {
            player = spider;
            
            spiderFollow = true;

        }
      

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
        if (distanceToPlayer < detectionRange && spiderFollow != true)
        {

            
            // If player is within attack range, attack
            if (distanceToPlayer < attackRange)
            {
      
                AttackPlayer();
            }
            else
            {

                ChaseEnemy();
          
            }
        }
        else
        {
            // Player is not in detection range, remain idle
        
            ChaseEnemy();
 
        }
    }
    void ChaseEnemy()
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


    void AttackPlayer()
    {

      
       RangedAttack();
          SoundManager.Instance.PlayAudio(atkAudio);

        
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
