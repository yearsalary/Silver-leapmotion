using UnityEngine;
using System.Collections;

public class BoxManager : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "CubeSet")
        {
            GetComponent<Renderer>().material.color = Color.blue;
            Destroy(other.gameObject);
        }
    }

}
