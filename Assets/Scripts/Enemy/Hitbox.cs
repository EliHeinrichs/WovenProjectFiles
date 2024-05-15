using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{

    public int Damage;
    public int bonusDmg = GameManager.Instance.level / 6;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag ("Player"))
        {
            
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage(bonusDmg + Damage);
        }
        if(other.CompareTag("Object"))
        {
            Destroy(other.gameObject);
        }
        if(other.gameObject.CompareTag("Summon"))
        {
            other.gameObject.GetComponent<SummonHealth>().TakeDamage(Damage);
        }
     
    }
}
