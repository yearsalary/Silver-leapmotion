using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TableHockeyGameManager : MonoBehaviour {
	public GameObject NetworkCtrl;
	public Dropdown selectRoomDropDown;
	public Text message;
	public State currentState;
	public Canvas wait_dialogueCanvas;
	public Canvas ready_dialogueCanvas;
	public Canvas gamePlayUI;
	private JSONObject currentJoinedRoom;

	public enum State {
		WAIT,READY,PLAY
	}

	// Use this for initialization
	void Start () {
		this.currentState = State.WAIT;
		wait_dialogueCanvas.enabled = true;
		ready_dialogueCanvas.enabled = false;
		gamePlayUI.enabled = false;

		message.text = "서버 접속시도 중...";
	}
		
	public void SetServerInfo(float connectedUserCount, List<JSONObject> rooms) {
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

	public void WaitGame(JSONObject currentServerInfo) {
		this.currentState = State.WAIT;
		wait_dialogueCanvas.enabled = true;
		ready_dialogueCanvas.enabled = false;
		gamePlayUI.enabled = false;

		SetServerInfo (currentServerInfo.GetField ("clientsLength").n, currentServerInfo.GetField ("rooms").list);
	}

	public void ReadyGame(JSONObject currentJoinedRoom) {
		this.currentJoinedRoom = currentJoinedRoom;
		this.currentState = State.READY;
		string attendants = "";

		wait_dialogueCanvas.enabled = false;
		ready_dialogueCanvas.enabled = true;
		gamePlayUI.enabled = false;

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
		this.currentState = State.PLAY;
		wait_dialogueCanvas.enabled = false;
		ready_dialogueCanvas.enabled = false;
		gamePlayUI.enabled = true;
	
	}

}

