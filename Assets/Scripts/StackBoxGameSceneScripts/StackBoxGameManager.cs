using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Diagnostics;

public class StackBoxGameManager : MonoBehaviour {
	public Text levelText;
	public Text timeText;
	public Text pointText;

	private int level;
	private float time;
	private int point;

	// Use this for initialization
	void Start () {
		level = 1;
		time = 120f;
		point = 0;

		DrawGameInfo ();

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
		
}
