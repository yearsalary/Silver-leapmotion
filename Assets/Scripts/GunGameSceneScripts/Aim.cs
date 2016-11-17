using UnityEngine;
using System.Collections;

public class Aim : MonoBehaviour {
    public GameObject aim;
    private Transform[] v;
    Vector3 palmPosition;
    
    // Use this for initialization
 
	
	// Update is called once per frame
	void Update () {
        v = GameObject.Find("LRigidHand(Clone)").GetComponentsInChildren<Transform>();

        foreach (Transform child in v)
        {
            if (child.name.Contains("palm"))
            {
                palmPosition = child.position;
                palmPosition.z = -1.5f;
                aim.transform.position = palmPosition;
            }
        }
    }
}
