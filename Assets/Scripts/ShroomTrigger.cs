using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomTrigger : MonoBehaviour
{
 

    void OnTriggerEnter2D(Collider2D other)
    {
 
        if(other.CompareTag("Player"))
        {
           gameObject.GetComponent<Animator>().SetTrigger("Brush");   
        }
    }

    
}
