using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[System.Serializable]
public class WeaponData
{
    public string weaponNameData;
    public WeaponScriptableObject weaponScript;
    public ItemScriptableObject itemWeapon;
}
[System.Serializable]
public class ShellData
{
    public string shellNameData;
    public ShellScriptableObject shellScript;
    public ItemScriptableObject itemShell;
}



public class PlayerPickup : MonoBehaviour
{


    private Inventory inventory;
    public ResourceInventory resourceInventory;
    public List<WeaponData> weapons;
    public List<ShellData> shells;
    public GameObject player;
    public AudioClip switchSelectedAudio;
    public AudioClip useAudio;
    public AudioClip dropAudio;
    public AudioClip pickupAudio;

   



    [SerializeField] private UI_Inventory uiInventory;



    private void HandleNumberKeyInput()
    {
        for (int i = 1; i <= 9; i++)
        {
            // Check for number key press (1-9)
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                SelectInventoryItemByIndex(i - 1); // Adjust for zero-based indexing
            }
        }
    }



    private void SelectInventoryItemByIndex(int index)
    {
        if (uiInventory != null && index >= 0 && index < uiInventory.GetInventorySize())
        {
            uiInventory.SetSelectedInventoryItem(index);
            SoundManager.Instance.PlayAudio(switchSelectedAudio);
        }
    }


    private void Start()
    {


        inventory = new Inventory(UseItem);
        inventory.itemList = GameManager.Instance.itemList;
        uiInventory.SetPlayer(this);
        uiInventory.SetInventory(inventory);
      



    }






    public float scrollSensitivity = 0.9f; // Adjust the sensitivity as needed


    void HandleScrollWheel()
    {
        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;
        // Assuming uiInventory is a reference to your inventory UI script
        int currentIndex = uiInventory.GetSelectedIndex();

        // Check if there is any scroll wheel input
        if (Mathf.Abs(scrollWheelInput) > 0)
        {
            // Move the index based on scroll direction
            if (scrollWheelInput > 0)
            {
                // Increment the index and wrap around to 0 if it exceeds the total number of items
                currentIndex = (currentIndex + 1) % uiInventory.GetInventorySize();
            }
            else
            {
                // Decrement the index and wrap around to the total number of items - 1 if it goes below 0
                currentIndex = (currentIndex - 1 + uiInventory.GetInventorySize()) % uiInventory.GetInventorySize();
            }

            // Set the new selected inventory item index
            uiInventory.SetSelectedInventoryItem(currentIndex);
            SoundManager.Instance.PlayAudio(switchSelectedAudio);

            // Debug the current index

        }
    }

    void Update()
    {

        HandleScrollWheel();
        HandleNumberKeyInput();


    
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }




    private void UseItem(Item item)
    {
        if(uiInventory.GetInventorySize() > 0)
        {


            switch (item.itemScriptableObject.itemType)
            {

  

                case Item.ItemType.Weapon:
                 
                    WeaponData foundWeaponData = weapons.Find(w => w.weaponNameData == item.itemScriptableObject.itemName);




                    if (foundWeaponData != null)
                    {
                        // Drop the previously equipped weapon data
                        if (GameManager.Instance.weaponSlot != null)
                        {
                            WeaponScriptableObject prevEquippedWeapon = GameManager.Instance.weaponSlot;
                            WeaponData prevEquippedWeaponData = weapons.Find(w => w.weaponScript == prevEquippedWeapon);

                            if (prevEquippedWeaponData != null)
                            {
                                // Drop or handle the previously equipped weapon and its corresponding item
                                ItemWorld.DropItem(GetPosition(), new Item { itemScriptableObject = prevEquippedWeaponData.itemWeapon });

                            }


                        }
                        GameManager.Instance.weaponSlot = foundWeaponData.weaponScript;
                        inventory.RemoveItem(uiInventory.GetSelectedItem());
                       
                    }
                   break;
                case Item.ItemType.Shell:

                    ShellData foundShellData = shells.Find(q => q.shellNameData == item.itemScriptableObject.itemName);




                    if (foundShellData != null)
                    {
                        // Drop the previously equipped weapon data
                        if (GameManager.Instance.shellSlot != null)
                        {
                            ShellScriptableObject prevEquippedShell = GameManager.Instance.shellSlot;
                            ShellData prevEquippedShellData = shells.Find(q => q.shellScript == prevEquippedShell);

                            if (prevEquippedShellData != null)
                            {
                                // Drop or handle the previously equipped weapon and its corresponding item
                                ItemWorld.DropItem(GetPosition(), new Item { itemScriptableObject = prevEquippedShellData.itemShell });

                            }


                        }
                        GameManager.Instance.shellSlot = foundShellData.shellScript;
                        inventory.RemoveItem(uiInventory.GetSelectedItem());
                        GameManager.Instance.armor = GameManager.Instance.shellSlot.shellArmor;
                        GameManager.Instance.shellHp = GameManager.Instance.shellSlot.shellHealth;
                        GameManager.Instance.shellStam = GameManager.Instance.shellSlot.shellStamina;
                        GameManager.Instance.shellHunger = GameManager.Instance.shellSlot.shellHunger;
                        GameManager.Instance.shellSummon = GameManager.Instance.shellSlot.shellSummon;
                        GameManager.Instance.SetStats();
                    }
                    break;




                case Item.ItemType.Summon:
                    if (GameManager.Instance.currentSummons < GameManager.Instance.maxSummons)
                    {
                        int destroyInt = Random.Range(1, 3);
                        if (destroyInt <= 1)
                        {
                            inventory.RemoveItem(new Item { itemScriptableObject = item.itemScriptableObject });
                            GameManager.Instance.itemList.Remove(item);
                        }
                        Vector3 randomDir = Random.onUnitSphere;

                    
                        Instantiate(item.itemScriptableObject.summonCreature, gameObject.transform.position + randomDir * 2.5f, Quaternion.identity);
                    
                        
                      
                    }
                    break;

                case Item.ItemType.Heal:
                    player.GetComponent<PlayerHealth>().currentHealth += item.itemScriptableObject.effectAmount;
                   
                    inventory.RemoveItem(new Item { itemScriptableObject = item.itemScriptableObject });
                    GameManager.Instance.itemList.Remove(item);
                   
                    break;
                case Item.ItemType.Food:
                    inventory.RemoveItem(new Item { itemScriptableObject = item.itemScriptableObject });
                    GameManager.Instance.itemList.Remove(item);
                    player.GetComponent<PlayerHealth>().hunger += item.itemScriptableObject.effectAmount;
                    break;
                case Item.ItemType.StaminaRecover:
                    inventory.RemoveItem(new Item { itemScriptableObject = item.itemScriptableObject });
                    GameManager.Instance.itemList.Remove(item);
                    GameManager.Instance.stamina += item.itemScriptableObject.effectAmount;
                    break;
                case Item.ItemType.FullRestore:
                    inventory.RemoveItem(new Item { itemScriptableObject = item.itemScriptableObject });
                    GameManager.Instance.itemList.Remove(item);
                    player.GetComponent<PlayerHealth>().hunger += item.itemScriptableObject.effectAmount;
                    GameManager.Instance.stamina += item.itemScriptableObject.effectAmount;
                    player.GetComponent<PlayerHealth>().currentHealth += item.itemScriptableObject.effectAmount;
                    break;
                case Item.ItemType.Orb:
                    GameManager.Instance.orbRestored = true;
                    inventory.RemoveItem(new Item { itemScriptableObject = item.itemScriptableObject });
                    break;


            }

            uiInventory.SetSelectedInventoryItem(uiInventory.GetInventorySize());
        }
     


    }

   


    public void UseFunc(InputAction.CallbackContext context)
    {
    
       
            if (context.performed && uiInventory.GetInventorySize() > 0)
            {
                UseItem(uiInventory.GetSelectedItem());
                SoundManager.Instance.PlayAudio(useAudio);
            }
        
     
    }

    public void DropFunc(InputAction.CallbackContext context)
    {
        if(context.performed && uiInventory.GetInventorySize() > 0 && uiInventory.GetSelectedItem() != null)
        {
   
            ItemWorld.DropItem(GetPosition(), uiInventory.GetSelectedItem());
            inventory.RemoveItem(uiInventory.GetSelectedItem());
            GameManager.Instance.itemList.Remove(uiInventory.GetSelectedItem());
            uiInventory.SetSelectedInventoryItem(uiInventory.GetInventorySize() -1);
            SoundManager.Instance.PlayAudio(dropAudio);

        }
    
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        ItemWorld itemWorld = collider.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            Item item = itemWorld.GetItem();
            if (item.itemScriptableObject.itemType == Item.ItemType.Resource)
            {
                // Add the Resource item to the resourceInventory
                GameManager.Instance.resourceList.Add(item);
                itemWorld.DestroySelf();
                resourceInventory.RefreshInventoryItems();
                SoundManager.Instance.PlayAudio(pickupAudio);
            }
            else 
            {
                if (inventory.itemList.Count < 5)
                {
                    // Add the item to the main inventory
                    GameManager.Instance.itemList.Add(itemWorld.GetItem());
                    itemWorld.DestroySelf();
                    uiInventory.SetSelectedInventoryItem(uiInventory.GetInventorySize());
                    SoundManager.Instance.PlayAudio(pickupAudio);
                }
            }
          
            //inventory.AddItem(itemWorld.GetItem());


        }
    }


}
