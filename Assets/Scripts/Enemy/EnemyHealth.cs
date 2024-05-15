using System.Collections;
using System.Collections.Generic;
using Unity . VisualScripting . InputSystem;
using UnityEngine;
using TMPro;


[System.Serializable]
public class ItemDrop
{
    public int dropRate;
    public ItemScriptableObject item;


}
[CreateAssetMenu]
public class EnemyHealth : MonoBehaviour
{

    public bool isBoss;
    public bool isFinalBoss;
    public bool isFinalFinalBoss;
    public int health;
    public GameObject exitSpawn;
    public GameObject damageParticle;

    public AudioClip hitAudio;
    public AudioClip deathAudio;

    public ItemDrop[] items;
    private float immuneTime;
    [SerializeField] private GameObject floatingText;
    public Sprite dialogueSprite;

    public GameObject finalSplat;


    private void Update ( )
    {
        immuneTime -= Time.time;
    }

    void Start()
    {
        int bonusHp = GameManager.Instance.level / 5;
        health += bonusHp;

    }


    public void TakeDamage(int damage)
    {
        if (immuneTime <= 0)
        {
            health -= damage;
            immuneTime = 0.7f;
            ShowDamage (damage . ToString ());
            SoundManager.Instance.PlayAudio(hitAudio);
        }
        //hit anim
       
        Instantiate(damageParticle, transform.position, Quaternion.identity);
        if (health <= 0)
        {
            immuneTime = 100f;
            
            GetLoot();
            Die ();
        }
    }


    void Die()
    {
        if(isFinalBoss)
        {
            if(isFinalFinalBoss)
            {
                Instantiate(finalSplat, transform.position, Quaternion.identity);
                GetLoot();
               
                gameObject.GetComponent<Animator>().SetTrigger("Dead");
                GameManager.Instance.gameStage = 3;
                Instantiate(exitSpawn, transform.position, Quaternion.identity);
                SoundManager.Instance.PlayAudio(deathAudio);

            }
            else
            {
                Instantiate(finalSplat, transform.position, Quaternion.identity);
                SoundManager.Instance.PlayAudio(deathAudio);
                gameObject.GetComponent<BossAI>().enabled = false;
                gameObject.GetComponent<BossRotate>().enabled = false;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                StartCoroutine(bossDeath());
            }
         
        }
        else
        {

            Destroy(gameObject);
            SoundManager.Instance.PlayAudio(deathAudio);
        }
  
    }

    public IEnumerator bossDeath()
    {
    

        DialogueManager.Instance.StartDialogue("My plan is foiled, a pity. But the Orb is still of use!", dialogueSprite);
        yield return new WaitForSeconds(7.5f);
        DialogueManager.Instance.StartDialogue("Alas the giant one hungers, and he shall now feed....", dialogueSprite);
       
        yield return new WaitForSeconds(7.5f);
        DialogueManager.Instance.StartDialogue("RAHHHHHHHHHHHHH....", dialogueSprite);
      
        yield return new WaitForSeconds(2.2f);
        DialogueManager.Instance.EndDialogue();
        Destroy(gameObject);
        SoundManager.Instance.PlayAudio(deathAudio);
    }


    public void GetLoot()
    {
  

        foreach (ItemDrop item in items)
        {
            float drawn = Random.Range(0f, 100f);
            if (drawn <= item.dropRate)
            {
                ItemWorld.DropItem(gameObject.transform.position, new Item { itemScriptableObject = item.item });
            }
        }
    }

    void ShowDamage ( string text )
    {


        GameObject prefab = Instantiate(floatingText,transform.position,Quaternion.identity);
        prefab . transform . position = new Vector3 (prefab . transform . position . x + Random . Range (-1f , 1f) , prefab . transform . position . y + Random . Range (0.5f , 1f) , 0);
        prefab . GetComponentInChildren<TextMeshProUGUI> () . text = text;
        return;


    }

    void OnDestroy()
    {
        if(isBoss)
        {
            Instantiate(exitSpawn, transform.position, Quaternion.identity);
            GameManager.Instance.gameStage = 2;
        }
     
    }

 
}
