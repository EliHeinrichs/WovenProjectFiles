using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ResourceInventory : MonoBehaviour
{
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    public UnityEngine.Rendering.Universal.Light2D light2D;
    private int lightRange;
    private int damage;
    private float moveSpeed;
    private float stamina;
    private int maxHealth;
    private int maxFood;
    private int maxSummon;
    private int rez;

    void Start()
    {
        itemSlotContainer = transform.Find("ItemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("SlotTemplate");

        RefreshInventoryItems();
    }


    void HandleResource()
    {
        damage = 0;
        lightRange = 0;
        rez = 0;
        moveSpeed = 0;
        stamina = 0;
        maxSummon = 0;
        maxFood = 0;


        light2D.pointLightOuterRadius = 5;
        light2D.pointLightInnerRadius = 2;
        foreach (Item item in GameManager.Instance.resourceList)
        {
            switch (item.itemScriptableObject.itemName)
            {
                case "Stinger":
                    damage += 1;
                    break;

                case "LightbugTail":
                 
                   lightRange += 1;
                   
                    break;

                case "ButterflyHeart":
                    rez += 1;
                
                    break;

                case "Wings":
                    moveSpeed += 1;
                    break;
                case "Battery":
                    stamina += 1;
                    break;

                case "Flute":
                    maxSummon += 1;            
                    break;

                case "SpiderEye":
                    maxFood += 10;

                    break;
                case "Shedding":
                    maxHealth += 1;
                    break;


            }


        }

        GameManager.Instance.light = lightRange;
        GameManager.Instance.moveSpeed = moveSpeed;
        GameManager.Instance.resourceHP = maxHealth;
        GameManager.Instance.resourceStamina = stamina;
        GameManager.Instance.resourceSummon = maxSummon;
        GameManager.Instance.resourceHunger = maxFood;
        GameManager.Instance.rez = rez;

        light2D.pointLightOuterRadius += lightRange;
        GameManager.Instance.resourceATK = damage;

        GameManager.Instance.SetStats();
    }

    public void RefreshInventoryItems()
    {
        HandleResource();
        Dictionary<string, int> itemStacks = new Dictionary<string, int>();
        HashSet<string> shownItemNames = new HashSet<string>();

        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        float itemSlotCellSize = 110f;
    
        int x = 0;
        int y = 0;

        // Dictionary to store the highest count of each item
        Dictionary<string, int> highestCountPerItem = new Dictionary<string, int>();

        foreach (Item item in GameManager.Instance.resourceList)
        {
            if (itemStacks.ContainsKey(item.itemScriptableObject.itemName))
            {
                // Item already exists in the inventory, increment the stack
                itemStacks[item.itemScriptableObject.itemName]++;
            }
            else
            {
                // First occurrence of the item, set stack to 1
                itemStacks[item.itemScriptableObject.itemName] = 1;
            }

            // Update the highest count of each item
            if (!highestCountPerItem.ContainsKey(item.itemScriptableObject.itemName) ||
                itemStacks[item.itemScriptableObject.itemName] > highestCountPerItem[item.itemScriptableObject.itemName])
            {
                highestCountPerItem[item.itemScriptableObject.itemName] = itemStacks[item.itemScriptableObject.itemName];
            }
        }

        foreach (Item item in GameManager.Instance.resourceList)
        {
            if (highestCountPerItem.ContainsKey(item.itemScriptableObject.itemName) &&
                highestCountPerItem[item.itemScriptableObject.itemName] > 0 &&
                !shownItemNames.Contains(item.itemScriptableObject.itemName))
            {
                RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
                itemSlotRectTransform.gameObject.SetActive(true);

                itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
                Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
                image.sprite = item.itemScriptableObject.itemSprite;

                // Display the stacking number
                TextMeshProUGUI stackText = itemSlotRectTransform.Find("StackText").GetComponent<TextMeshProUGUI>();
                stackText.text = highestCountPerItem[item.itemScriptableObject.itemName].ToString();

                shownItemNames.Add(item.itemScriptableObject.itemName);
                y++;
            }
        }
    }

    void Update()
    {
        if (GameManager.Instance.currentHp <=0)
        {

           
            RefreshInventoryItems();




        }
        
    }

}