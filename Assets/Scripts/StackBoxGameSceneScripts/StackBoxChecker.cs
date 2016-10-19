using UnityEngine;
using System.Collections;

public class StackBoxChecker : MonoBehaviour {
	

	// Update is called once per frame
	void Update () {
		
	}

	void MoveDown() {
			this.transform.Translate (Vector3.down * 1 * Time.deltaTime);
	}

	void OnTriggerEnter(Collider col) {
		if (col.tag == "Box" ) {
			Debug.Log ("aaa");
		}
			
	}
		

		
}
