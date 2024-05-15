using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyType
{
    Melee,
    Ranged,

    // Add more attack types as needed
}
public class EnemyAI : MonoBehaviour
{
 
        public float universalMoveSpeed = 3f; // Universal move speed
    private float tempMoveSpeed;
        public float detectionRange = 5f;
        public float attackRange = 1f;
    public AudioClip atkAudio;
 

      public float meleeAttackDuration = 1f;


      public float meleeAttackRate = 2f;
      public float rangedAttackRate = 2f;
  

      public GameObject hitbox; // Hitbox for melee and dash attacks
       public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public Animator animator;
    public float projectileSpeed;

        public EnemyType enemyType = EnemyType.Melee;

        private Transform player;
        private bool isMeleeAttacking = false;

        private float nextAttackTime = 0f;

        void Start ( )
        {
            player = GameObject . FindGameObjectWithTag ("Player") . transform;
            tempMoveSpeed = universalMoveSpeed;
        }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void Update()
    {
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
            if ( distanceToPlayer < detectionRange )
            {
          

                // If player is within attack range, attack
                if ( distanceToPlayer < attackRange )
                {
                    AttackPlayer ();
                }
                else
                {
                   ChasePlayer();
                   animator.SetBool("isWalking", true);
                }
            }
            else
            {
                // Player is not in detection range, remain idle
                Idle ();
                animator . SetBool ("isWalking" , false);
            }
    }
    void ChasePlayer ( )
    {
        // Calculate direction to move towards the player
        Vector2 direction = (player.position - transform.position).normalized;

        // Check if the player is within attack range
        if ( Vector2 . Distance (transform . position , player . position) >= attackRange )
        {
            // Move towards the player with universal move speed
            transform . position += ( Vector3 ) direction * universalMoveSpeed * Time . deltaTime;
        }
    }
    void AttackPlayer ( )
        {
       
            switch ( enemyType )
            {
                case EnemyType . Melee:
                    MeleeAttack ();
                    break;
                case EnemyType . Ranged:
                    RangedAttack ();
                    break;
     
            }
  
    }

        void Idle ( )
        {
            // Do nothing when player is not in detection range
        }

        void MeleeAttack ( )
        {
            if ( !isMeleeAttacking && Time . time >= nextAttackTime )
            {
            animator . SetTrigger ("Attack");
            SoundManager.Instance.PlayAudio(atkAudio);

            // Start the melee attack
            isMeleeAttacking = true;
                //hitbox . SetActive (true);

                // Stop the melee attack after a specified duration
                Invoke ("StopMeleeAttack" , meleeAttackDuration);

                // Set the cooldown for the next attack
                nextAttackTime = Time . time + 1f / meleeAttackRate;
            }
        }

        void StopMeleeAttack ( )
        {
            // Stop the melee attack
            isMeleeAttacking = false;
           // hitbox . SetActive (false);
        }

        void RangedAttack ( )
        {
        if ( Time . time >= nextAttackTime )
        {
            animator . SetTrigger ("Attack");
            SoundManager.Instance.PlayAudio(atkAudio);
            // Instantiate projectile at the spawn point
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

            // Calculate the direction to shoot the projectile
            Vector2 direction = (player.position - projectileSpawnPoint.position).normalized;

            // Apply force to the projectile
            projectile . GetComponent<Rigidbody2D> () . velocity = direction * projectileSpeed;

            // Set the cooldown for the next attack
            nextAttackTime = Time . time + 1f / rangedAttackRate;
        }
    }



    }

