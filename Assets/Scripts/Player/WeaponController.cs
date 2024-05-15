using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
   private EnemyHealth enemyHealth;
   

    private SpriteRenderer spriteRenderer;

    public WeaponScriptableObject Data;


    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Data = GameManager . Instance . weaponSlot;
        spriteRenderer.sprite = Data.weaponSprite;
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>().enabled = false;
 
    }

    void Update ()
    {

      
        if (Data != GameManager.Instance.weaponSlot)
        {
            Data = GameManager.Instance.weaponSlot;
            spriteRenderer . sprite = Data . weaponSprite;
           Destroy(GetComponent<PolygonCollider2D>());
            gameObject.AddComponent<PolygonCollider2D>().enabled = false;
        
        }
       
        
    }

    public void EnableDamage()
    {
        gameObject.GetComponent<PolygonCollider2D>().enabled = true;
        gameObject.GetComponent <PolygonCollider2D>().isTrigger = true;
    }

    public void DisableDamage()
    {
        gameObject . GetComponent<PolygonCollider2D> () . enabled = false;
        gameObject . GetComponent<PolygonCollider2D> () . isTrigger = false;

    }


    private void OnTriggerEnter2D(Collider2D col) {
        if(col.CompareTag("Enemy")) 
        {
           enemyHealth = col.GetComponent<EnemyHealth>();

           enemyHealth.TakeDamage(Data.attackDamage + GameManager.Instance.resourceATK);
        
        }
        if(col.CompareTag("Object"))
        {

            Destroy (col . gameObject);
        }
    }
}
