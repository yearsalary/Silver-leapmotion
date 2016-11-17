using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssistantModel {
	public string id;
	public string name;
	public List<TraineeModel> traineeList;

	public AssistantModel(string id, string name, List<TraineeModel> traineeList) {
		this.id = id;
		this.name = name;
		this.traineeList = traineeList;
	}

    public AssistantModel(List<TraineeModel> traineeList) {
		this.traineeList = traineeList;
	}
}
