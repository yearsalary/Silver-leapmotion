using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoxManager : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CubeSet")
        {
            Debug.Log(other.GetComponentInParent<Renderer>().material.color);
            if (other.GetComponentInParent<Renderer>().material.color == GetComponent<Renderer>().material.color)
            {
                Destroy(other.gameObject);
            }
        }
        
    }

}
