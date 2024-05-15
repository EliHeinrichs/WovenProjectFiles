using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine . SceneManagement;
public class GameManager : MonoBehaviour
{

    public int level;

    public bool playerIsDead = false;


    public List<Item> itemList;
    public List<Item> resourceList;
    public List<Item> storageList;
    public ShellScriptableObject shellSlot;
    public WeaponScriptableObject weaponSlot;
     public bool storageOpen;

    public int currentHp;
    public int maxHp;


    public float stamina;
    public float maxStamina;

    public float hunger;
    public float maxHunger;

    public int resourceATK;
    public int resourceHP;
    public float resourceStamina;
    public int resourceSummon;
    public int resourceHunger;

    public float moveSpeed;
    public int rez;

    public int light;

    public int maxSummons;
    public int currentSummons;

    public int armor;
    public int shellHp;
    public float shellStam;
    public int shellHunger;
    public int shellSummon;

    public int gameStage = 1;

    public bool orbRestored;

    public ShellScriptableObject baseShell;
    public WeaponScriptableObject baseWeapon;


    public static GameManager Instance { get; private set; }
 
    // Other variables and functions can be added as needed

    private void Awake ( )
    {
      
        // Set up the singleton pattern
        if ( Instance == null)
        {
         
            
                Instance = this;
                DontDestroyOnLoad(gameObject);
            
              
        }
        else
        {
            Destroy (gameObject);
        }
        armor = shellSlot.shellArmor;
        shellStam = shellSlot.shellStamina;
        shellHunger = shellSlot.shellHunger;
        shellHp = shellSlot.shellHealth;
        shellSummon = shellSlot.shellSummon;

        SetStats ();
        currentHp = maxHp;
        hunger = maxHunger;
        stamina = maxStamina;
      
    
    }

    public void SetStats()
    {
        maxHp = resourceHP + shellHp + 5;
        maxHunger = resourceHunger + shellHunger + 200;
        maxSummons = resourceSummon + shellSummon + 1;
        maxStamina = resourceStamina + shellStam + 10;
    }

    public void ResetGame()
    {

      
        itemList.Clear();
        storageList.Clear();
        resourceList.Clear();
        shellSlot = baseShell;
        weaponSlot = baseWeapon;
        gameStage = 1;
        level = 0;
        rez = 0;
        armor = shellSlot.shellArmor;
        shellStam = shellSlot.shellStamina;
        shellHunger = shellSlot.shellHunger;
        shellHp = shellSlot.shellHealth;
        shellSummon = shellSlot.shellSummon;
   
        SetStats();
        currentHp = maxHp;
        hunger = maxHunger;
        stamina = maxStamina;
       

    }

}
