using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMiniBoss : MonoBehaviour
{

    public GameObject[] bosses;
    // Start is called before the first frame update
    void Start()
    {
        bosses[Random.Range(0, bosses.Length)].SetActive(true);
    }

   
}
