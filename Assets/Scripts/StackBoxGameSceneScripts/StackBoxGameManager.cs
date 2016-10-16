﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Diagnostics;

enum BoxColors {
	
}

public class StackBoxGameManager : MonoBehaviour {
	public Text levelText;
	public Text timeText;
	public Text pointText;
	public GameObject boxFactory;
	public GameObject stackBox;

	private int level;
	private float time;
	private int point;

	// Use this for initialization
	void Start () {
		level = 1;
		time = 120f;
		point = 0;
		MakeBox (10);
	}
	
	// Update is called once per frame
	void Update () {

		CheckTime ();
		DrawGameInfo ();
	}

	void CheckTime() {
		time -= Time.deltaTime;
	}

	void DrawGameInfo() {
		levelText.text = "Level: "+level;
		timeText.text = "Time: "+time;
		pointText.text = "Point: "+point;
	}

	void MakeBox(int count) {
		GameObject box;
		Color boxColor;

		for (int i = 0; i < count; i++) {
			box = (GameObject)Instantiate (stackBox, boxFactory.transform.position, stackBox.transform.rotation);
			switch (i % 4) {
			case 0:
				box.GetComponent<Renderer> ().material.color = Color.yellow;
				break;
			case 1:
				box.GetComponent<Renderer> ().material.color = Color.red;
				break;
			case 2:
				box.GetComponent<Renderer> ().material.color = Color.green;
				break;
			case 3:
				box.GetComponent<Renderer> ().material.color = Color.blue;
				break;
			}
		}
	}
		
}
