using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSequence : MonoBehaviour
{
    public GameObject[] destroyedTerrain;
    public GameObject bossStage2;
    public GameObject explosionParticle;



    void Start()
    {
        StartCoroutine(stage2Sequence());

        explosionParticle.SetActive(true);
    }

    public IEnumerator stage2Sequence()
    {
     
        foreach (GameObject go in destroyedTerrain)
        {
            Destroy(go);
        }

        yield return new WaitForSeconds(0.6f);
        bossStage2.SetActive(true);
    }
}
