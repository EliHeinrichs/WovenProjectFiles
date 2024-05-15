using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungryOneAI : MonoBehaviour
{
    public float rangedAttackRate = 2f;


    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public Animator animator;
    public float projectileSpeed;


    public GameObject pitSpawn;
    public float pitTimer;
   
    public float timerSwipe;



    private Transform player;


    private float nextAttackTime = 0f;

    public AudioClip swipeAudio;
    public AudioClip shootAudio;
    public AudioClip summonAudio;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }


    void Update()
    {
        timerSwipe-= Time.deltaTime;
        pitTimer -= Time.deltaTime;
        if (timerSwipe <= 0)
        {

            animator.SetTrigger("Swipe");
            SoundManager.Instance.PlayAudio(swipeAudio);
            timerSwipe = 10f;
        }

        if(pitTimer <= 0)
        {
            SoundManager.Instance.PlayAudio(summonAudio);
            Vector3 randomDir = Random.onUnitSphere;
            Instantiate(pitSpawn, transform.position + randomDir * Random.Range(5f,18f), Quaternion.identity);
            pitTimer = 1.4f;
        }

        AttackPlayer();


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
