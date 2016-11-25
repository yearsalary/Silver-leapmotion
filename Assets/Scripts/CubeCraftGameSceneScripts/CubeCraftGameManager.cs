﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CubeCraftGameManager : MonoBehaviour {

	public GameObject cube;
	public GameObject parentObject;
	public Text progressText;

	public Material red_mlt;
	public Material blue_mlt;
	public Material green_mlt;
	public Material wood_mlt;

	bool isChangingColor= false;
	int currentCubeColorIndx = 0;
	int cubeId = 0;

	Color[] cubeColors = {
		Color.red,
		Color.blue,
		Color.green
	};
		
	void Update() {
		CreateCube ();
		ChangeColor ();
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
				childObj.GetComponent<Renderer> ().material = red_mlt;

				childObj.GetComponent<Renderer> ().material.color = cubeColors [currentCubeColorIndx];
				childObj.name += cubeId;
				cubeId++;
			}
		}

	}

	void RotateCube() {
	}

	void ChangeColor() {
		GameObject leftRigidHand = GameObject.Find ("LRigidHand(Clone)");
		Transform[] transformArray;

		if (leftRigidHand != null) {
			transformArray = leftRigidHand.GetComponentsInChildren<Transform> ();
			foreach (Transform childTransform in transformArray) {

				if (childTransform.name.Contains ("palm")) {
					//Debug.Log (childTransform.rotation.z);
					if (!isChangingColor && 0.95 <= childTransform.rotation.z) {
						Debug.Log ("aaa");
						if (currentCubeColorIndx != cubeColors.Length - 1)
							currentCubeColorIndx++;
						else
							currentCubeColorIndx = 0;
						isChangingColor = true;
					} else if (isChangingColor && 0.1 >= childTransform.rotation.z) {
						Debug.Log ("bbb");
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
