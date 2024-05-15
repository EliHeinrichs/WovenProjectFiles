using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorManager : MonoBehaviour
{

    public SpriteRenderer armorRenderer;
    public SpriteRenderer backRenderer;
    public ShellScriptableObject shellData;


    private void Start ( )
    {
        armorRenderer = GetComponent<SpriteRenderer> ();
        shellData = GameManager . Instance . shellSlot;
        armorRenderer . sprite = shellData . shellSprite;
        backRenderer . sprite = shellData . shellBackSprite;
    }
    private void Update ( )
    {

        if ( shellData != GameManager . Instance . shellSlot)
        {
            shellData = GameManager . Instance . shellSlot;
            armorRenderer . sprite = shellData . shellSprite;
            backRenderer.sprite = shellData . shellBackSprite;
            

        }
    }

}
