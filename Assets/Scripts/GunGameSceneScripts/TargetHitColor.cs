using UnityEngine;
using System.Collections;

public class TargetHitColor : MonoBehaviour {
    public string color;
    //GameObject target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BULLET")
        {
            GameObject.Find("Target2").GetComponent<TargetHit>().setColor(color);
            //Debug.Log("ccc : " + color);
            
        }
    }
}
