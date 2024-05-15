using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{

    Slash,
    Stab,
    Shoot,
    Slam,
    SummonStaff,


}

[CreateAssetMenu(menuName = "ScriptableObjects/WeaponDataScriptableObject")]
public class WeaponScriptableObject : ScriptableObject
{
    public string weaponName;
    public int attackDamage;
    public float attackSpeed;
    public Sprite weaponSprite;
    public WeaponType attackType;
    public GameObject projectile;
    public string weaponDesc;

}