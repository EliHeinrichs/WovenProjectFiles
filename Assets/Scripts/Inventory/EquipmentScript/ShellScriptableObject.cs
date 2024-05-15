using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (menuName = "ScriptableObjects/ShellDataScriptableObject")]
public class ShellScriptableObject : ScriptableObject
{
    public Sprite shellSprite;
    public Sprite shellBackSprite;
    public int shellHealth;
    public float shellStamina;
    public int shellHunger;
    public int shellArmor;
    public string shellName;
    public int shellSummon;
    public string shellDesc;
}
