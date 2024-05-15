using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public int Damage;


    void OnTriggerEnter2D ( Collider2D other )
    {
        
        if ( other . CompareTag ("Enemy") )
        {
  
            other . gameObject . GetComponent<EnemyHealth> () . TakeDamage (Damage + GameManager.Instance.resourceATK + GameManager.Instance.weaponSlot.attackDamage);
        }
        if ( other . CompareTag ("Object") )
        {
            Destroy (other . gameObject);
        }
    }
}
