using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CubeCraftGameManager : MonoBehaviour {

	public GameObject cube;
	public GameObject parentObject;
	public Text progressText;

	void Update() {
		CreateCube ();
	}

	void CreateCube() {
		GameObject rigidHand = GameObject.Find ("RigidHand(Clone)");
		GrabbingHand grabbingHand;
		Transform[] transformArray;
		GameObject childObj;

		if (rigidHand == null)
			return;
		
		grabbingHand = rigidHand.GetComponentInChildren<GrabbingHand> ();
		transformArray = rigidHand.GetComponentsInChildren<Transform> ();
		foreach (Transform childTransform in transformArray) {
			if (childTransform.name.Contains ("palm") && grabbingHand.GetPinchState ().Equals (GrabbingHand.PinchState.kPinched)) {
				childObj = (GameObject)Instantiate (cube, childTransform.position, Quaternion.LookRotation(Vector3.forward));		
				childObj.transform.parent = parentObject.transform;
			}
		}

	}

	public void Display(float progress) {
		progressText.text = "Exporting objects... (" + Mathf.Round (progress* 100) + "%)";
	}

}
