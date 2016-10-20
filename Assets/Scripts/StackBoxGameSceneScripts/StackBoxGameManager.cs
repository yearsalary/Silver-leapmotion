using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Diagnostics;

public class StackBoxGameManager : MonoBehaviour {
	public Text levelText;
	public Text timeText;
	public Text pointText;
	public GameObject boxFactory;
	public GameObject stackBox;
	public GameObject handController;
	List<GameObject> stackBoxList;


	private int level;
	private float time;
	private int point;

	// Use this for initialization
	void Start () {
		level = 1;
		time = 120f;
		point = 0;
		MakeBox (5);
	}
	
	// Update is called once per frame
	void Update () {
		CheckTime ();
		DrawGameInfo ();

		if(handController.GetComponent<HandController>().GetAllGraphicsHands().Length == 0 )
			CheckStackCount ();
	}

	void CheckTime() {
		time -= Time.deltaTime;
	}

	void DrawGameInfo() {
		levelText.text = "Level: "+level;
		timeText.text = "Time: "+time;
	}

	void MakeBox(int count) {
		GameObject box;
		stackBoxList = new List<GameObject> ();

		for (int i = 0; i < count; i++) {
			box = (GameObject)Instantiate (stackBox, boxFactory.transform.position, stackBox.transform.rotation);
			stackBoxList.Add(box);

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

	void CheckStackCount() {
		int stackCount = 0;

		foreach (GameObject box in stackBoxList) {
			foreach (StackBoxChecker checker in box.GetComponentsInChildren<StackBoxChecker>()) {
				if (checker.IsSwitchOn ())
					stackCount++;
				//UnityEngine.Debug.Log (checker.GetInstanceID () + ": " + checker.IsSwitchOn ());

			}
		}
			
		if (stackCount == (stackBoxList.Count - 1) * 2)
			UnityEngine.Debug.Log ("aaaaaaaaaa");
	
	}
		
		
}
