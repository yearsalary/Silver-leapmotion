﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CubeCraftGameManager : MonoBehaviour {

	public GameObject cube;
	public GameObject parentObject;
	public Text progressText;
	public Material[] color_mtls;
	public float createCubeDistance;


	bool isChangingColor= false;
	int currentCubeColorIndx = 0;
	int cubeId = 0;

		
	void Update() {
		CreateCube ();
		ChangeColor ();
		RotateCube ();
	}

	void CreateCube() {
		GameObject rightRigidHand = GameObject.Find ("RRigidHand(Clone)");
		GrabbingHand grabbingHand;
		Transform[] transformArray;
		GameObject childObj;

		if (rightRigidHand == null)
			return;
		
		grabbingHand = rightRigidHand .GetComponentInChildren<GrabbingHand> ();
		transformArray = rightRigidHand .GetComponentsInChildren<Transform> ();
		foreach (Transform childTransform in transformArray) {
			if (childTransform.name.Contains ("palm") && grabbingHand.GetPinchState ().Equals (GrabbingHand.PinchState.kPinched)) {
				childObj = (GameObject)Instantiate (cube, childTransform.position, Quaternion.LookRotation(Vector3.forward));		
				childObj.transform.parent = parentObject.transform;
				childObj.GetComponent<Renderer> ().material = color_mtls[currentCubeColorIndx];

				childObj.name += cubeId;
				cubeId++;
			}
		}

	}

	void RotateCube() {
		GameObject leftRigidHand = GameObject.Find ("LRigidHand(Clone)");
		GrabbingHand grabbingHand;
		Transform[] transformArray;
		Vector3 handScreenPos;
		Vector3 parentSceenPos = Camera.main.WorldToScreenPoint (parentObject.transform.position);

		if (leftRigidHand == null)
			return;


		grabbingHand = leftRigidHand .GetComponentInChildren<GrabbingHand> ();
		transformArray = leftRigidHand .GetComponentsInChildren<Transform> ();

		foreach (Transform childTransform in transformArray) {
			if (childTransform.name.Contains ("palm")) {
				Debug.Log ("aaaa");
				handScreenPos = Camera.main.WorldToScreenPoint (childTransform.position);
				Debug.Log ("x " + (parentSceenPos.x-handScreenPos.x) + " y " + (parentSceenPos.y-handScreenPos.y));
				float deltaPosX = (parentSceenPos.x - handScreenPos.x);
				float deltaPosY = (parentSceenPos.y - handScreenPos.y);
				float speed = 1f;

				if (Mathf.Abs (deltaPosY) < 100) {
					if (deltaPosX > 0f)
						parentObject.transform.Rotate (Vector3.up, speed, Space.World);
					else
						parentObject.transform.Rotate (Vector3.down, speed, Space.World);
				} else {
					if (deltaPosY > 0f)
						parentObject.transform.Rotate (Vector3.left, speed, Space.World);
					else
						parentObject.transform.Rotate (Vector3.right, speed, Space.World);
				}
				//parentObject.transform.rotation = Quaternion.LookRotation(new Vector3 (childTransform.transform.position.x,childTransform.transform.position.y,childTransform.transform.position.z));
			}
		}
	}

	void ChangeColor() {
		GameObject rightRigidHand = GameObject.Find ("RRigidHand(Clone)");
		Transform[] transformArray;

		if (rightRigidHand != null) {
			transformArray = rightRigidHand .GetComponentsInChildren<Transform> ();
			foreach (Transform childTransform in transformArray) {

				if (childTransform.name.Contains ("palm")) {
					//Debug.Log (childTransform.rotation.z);
					if (!isChangingColor && 0.95 <= childTransform.rotation.z) {
						if (currentCubeColorIndx != color_mtls.Length - 1)
							currentCubeColorIndx++;
						else
							currentCubeColorIndx = 0;
						isChangingColor = true;
					} else if (isChangingColor && 0.1 >= childTransform.rotation.z) {
						isChangingColor = false;
					}
				}
					
			}
		}
	}

	public void Display(float progress) {
		progressText.text = "Exporting objects... (" + Mathf.Round (progress* 100) + "%)";
	}

}