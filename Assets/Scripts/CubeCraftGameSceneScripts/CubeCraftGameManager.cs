using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CubeCraftGameManager : MonoBehaviour {

	public GameObject cube;
	public GameObject parentObject;
	public Material[] color_mtls;
	public float createCubeDistance;
	public Image[] colorImage;
	private List<GameObject> cubeList;
	bool isChangingColor= false;
	int currentCubeColorIndx = 0;
	int cubeId = 0;
	private bool isPlaying = false;


	public Canvas craftCanvas;
	public Canvas dialogueCanvas;

	void Start() {
		InitGame ();


	}

	void Update() {
		if (!isPlaying)
			return;
		CreateCube ();
		ChangeColor ();
		RotateCube ();
		RemoveCube ();
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
				cubeList.Add (childObj);
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
			if (childTransform.name.Contains ("palm") && !grabbingHand.GetPinchState ().Equals (GrabbingHand.PinchState.kPinched)) {
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
						SetColorImages ();
						isChangingColor = true;
					} else if (isChangingColor && 0.1 >= childTransform.rotation.z) {
						isChangingColor = false;
					}
				}
					
			}
		}
	}

	private void SetColorImages() {
		if (currentCubeColorIndx == 0) {
			colorImage [0].color = color_mtls [color_mtls.Length - 1].color;
			colorImage [1].color = color_mtls [currentCubeColorIndx].color;
			colorImage [2].color = color_mtls [currentCubeColorIndx + 1].color;
		} else if (currentCubeColorIndx == color_mtls.Length - 1) {
			colorImage [0].color = color_mtls [currentCubeColorIndx - 1].color;
			colorImage [1].color = color_mtls [currentCubeColorIndx].color;
			colorImage [2].color = color_mtls [0].color;
		} else {
			colorImage [0].color = color_mtls [currentCubeColorIndx - 1].color;
			colorImage [1].color = color_mtls [currentCubeColorIndx].color;
			colorImage [2].color = color_mtls [currentCubeColorIndx + 1].color;
		}
	}

	private void RemoveCube() {
		GameObject leftRigidHand = GameObject.Find ("LRigidHand(Clone)");
		GrabbingHand grabbingHand;
		Transform[] transformArray;
		GameObject childObj;

		if (leftRigidHand == null)
			return;

		grabbingHand = leftRigidHand .GetComponentInChildren<GrabbingHand> ();
		transformArray = leftRigidHand .GetComponentsInChildren<Transform> ();
		foreach (Transform childTransform in transformArray) {
			if (childTransform.name.Contains ("palm") && grabbingHand.GetPinchState ().Equals (GrabbingHand.PinchState.kPinched)) {
				foreach (GameObject cube in cubeList) {
					if (childTransform.GetComponent<BoxCollider> ().bounds.Contains (cube.transform.position)) {
						cubeList.Remove (cube);
						Destroy (cube);
						return;
					}
				}
			}
		}
	}

	public void InitGame() {
		this.dialogueCanvas.enabled = true;
		this.craftCanvas.enabled = false;
		this.isPlaying = false;

		//TODO: setColor..
		currentCubeColorIndx=0;
		colorImage [0].color = color_mtls [color_mtls.Length - 1].color;
		colorImage [1].color = color_mtls [currentCubeColorIndx].color;
		colorImage [2].color = color_mtls [currentCubeColorIndx + 1].color;

		if (cubeList != null) {
			foreach (GameObject cube in cubeList) {
				Destroy (cube);
			}
			cubeList. Clear();
		}
			
	}

	public void StartGame() {
		this.dialogueCanvas.enabled = false;
		this.craftCanvas.enabled = true;
		this.isPlaying = true;
		cubeList = new List<GameObject> ();
	}

	public void EndGame() {
		SceneManager.LoadScene("MainMenuScene2");
	}
}
