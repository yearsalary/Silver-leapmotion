using UnityEngine;
using System.Collections;

public class TraineeModel {
	private string id;
	private string name;

	public TraineeModel(string id, string name) {
		this.id = id;
		this.name = name;
	}

    public string getId()
    {
        return id;
    }

    public string getName()
    {
        return name;
    }
}
