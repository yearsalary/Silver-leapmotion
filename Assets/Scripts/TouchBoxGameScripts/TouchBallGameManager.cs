using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Diagnostics;
using UnityEngine.SceneManagement;


public class TouchBallGameManager : MonoBehaviour {
	public Text levelText;
	public Text timeText;
	public Text pointText;

	public GameObject hitBall;
	public GameObject handController;
	public GameObject ballFactory;

	public Canvas dialogueCanvas;
	public Canvas gamePlayUI;
	public Text dialogueMessage;

	private int level;
	private float time;
	private int point;
	private bool isStopGame;

	void Start() {
		level = 1;
		dialogueMessage.text = "시작\n *.";
		dialogueCanvas.enabled = true;
		gamePlayUI.enabled = false;
		isStopGame = true;
	}

	void Update () {
		if (isStopGame)
			return;

		CheckTime ();
		DrawGameInfo ();

		/*
		if(handController.GetComponent<HandController>().GetAllGraphicsHands().Length == 0 )
			CheckStackCount ();
			*/
	}

	void CheckTime() {
		time -= Time.deltaTime;
		if (time <= 0) {
			time = 0;
			FinishGame (false);
		}
	}

	void DrawGameInfo() {
		timeText.text = "Time: "+(int)time;
	}

	void MakeBall(int count) {
		GameObject ball = (GameObject)Instantiate (hitBall, ballFactory.transform.position, hitBall.transform.rotation);
	}

	void ChecHitBallCount() {
		int hitBallCount = 0;


	}

	void InitGame() {
		time = 120f;
		MakeBall (level + 1);
		levelText.text = "Level: "+level;
		isStopGame = false;
	}

	void FinishGame(bool isSucceed) {
		isStopGame = true;

		/*
		foreach (GameObject box in stackBoxList)
			Destroy (box);
		stackBoxList.Clear ();
		*/


		dialogueMessage.text = "";
		if (isSucceed) {
			dialogueMessage.text += level + "레벨을 성공하였습니다.\n 다음 레벨을 플레이 해보세요.";
			level++;
		} else {
			dialogueMessage.text += level + "레벨을 실패하였습니다.\n 다시 플레이 해보세요.";
		}
		dialogueCanvas.enabled = true;
		gamePlayUI.enabled = false;
	}

	public void OnPlayButton() {
		UnityEngine.Debug.Log("aaa");
		dialogueCanvas.enabled = false;
		gamePlayUI.enabled = true;
		InitGame ();
	}

	public void OnPlayEndButton() {
		//게임 기록 데이터 정리 및 전송..

		SceneManager.LoadScene("MainMenuScene");
	}

	public void OnGameStopButton() {
		UnityEngine.Debug.Log ("aaaaaaaa");
		FinishGame (false);
	}



}
