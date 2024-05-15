using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeOrbScript : MonoBehaviour
{
    public GameObject orb;
    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.orbRestored == true)
        {
            orb.SetActive(true);
        }
    }
}
