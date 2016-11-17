using UnityEngine;
using System.Collections;

public class MonsterManager : MonoBehaviour {
    //public GameObject monster;

	void onCollisionEnter(Collision coll)
    {
        if(coll.collider.tag == "BULLET")
        {
            Debug.Log("aa");
            Destroy(coll.gameObject);
        }
    }
}
