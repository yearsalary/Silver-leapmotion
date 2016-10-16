using UnityEngine;
using System.Collections;

public class StackBoxChecker : MonoBehaviour {
	public Vector3 originPos;
	public double maxHeight;
	public GameObject handController;

	void Start() {
		originPos = this.transform.position;
		maxHeight = -10.0;
	}

	// Update is called once per frame
	void Update () {
		
		if (handController.GetComponentsInChildren<GrabbingHand> ().Length == 0)
			MoveDown();
		else
			this.transform.position = originPos;

		Debug.Log (maxHeight);
	}

	void MoveDown() {
			this.transform.Translate (Vector3.down * 1 * Time.deltaTime);
	}

	void OnTriggerEnter(Collider col) {
		if (col.tag == "Box") {
			if(maxHeight < col.transform.position.y)
				maxHeight = col.transform.position.y;
			this.transform.position = originPos;
		}
			
	}
		

		
}
