using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CodeMonkey.Utils;
using System.Reflection;

public class StorageUI : MonoBehaviour
{

    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;

    private List<Item> lastStorageList;
    public AudioClip inventorySFX;


    private void Start()
    {
        itemSlotContainer = transform.Find("ItemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("SlotTemplate");
        lastStorageList = new List<Item>(GameManager.Instance.storageList);
        RefreshStorage();

    }


    void Update()
    {
        if (!ListsAreEqual(lastStorageList, GameManager.Instance.storageList))
        {
            // Update lastStorageList
            lastStorageList = new List<Item>(GameManager.Instance.storageList);

            // Refresh the storage UI
            RefreshStorage();
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

    public void RefreshStorage()
    {
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);


        }

        int x = 0;
        int y = 0;
        float itemSlotCellSize =  200f;

        foreach (Item item in GameManager.Instance.storageList)
        {


            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            itemSlotRectTransform.GetComponent<Button_UI>().MouseRightClickFunc = () =>
            {


                if (GameManager.Instance.itemList.Count < 5)
                {
                    GameManager.Instance.storageList.Remove(item);
                    GameManager.Instance.itemList.Add(item);
                }

            };

            itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () => {

                if (GameManager.Instance.itemList.Count < 5)
                {
                    GameManager.Instance.storageList.Remove(item);
                    GameManager.Instance.itemList.Add(item);
                }


            };



            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
            image.sprite = item.itemScriptableObject.itemSprite;



            x++;

            if (x > 3)
            {

                x = 0;
                y++;
            }


        }
    }




}
