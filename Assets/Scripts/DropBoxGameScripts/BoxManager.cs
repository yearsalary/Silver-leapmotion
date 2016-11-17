using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoxManager : MonoBehaviour
{
    List<GameObject> dropBoxList;

    void OnTriggerEnter(Collider other)
    {
        
        dropBoxList = GameObject.Find("DropBoxGameManagers").GetComponent<DropBoxGameManager>().dropBoxList;
        
        if (other.gameObject.tag == "CubeSet")
        {
            
            if (other.GetComponentInParent<Renderer>().material.color == GetComponentInParent<Renderer>().material.color)
            {
                Destroy(other.gameObject);
                
                
            }
        }
        
    }

}
