using UnityEngine;
using System.Collections;

public class CubeCraft_Cube : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider col) {
		//if (col.GetComponentInParent<Transform>().transform.name.Contains("RRigidHand(Clone)"))
		Transform[] transforms = col.GetComponentsInParent<Transform>();
		foreach(Transform parentTransform in transforms) {
			if(parentTransform.name.Contains("RRigidHand(Clone)"))
				Debug.Log("SSS");
		}
	}
}
