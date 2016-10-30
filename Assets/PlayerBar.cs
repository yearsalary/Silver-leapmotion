using UnityEngine;
using System.Collections;

public class PlayerBar : MonoBehaviour {
	public GameObject handController;
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
}
