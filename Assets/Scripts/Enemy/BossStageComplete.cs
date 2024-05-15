using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStageComplete : MonoBehaviour
{
    public GameObject endStageManager;
   void OnDestroy()
    {
        endStageManager.SetActive(true);
    }
}
