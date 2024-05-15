using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonScript : MonoBehaviour
{
    public GameObject Summon;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(Summon, transform.position, Quaternion.identity);
    }

 
}
