using UnityEngine;
using System.Collections;

public class StackBoxChecker : MonoBehaviour {
	
	private bool isSwitchOn = false;
	private bool isOnGround= false;

	void OnTriggerEnter(Collider col) {
		if (col.tag == "Box")
			isSwitchOn = true;
		if (col.tag == "Ground")
			isOnGround = true;
	}

	void OnTriggerExit(Collider col) {
		if (col.tag == "Box" )
			isSwitchOn= false;
		if (col.tag == "Ground") 
			isOnGround = false;
	}

	public bool IsSwitchOn() {
		return this.isSwitchOn;
	}

	public bool IsOnGround() {
		return this.isOnGround;
	}
}
