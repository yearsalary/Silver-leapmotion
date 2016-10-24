using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoxManager : MonoBehaviour
{
    //int cubCount;
    //public bool isStopGame = false;
    List<GameObject> dropBoxList;

    void OnTriggerEnter(Collider other)
    {
        //cubCount = GameObject.Find("DropBoxGameManagers").GetComponent<DropBoxGameManager>().cubeCount;
        dropBoxList = GameObject.Find("DropBoxGameManagers").GetComponent<DropBoxGameManager>().dropBoxList;
        Debug.Log(dropBoxList.Count);
        if (other.gameObject.tag == "CubeSet")
        {
            //Debug.Log(other.GetComponentInParent<Renderer>().material.color);
            if (other.GetComponentInParent<Renderer>().material.color == GetComponentInParent<Renderer>().material.color)
            {
                Destroy(other.gameObject);
                //cubCount--;

                /*if(cubCount == 0)
                {
                    isStopGame = true;
                }*/
                
            }
        }
        
    }

}
