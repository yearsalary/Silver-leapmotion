using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TableHockeyGameManager : MonoBehaviour {
	public GameObject NetworkCtrl;
	public Dropdown selectRoomDropDown;
	public Canvas wait_dialogueCanvas;
	public Canvas ready_dialogueCanvas;
	public Canvas end_dialogueCanvas;
	public Canvas gamePlayUI;


	private TableHockeySocketIOController socketIOCtrl;
	private State currentState;
	private JSONObject currentServerInfo;
	private JSONObject currentJoinedRoom;

	private Button playReadyBtn;
	private Button playReadyCancelBtn;
	private Button leaveRoomBtn;

	private float time = 30f;
	private float playerPoint = 0f;
	private float opponentPlayerPoint = 0f;

	public enum State {
		WAIT,READY,PLAY,END
	}

	// Use this for initialization
	void Start () {
		socketIOCtrl = NetworkCtrl.GetComponent<TableHockeySocketIOController> ();
		this.SetCurrentState (State.WAIT);
		wait_dialogueCanvas.enabled = true;
		ready_dialogueCanvas.enabled = false;
		end_dialogueCanvas.enabled = false;
		gamePlayUI.enabled = false;

		findChildrenTxt(wait_dialogueCanvas, "Message").text = "서버 접속시도 중...";

		playReadyBtn = findChildrenBtn (ready_dialogueCanvas,"StartReadyButton");
		playReadyCancelBtn = findChildrenBtn (ready_dialogueCanvas,"StartReadyCancelButton");
		leaveRoomBtn = findChildrenBtn (ready_dialogueCanvas, "LeaveRoomButton");

		playReadyBtn.interactable = true;
		playReadyCancelBtn.interactable = false;
	}
		
	private IEnumerator SetPlayTimer() {
		while(time>=0) {
			yield return new WaitForSeconds (1);
			time -= 1;
			socketIOCtrl.SendPlayTimeMSg (time);
		}
		//시간종료...
		socketIOCtrl.SendPlayEndMSg();

	}
		
	public void SetServerInfo() {
		List<JSONObject> rooms = currentServerInfo.GetField ("rooms").list;
		float connectedUserCount = currentServerInfo.GetField ("clientsLength").n;

		List<string> options = new List<string> ();
		rooms.ForEach ((v) => {
			options.Add(v.GetField("title").str);
		});
		selectRoomDropDown.ClearOptions ();
		selectRoomDropDown.AddOptions (options);
		findChildrenTxt(wait_dialogueCanvas, "Message").text = "서버접속완료.. \n현재 접속인원: " + connectedUserCount + "명";
	}

	public void CreateRoom() {
		socketIOCtrl.SendCreateRoomMSg ();
	}

	public void JoinRoom() {
		string roomTitle = selectRoomDropDown.captionText.text;
		socketIOCtrl.SendJoinRoomMSg (roomTitle);
	}

	public void LeaveRoom() {
		string currentJoinedRoomTitle = currentJoinedRoom.GetField ("title").str;
		socketIOCtrl.SendLeaveRoomMSg (currentJoinedRoomTitle);
	}

	public void PlayReady() {
		playReadyBtn.interactable = false;
		leaveRoomBtn = false;
		playReadyCancelBtn.interactable = true;
		socketIOCtrl.SendPlayReadyMSg ();
	}

	public void PlayReadyCancel() {
		playReadyCancelBtn.interactable = false;
		leaveRoomBtn = true;
		playReadyBtn.interactable = true;
		socketIOCtrl.SendPlayReadyCancelMSg ();
	}
		
	public void WaitGame() {
		Debug.Log ("WAIT");
		wait_dialogueCanvas.enabled = true;
		ready_dialogueCanvas.enabled = false;
		end_dialogueCanvas.enabled = false;
		gamePlayUI.enabled = false;
		currentJoinedRoom = null;
		SetServerInfo ();
	}

	public void ReadyGame() {
		Debug.Log ("READY");
		wait_dialogueCanvas.enabled = false;
		ready_dialogueCanvas.enabled = true;
		end_dialogueCanvas.enabled = false;
		gamePlayUI.enabled = false;
		string attendants = "";

		currentJoinedRoom.GetField ("attendants").list.ForEach ((v) => {
			attendants += v.GetField("name").str;
			attendants += "(준비상태:"+v.GetField("ready").b+")";
			attendants += "/ ";	
		});

		string msg = "방명: " + currentJoinedRoom.GetField ("title").str + "\n" +
		             "방장: " + currentJoinedRoom.GetField ("master").str + "\n" +
		             "참석자: \n" + attendants;


		ready_dialogueCanvas.GetComponentInChildren<Text> ().text = msg;

	}

	public void PlayGame() {
		Debug.Log ("PLAY");
		wait_dialogueCanvas.enabled = false;
		ready_dialogueCanvas.enabled = false;
		end_dialogueCanvas.enabled = false;
		gamePlayUI.enabled = true;
		SetPlayPointView ();
		//master initGame
		if (currentJoinedRoom.GetField ("master").str.Equals (socketIOCtrl.name)) {
			this.setPlayerPoint (0f);
			this.setOpponentPlayerPoint(0f);
			this.time = 30f;
			StartCoroutine (SetPlayTimer ());
			NetworkCtrl.GetComponent<TableHockeySocketIOController>().SendBallOwnerChangeMsg ();
			NetworkCtrl.GetComponent<TableHockeySocketIOController> ().ball.transform.position = new Vector3 (0f, 0f, -4.5f);
		}
			
	}

	public void EndGame() {
		Debug.Log ("END");
		wait_dialogueCanvas.enabled = false;
		ready_dialogueCanvas.enabled = false;
		end_dialogueCanvas.enabled = true;
		gamePlayUI.enabled = false;
		string msg = "결과\n";

		msg += socketIOCtrl.name +" " + playerPoint +" :";
		msg += opponentPlayerPoint + " " + socketIOCtrl.getOtehrPlayerName ();
		msg += "\n";

		if (playerPoint > opponentPlayerPoint)
			msg += "승리하였습니다.";
		else if (playerPoint == opponentPlayerPoint)
			msg += "무승부 입니다.";
		else
			msg += "패배하였습니다.";


		findChildrenTxt (end_dialogueCanvas, "Message").text = msg;
	}

	public void ExitGame() {
		Debug.Log("EXIT");
		socketIOCtrl.SendPlayExitMSg();
		SceneManager.LoadScene("MainMenuScene");
	}

	public void GoBackReady() {
		playReadyBtn.interactable = true;
		leaveRoomBtn.interactable = true;
		playReadyCancelBtn.interactable = false;
		currentState = State.READY;
		SetGameView ();
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
		case State.END:
			EndGame ();
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

	public void setPlayerPoint(float point) {
		this.playerPoint = point;
	}

	public void setOpponentPlayerPoint(float point) {
		this.opponentPlayerPoint = point;
	}

	public State getCurrentState() {
		return this.currentState;
	}

	public JSONObject getCurrentJoinedRoom() {
		return this.currentJoinedRoom;
	}

	public float getPlayerPoint() {
		return this.playerPoint;
	}

	public float getOpponentPlayerPoint() {
		return this.opponentPlayerPoint;
	}
		
	public void SetPlayTimeView() {
		Text txt = findChildrenTxt (gamePlayUI,"TimeText");
		txt.text = "Time: " + time;
	}

	public void SetPlayPointView() {
		Text txt = findChildrenTxt (gamePlayUI,"PointText");
		txt.text = socketIOCtrl.name +" " + playerPoint +" :";
		txt.text += opponentPlayerPoint + " " + socketIOCtrl.getOtehrPlayerName ();
	}

	public void SetPlayTime(float time) {
		this.time = time;
	}
		
	private Button findChildrenBtn(Canvas canvas, string name) {
		Button[] buttons;
		buttons = canvas.GetComponentsInChildren<Button> ();

		foreach (Button btn in buttons) {
			if (btn.name.Contains (name))
				return btn;
		}
		return null;
	}

	private Text findChildrenTxt(Canvas canvas, string name) {
		Text[] texts;
		texts = canvas.GetComponentsInChildren<Text> ();

		foreach (Text text in texts) {
			if (text.name.Contains (name))
				return text;
		}
		return null;
	}


}

