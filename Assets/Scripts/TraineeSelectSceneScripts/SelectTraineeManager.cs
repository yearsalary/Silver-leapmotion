﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectTraineeManager : MonoBehaviour {
	public Dropdown dropDownBox;

	public void SelectTrainee() {
		string selectedTraineeName = dropDownBox.captionText.text;
		
		GameStatusModel.trainee = GameStatusModel.assistant.traineeList.Find (e => e.getName().Equals (selectedTraineeName));
		SceneManager.LoadScene ("MainMenuScene2");
	}

}
