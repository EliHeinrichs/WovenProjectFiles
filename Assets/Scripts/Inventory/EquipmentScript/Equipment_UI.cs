using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Equipment_UI : MonoBehaviour
{

    public Image weaponImg;
    public Image shellImg;

    public TextMeshProUGUI speedText;
    public TextMeshProUGUI lightText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI hungerText;
    public TextMeshProUGUI rezText;
    public TextMeshProUGUI summonText;
    public TextMeshProUGUI attackSpeed;
    public TextMeshProUGUI armor;


    public TextMeshProUGUI floorText;

    public TextMeshProUGUI weaponText;
    public TextMeshProUGUI shellText;



    void Start()
    {
     

        RefreshEquipmentItems();
    }


    public void RefreshEquipmentItems()
    {
        weaponImg.sprite = GameManager.Instance.weaponSlot.weaponSprite;
        shellImg.sprite = GameManager.Instance.shellSlot.shellSprite;
        weaponText.text = GameManager.Instance.weaponSlot.name;
        shellText.text = GameManager.Instance.shellSlot.name;

    }

    void Update()
    {
        speedText.text = "MoveSpeed: " + GameManager.Instance.moveSpeed;
        lightText.text = "Light: " + GameManager.Instance.light;
        damageText.text = "Damage: " + GameManager.Instance.resourceATK + GameManager.Instance.weaponSlot.attackDamage;
        healthText.text = "Health: " + GameManager.Instance.currentHp + "/" + GameManager.Instance.maxHp;
        staminaText.text = "Stamina: " + GameManager.Instance.stamina.ToString("F0") + "/" + GameManager.Instance.maxStamina;
        hungerText.text = "Hunger: " + GameManager.Instance.hunger.ToString("F0") + "/" + GameManager.Instance.maxHunger;
        rezText.text = "Ressurects: " + GameManager.Instance.rez;
        summonText.text = "Max Summons: " + GameManager.Instance.maxSummons;
        attackSpeed.text = "Attack Speed: " + GameManager.Instance.weaponSlot.attackSpeed;
        armor.text = "Armor: " + GameManager.Instance.armor;
        floorText.text = "Depth: " + GameManager.Instance.level;

        if(weaponImg.sprite != GameManager.Instance.weaponSlot.weaponSprite)
        {
            RefreshEquipmentItems();
        }
        if(shellImg.sprite != GameManager.Instance.shellSlot.shellSprite)
        {
            RefreshEquipmentItems();
        }
    }
}