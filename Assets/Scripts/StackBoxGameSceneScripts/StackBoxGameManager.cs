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
	public Canvas dialogueCanvas;
	public Text dialogueMessage;

	private int level;
	private float time;
	private int point;
	private bool isStopGame;

	// Use this for initialization
	void Start () {
		level = 1;
		InitGame ();
		dialogueCanvas.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		CheckTime ();
		DrawGameInfo ();

		if(handController.GetComponent<HandController>().GetAllGraphicsHands().Length == 0 )
			CheckStackCount ();
	}

	void CheckTime() {
		if (isStopGame)
			return;
		time -= Time.deltaTime;
		if (time <= 0) {
			time = 0;
			FinishGame (false);
		}
	}

	void DrawGameInfo() {
		
		timeText.text = "Time: "+(int)time;
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
			
		if (stackCount == (stackBoxList.Count - 1) * 2 && time <=119f)
			FinishGame (true);
	}

	void InitGame() {
		time = 120f;
		MakeBox (level + 1);
		levelText.text = "Level: "+level;
		isStopGame = false;
	}

	void FinishGame(bool isSucceed) {
		isStopGame = true;

		foreach (GameObject box in stackBoxList)
			Destroy (box);
		stackBoxList.Clear ();

		dialogueMessage.text = "";
		if (isSucceed) {
			dialogueMessage.text += level + "레벨을 성공하였습니다.\n 다음 레벨을 플레이 해보세요.";
			level++;
		} else {
			dialogueMessage.text += level + "레벨을 실패하였습니다.\n 다시 플레이 해보세요.";
		}
		dialogueCanvas.enabled = true;
			
	}

	public void OnPlayButton() {
		dialogueCanvas.enabled = false;
		InitGame ();
	}

	public void OnPlayEndButton() {

	}


				
}
