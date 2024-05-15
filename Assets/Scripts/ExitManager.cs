using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DoorData
{
    public GameObject roomTypeDoor;
 

}


public class ExitManager : MonoBehaviour
{
    public DoorData baseRoom;
    public Transform[] spawnDoorPositions;
    public List<DoorData> doorData;
    public DoorData finalRoom;




    private void Start()
    {
        int baseCount = 0;
        foreach (Transform t in spawnDoorPositions)
        {
            if (baseCount < 1)
            {
                Instantiate(baseRoom.roomTypeDoor, t.position, Quaternion.identity);
              
                baseCount++;
            }
            else
            {
                if(GameManager.Instance.gameStage >= 2)
                {
                    doorData.Add(finalRoom);
                }

                DoorData uniqueDoor = doorData[Random.Range(0, doorData.Count)];
                Instantiate(uniqueDoor.roomTypeDoor, t.position, Quaternion.identity);

            }



        }
    }


}
