using UnityEngine;
using System.Collections;

public class InitManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log ("도우미: "AssistantModel.name);

		foreach (TraineeModel trainee in AssistantModel.traineeList) {
			Debug.Log ("훈련자:"+ trainee.name);
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
