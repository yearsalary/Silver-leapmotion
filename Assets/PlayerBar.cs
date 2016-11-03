using UnityEngine;
using System.Collections;

public class PlayerBar : MonoBehaviour {
	public GameObject handController;
	public GameObject ball;
	public GameObject netWorkCtrl;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		ChaseHand ();
	}

	void ChaseHand() {
		GameObject hand = getChildGameObject (handController, "RigidHand(Clone)");
		if (hand == null)
			return;

		GameObject palm = getChildGameObject (hand, "palm");
		if (palm != null) {
			gameObject.transform.position = new Vector3(palm.transform.position.x, gameObject.transform.position.y, palm.transform.position.z);
			//Debug.Log (gameObject.transform.position.ToString());
			//Debug.Log (palm.transform.position.ToString());
		}
			
	}

	static public GameObject getChildGameObject(GameObject fromGameObject, string withName) {
		
		Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
		foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
		return null;
	}
		
	void OnCollisionEnter(Collision col) 
	{ 
		if (col.gameObject.Equals (ball)) {
			netWorkCtrl.GetComponent<SocketIOController> ().SendBallCollisonMsg ();
			Vector3 reflectedVector = Vector3.Reflect (col.contacts [0].point, col.contacts [0].normal.normalized);
			//Debug.Log (reflectedVector.ToString ());
			reflectedVector.Set (reflectedVector.x,0f,reflectedVector.z);
			ball.GetComponent<TableHockeyBall> ().SetMoveDirection (reflectedVector);
		}
	}
}