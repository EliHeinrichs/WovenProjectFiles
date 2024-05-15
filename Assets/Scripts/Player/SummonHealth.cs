using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonHealth : MonoBehaviour
{
    public int health;
    public AudioClip hitAudio;
    public AudioClip deathAudio;

    public void TakeDamage(int damage)
    {
        Debug.Log("Damage");
        health -= damage;
        SoundManager.Instance.PlayAudio(hitAudio);
        if (health <= 0 )
        {
            Destroy(gameObject);
            SoundManager.Instance.PlayAudio(deathAudio);
        }
    }
}
