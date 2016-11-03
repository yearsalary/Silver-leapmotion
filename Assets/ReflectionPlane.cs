using UnityEngine;
using System.Collections;

public class ReflectionPlane : MonoBehaviour {
	public GameObject ball;
	bool isFirstColEnter;

	void Start() {
		isFirstColEnter = true;
	}

	void OnCollisionStay(Collision col) 
	{ 
		//Debug.Log ("Stay");
		if (col.gameObject.Equals (ball) && isFirstColEnter) {
			Vector3 reflectedVector = Vector3.Reflect (ball.GetComponent<TableHockeyBall>().getMoveDirection(),col.contacts [0].normal.normalized);
			Debug.Log (reflectedVector.ToString ());
			reflectedVector.Set (reflectedVector.x,0f,reflectedVector.z);
			ball.GetComponent<TableHockeyBall> ().SetMoveDirection (reflectedVector);
			isFirstColEnter = false;
		}
	}
		
	void OnCollisionExit(Collision col) {
		//Debug.Log ("exit");
		isFirstColEnter = true;
	}
}
