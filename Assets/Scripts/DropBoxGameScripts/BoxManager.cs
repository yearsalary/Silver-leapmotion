using UnityEngine;
using System.Collections;

public class BoxManager : MonoBehaviour
{
    Color boxColor = Color.blue;
    CubeManager cm;

    void Awake()
    {
        cm = GameObject.Find("Cube").GetComponent<CubeManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "CubeSet")
        {
            if(cm.cubeColor == boxColor)
            {
                Debug.Log("111");
                Destroy(other.gameObject);
            }
        }
    }

}
