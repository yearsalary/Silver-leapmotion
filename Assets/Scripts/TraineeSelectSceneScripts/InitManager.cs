using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InitManager : MonoBehaviour {

	public Text systemMessage;
	public Dropdown selectTraineeDropDown;

	// Use this for initialization
	void Start () {
		List<string> options = new List<string> ();

		foreach (TraineeModel trainee in GameStatusModel.assistant.traineeList) {
			Debug.Log ("훈련자:" + trainee.name);
			options.Add (trainee.name);
		}

		systemMessage.text = GameStatusModel.assistant.name + " 도우미님 환영합니다.";

		selectTraineeDropDown.ClearOptions ();
		selectTraineeDropDown.AddOptions (options);


	}

}
