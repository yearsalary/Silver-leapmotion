using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TableHockeyGameManager : MonoBehaviour {
	public GameObject NetworkCtrl;
	public Dropdown selectRoomDropDown;
	public Text message;
	public Canvas wait_dialogueCanvas;
	public Canvas ready_dialogueCanvas;
	public Canvas gamePlayUI;

	private State currentState;
	private JSONObject currentServerInfo;
	private JSONObject currentJoinedRoom;

	public enum State {
		WAIT,READY,PLAY
	}

	// Use this for initialization
	void Start () {
		this.SetCurrentState (State.WAIT);
		wait_dialogueCanvas.enabled = true;
		ready_dialogueCanvas.enabled = false;
		gamePlayUI.enabled = false;

		message.text = "서버 접속시도 중...";
	}
		
	public void SetServerInfo() {
		List<JSONObject> rooms = currentServerInfo.GetField ("rooms").list;
		Debug.Log ("qqqqqqqqqqqqqqq"+rooms.Count);
		float connectedUserCount = currentServerInfo.GetField ("clientsLength").n;

		List<string> options = new List<string> ();
		rooms.ForEach ((v) => {
			options.Add(v.GetField("title").str);
		});
		selectRoomDropDown.ClearOptions ();
		selectRoomDropDown.AddOptions (options);
		message.text = "서버접속완료.. \n현재 접속인원: " + connectedUserCount + "명";
	}

	public void CreateRoom() {
		NetworkCtrl.GetComponent<TableHockeySocketIOController> ().SendCreateRoomMSg ();
	}

	public void JoinRoom() {
		string roomTitle = selectRoomDropDown.captionText.text;
		NetworkCtrl.GetComponent<TableHockeySocketIOController> ().SendJoinRoomMSg (roomTitle);
	}

	public void LeaveRoom() {
		string currentJoinedRoomTitle = currentJoinedRoom.GetField ("title").str;
		NetworkCtrl.GetComponent<TableHockeySocketIOController> ().SendLeaveRoomMSg (currentJoinedRoomTitle);
	}

	public void PlayReady() {
		Button[] buttons;
		buttons = ready_dialogueCanvas.GetComponentsInChildren<Button> ();
		foreach (Button btn in buttons) {
			if (btn.name.Contains ("StartReadyButton"))
				btn.interactable = false;
		}

	}

	public void WaitGame() {
		Debug.Log ("WAIT");
		wait_dialogueCanvas.enabled = true;
		ready_dialogueCanvas.enabled = false;
		gamePlayUI.enabled = false;
		currentJoinedRoom = null;
		SetServerInfo ();
	}

	public void ReadyGame() {
		Debug.Log ("READY");
		wait_dialogueCanvas.enabled = false;
		ready_dialogueCanvas.enabled = true;
		gamePlayUI.enabled = false;
		string attendants = "";

		currentJoinedRoom.GetField ("attendants").list.ForEach ((v) => {
			attendants += v.str;
			attendants += "/ ";	
		});

		string msg = "방명: " + currentJoinedRoom.GetField ("title").str + "\n" +
		             "방장: " + currentJoinedRoom.GetField ("master").str + "\n" +
		             "참석자: " + attendants;


		ready_dialogueCanvas.GetComponentInChildren<Text> ().text = msg;

	}

	public void PlayGame() {
		Debug.Log ("PLAY");
		this.currentState = State.PLAY;
		wait_dialogueCanvas.enabled = false;
		ready_dialogueCanvas.enabled = false;
		gamePlayUI.enabled = true;
	
	}

	public void SetGameView() {
		switch(currentState) {
		case State.WAIT:
			WaitGame ();
				break;
		case State.READY:
			ReadyGame ();
				break;
		case State.PLAY:
			PlayGame ();
				break;
		}
	}

	public void SetCurrentServerInfo(JSONObject serverInfo) {
		this.currentServerInfo = serverInfo;
	}

	public void SetCurrentJoinedRoom(JSONObject roomInfo) {
		this.currentJoinedRoom = roomInfo;
	}

	public void SetCurrentState(State state) {
		this.currentState = state;
	}

	public State getCurrentState() {
		return this.currentState;
	}

	public JSONObject getCurrentJoinedRoom() {
		return this.currentJoinedRoom;
	}

}

