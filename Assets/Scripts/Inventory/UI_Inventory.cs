using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CodeMonkey.Utils;
using System . Reflection;

public class UI_Inventory : MonoBehaviour
{
     private Inventory inventory;
     private Transform itemSlotContainer;
     private Transform itemSlotTemplate;
     private PlayerPickup player;

    public int selectedIndex = -1;
    private float selectedScaleFactor = 1.2f;
    private Item selectedItem = null;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    private List<Item> lastInventoryList;

    public AudioClip inventorySFX;

    private void Start()
   {
      itemSlotContainer = transform.Find("ItemSlotContainer");
      itemSlotTemplate = itemSlotContainer.Find("SlotTemplate");
        lastInventoryList = new List<Item>(GameManager.Instance.itemList);
        RefreshInventoryItems();
    }

    void Update()
    {
        if(GetSelectedItem() != null)
        {
            nameText.text = GetSelectedItem().itemScriptableObject.itemName;
            descriptionText.text = GetSelectedItem().itemScriptableObject.itemDescription;
        }

        if(selectedIndex < 0)
        {
            nameText.text = "";
            descriptionText.text = "";
        }

        if (!ListsAreEqual(lastInventoryList, GameManager.Instance.itemList))
        {
            // Update lastStorageList
            lastInventoryList = new List<Item>(GameManager.Instance.itemList);

            // Refresh the storage UI
            RefreshInventoryItems();
        }

    }

    private bool ListsAreEqual(List<Item> list1, List<Item> list2)
    {
        // Compare lists element by element
        if (list1.Count != list2.Count)
        {
            return false;
        }

        for (int i = 0; i < list1.Count; i++)
        {
            if (list1[i] != list2[i])
            {
                return false;
            }
        }

        return true;
    }

    public void SetPlayer(PlayerPickup player)
   {
      this.player = player;
   }

     public void SetInventory(Inventory inventory) {


        this.inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
     }


   private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
   {
   
      RefreshInventoryItems();
   }

    public int GetInventorySize ( )
    {
        return inventory . GetItemList () . Count;
        
    }
    public void SetSelectedInventoryItem ( int index )
    {
        selectedIndex = Mathf . Clamp (index , 0 , GetInventorySize () - 1);
        selectedItem = inventory . GetItemList () [selectedIndex];
        RefreshInventoryItems (); // Refresh UI to highlight the selected item
 
    }
 
    private void RefreshInventoryItems()
    {
  
      foreach (Transform child in itemSlotContainer){
         if(child == itemSlotTemplate) continue;
         Destroy(child.gameObject);
       

      }

      int x = 0;
      int y = 0;
      float itemSlotCellSize = 140f;
        int index = 0;
      foreach(Item item in inventory.GetItemList()){

       
         RectTransform itemSlotRectTransform = Instantiate (itemSlotTemplate,itemSlotContainer).GetComponent<RectTransform>();
         itemSlotRectTransform.gameObject.SetActive(true);
      
        itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () => {
            if (GameManager.Instance.storageOpen == true && GameManager.Instance.storageList.Count < 8)
            {

                GameManager.Instance.storageList.Add(item);
                inventory.RemoveItem(item);
                SoundManager.Instance.PlayAudio(inventorySFX);

            }
            else
            {
                inventory.UseItem(item);
            }
         };
         itemSlotRectTransform.GetComponent<Button_UI>().MouseRightClickFunc = () => {
             // Drop Item
             

             if (GameManager.Instance.storageOpen == true && GameManager.Instance.storageList.Count < 8)
             {
          
                 GameManager.Instance.storageList.Add(item);
                 inventory.RemoveItem(item);
                 SoundManager.Instance.PlayAudio(inventorySFX);

             }
             else
             {
                 Item duplicateItem = new Item { itemScriptableObject = item.itemScriptableObject };


                 inventory.RemoveItem(item);

                 ItemWorld.DropItem(player.GetPosition(), duplicateItem);
             }

       
         }; 
 

       

         itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
         Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
         TextMeshProUGUI keyNum = itemSlotRectTransform.Find("KeyText").GetComponent<TextMeshProUGUI>();
            keyNum.text = (index +1).ToString();
         image.sprite = item.itemScriptableObject.itemSprite;


            Vector3 scale = (index == selectedIndex) ? new Vector3(selectedScaleFactor, selectedScaleFactor, 1f) : Vector3.one;
            itemSlotRectTransform . localScale = scale;
   

         x++;
         index++;
         if(x>9){

            x=0;
            y++;
         }

           GetInventorySize ();
      }

    }

    public int GetSelectedIndex()
    {
        return selectedIndex;
    }
    public Item GetSelectedItem()
    {
        return selectedItem;
    }

}
