using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TraineeSelectSceneInitManager : MonoBehaviour {

	public Text systemMessage;
	public Dropdown selectTraineeDropDown;

	// Use this for initialization
	void Start () {
		List<string> options = new List<string> ();

		foreach (TraineeModel trainee in GameStatusModel.assistant.traineeList)
			options.Add (trainee.getName());
		
		systemMessage.text = GameStatusModel.assistant.name + " 도우미님 환영합니다.";

		selectTraineeDropDown.ClearOptions ();
		selectTraineeDropDown.AddOptions (options);

	}

}
