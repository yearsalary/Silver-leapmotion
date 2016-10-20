using UnityEngine;
using System.Collections;

public class StackBoxChecker : MonoBehaviour {
	
	private bool isSwitchOn = false;

	void OnTriggerStay(Collider col) {
		if (col.tag == "Box")
			isSwitchOn = true;
	}

	void OnTriggerExit(Collider col) {
		if (col.tag == "Box" )
			isSwitchOn= false;
	}

	public bool IsSwitchOn() {
		return this.isSwitchOn;
	}

}
