using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SocketIO;

public class TableHockeySocketIOController: MonoBehaviour {

	public SocketIOComponent socket;
	public GameObject gameManager;
	public GameObject playerBar;
	public GameObject opponentBar;
	public GameObject ball;
	private TableHockeyGameManager tableHockeyGameManager;

	private string ballOwner = "";
	public string name;


	// Use this for initialization
	void Start () {
		tableHockeyGameManager = gameManager.GetComponent<TableHockeyGameManager> ();
		StartCoroutine (ConnectToServer ());
		socket.On ("USER_CONNECTED", OnUserConnected);
		socket.On ("USER_DISCONNECTED", OnUserDisConnected);
		//room related...
		socket.On ("CREATED_ROOM", OnCreatedRoom);
		socket.On ("DESTROY_ROOM", OnDestroyRoom);
		socket.On ("JOINED_ROOM", OnJoinedRoom);
		socket.On ("LEFT_ROOM", OnLeftRoom);
		socket.On ("READY_CHANGE", OnUserPlayReadyChange);

		//gameplay related...
		socket.On ("PLAY", OnUserPlay);
		socket.On ("PLAY_ENDED", OnUserPlayEnded);

	}

	IEnumerator ConnectToServer() {
		Dictionary<string, string> data = new Dictionary<string, string> ();
		data ["name"] = name;

		yield return new WaitForSeconds (0.5f);
		socket.Emit ("USER_CONNECT", new JSONObject(data));
		yield return new WaitForSeconds (1f);

		//socket.Emit ("PLAY", new JSONObject (data));
	}

	// Update is called once per frame
	void Update () {

		if (!tableHockeyGameManager.getCurrentState().Equals(TableHockeyGameManager.State.PLAY))
			return;
		
		SendBarMoveMSg();

		if (ballOwner.Equals (name))
			SendBallMoveMSg ();

		if (Vector3.Distance (playerBar.transform.position, ball.transform.position) < 5f && ball.transform.position.z<=0f)
			SendBallOwnerChangeMsg ();

	}
	
	private void OnUserConnected(SocketIOEvent evt) {
		JSONObject currnetUser = evt.data.GetField ("currentUser");
		JSONObject currentServerInfo = evt.data.GetField ("currentServerInfo");

		Debug.Log ("Get the msg from server is: " + currentServerInfo.GetField("clientsLength").n +" OnUserConnected ");
		Debug.Log ("Get the msg from server is: " + currentServerInfo.GetField("rooms") +" OnUserConnected ");

		tableHockeyGameManager.SetCurrentServerInfo (currentServerInfo);

		if (currnetUser.GetField ("name").str.Equals (name)) 
			tableHockeyGameManager.SetCurrentState (TableHockeyGameManager.State.WAIT);

		tableHockeyGameManager.SetGameView ();

	}

	private void OnUserDisConnected(SocketIOEvent evt) {
		JSONObject currentServerInfo = evt.data;

		Debug.Log ("Get the msg from server is: " + currentServerInfo.GetField("clientsLength").n +" OnUserDisConnected ");
		Debug.Log ("Get the msg from server is: " + currentServerInfo.GetField("rooms") +" OnUserDisConnected ");
	

		tableHockeyGameManager.SetCurrentServerInfo (currentServerInfo);

		tableHockeyGameManager.SetGameView ();

	}

	private void OnUserPlayReadyChange(SocketIOEvent evt) {
		JSONObject roomInfo = evt.data;
		Debug.Log ("Get the msg from server is: " + evt.data + " OnUserPlayReadyChange ");

		tableHockeyGameManager.SetCurrentJoinedRoom (roomInfo);
		tableHockeyGameManager.SetGameView ();
	}

	private void OnUserPlay(SocketIOEvent evt) {
		Debug.Log ("Get the msg from server is: " + evt.data + " OnUserPlay ");
		socket.On ("MOVE", OnUSerMove);
		socket.On ("BALL_OWNER_CHANGE", OnBallOwnerChange);
		socket.On ("BALL_MOVE", OnBallMove);
		socket.On ("PLAY_TIME_CHANGED", OnTimeChange);
		socket.On ("PLAY_POINT_CHANGED", OnPointChange);

		tableHockeyGameManager.SetCurrentState (TableHockeyGameManager.State.PLAY);
		tableHockeyGameManager.SetGameView ();
	}

	private void OnUserPlayEnded(SocketIOEvent evt) {
		JSONObject roomInfo = evt.data;
		Debug.Log ("Get the msg from server is: " + evt.data + " OnUserPlayEnded ");
		socket.Off ("MOVE", OnUSerMove);
		socket.Off ("BALL_OWNER_CHANGE", OnBallOwnerChange);
		socket.Off ("BALL_MOVE", OnBallMove);
		socket.Off ("PLAY_TIME_CHANGED", OnTimeChange);
		socket.Off ("PLAY_POINT_CHANGED", OnPointChange);


		tableHockeyGameManager.SetCurrentState (TableHockeyGameManager.State.END);
		ballOwner = "";
		ball.transform.position = new Vector3 (0f,0f,0f);
		ball.GetComponent<TableHockeyBall> ().SetMoveDirection (Vector3.zero);

		tableHockeyGameManager.SetCurrentJoinedRoom (roomInfo);
		tableHockeyGameManager.SetGameView ();
	}

	private void OnUSerMove(SocketIOEvent evt) {
		if (name != evt.data.GetField ("name").str) 
			opponentBar.transform.position  = -JsonToVector3 (evt.data.GetField ("position").str);
	}
		
	private void OnBallOwnerChange(SocketIOEvent evt) {
		Debug.Log ("ballOWner: "+evt.data.GetField ("name").str);
		ballOwner = evt.data.GetField ("name").str;
	}
		
	private void OnBallMove(SocketIOEvent evt) {
		if (!ballOwner.Equals (name)) {
			ball.GetComponent<TableHockeyBall> ().SetMoveDirection (-JsonToVector3 (evt.data.GetField ("moveDirection").str));
			ball.transform.position = -JsonToVector3 (evt.data.GetField ("position").str);
		}
	}

	private void OnTimeChange(SocketIOEvent evt) {
		string time = evt.data.GetField ("time").str;

		Debug.Log ("Get the msg from server is: " + time + "sec OnTimeChange ");

		if (!isBallOwner ())
			tableHockeyGameManager.SetPlayTime (int.Parse (time));
		tableHockeyGameManager.SetPlayTimeView ();	
	}

	private void OnPointChange(SocketIOEvent evt) {
		string sender = evt.data.GetField ("name").str;

		Debug.Log ("Get the msg from server is: " + sender + "sec OnTimeChange ");

		if (sender.Equals(name))
			tableHockeyGameManager.setOpponentPlayerPoint (tableHockeyGameManager.getOpponentPlayerPoint()+1);
		else
			tableHockeyGameManager.setPlayerPoint (tableHockeyGameManager.getPlayerPoint()+1);
		tableHockeyGameManager.SetPlayPointView ();	
	}
		
	public void SendBarMoveMSg() {
		Transform transform = playerBar.GetComponent<Transform> ();
		Dictionary<string, string> data = new Dictionary<string, string> ();

		data["name"] = name;
		data["position"] = transform.position.x + "," + transform.position.y + "," + transform.position.z;
		data ["title"] = tableHockeyGameManager.getCurrentJoinedRoom ().GetField ("title").str;
		socket.Emit ("MOVE", new JSONObject(data));
	}

	public void SendBallOwnerChangeMsg(){
		Dictionary<string, string> data = new Dictionary<string, string> ();
		this.ballOwner = name;// BallOwner 자신으로 수정..

		data["name"] = name;
		data ["title"] = tableHockeyGameManager.getCurrentJoinedRoom ().GetField ("title").str;
		socket.Emit ("BALL_OWNER_CHANGE", new JSONObject (data));
	}

	public void SendBallMoveMSg() {
		Transform transform = ball.GetComponent<Transform> ();
		Dictionary<string, string> data = new Dictionary<string, string> ();
		Vector3 ballMoveDirection = ball.GetComponent<TableHockeyBall> ().getMoveDirection ();
		data["moveDirection"] = ballMoveDirection.x + "," + ballMoveDirection.y + "," + ballMoveDirection.z;
		data["position"] = transform.position.x + "," + transform.position.y + "," + transform.position.z;
		data ["title"] = tableHockeyGameManager.getCurrentJoinedRoom ().GetField ("title").str;

		socket.Emit ("BALL_MOVE", new JSONObject(data));
	}

	/**
	 *	room related event hendler.
	 * */
	private void OnCreatedRoom(SocketIOEvent evt) {
		JSONObject currentServerInfo = evt.data.GetField("currentServerInfo");
		JSONObject roomInfo = evt.data.GetField("roomInfo");

		Debug.Log ("Get the msg from server is: " +currentServerInfo.GetField("clientsLength").n + " OnCreatedRoom");
		Debug.Log ("Get the msg from server is: " + currentServerInfo.GetField("rooms") + " OnCreatedRoom");
		Debug.Log ("Get the msg from server is: " + roomInfo.GetField("title") + " OnCreatedRoom");
		Debug.Log ("Get the msg from server is: " + roomInfo.GetField("master") + " OnCreatedRoom");


		tableHockeyGameManager.SetCurrentServerInfo (currentServerInfo);

		if (roomInfo.GetField ("master").str.Equals (name)) {
			tableHockeyGameManager.SetCurrentState (TableHockeyGameManager.State.READY);
			tableHockeyGameManager.SetCurrentJoinedRoom (roomInfo);
		}

		Debug.Log (tableHockeyGameManager.getCurrentState ().ToString ());
		tableHockeyGameManager.SetGameView ();
	}

	private void OnDestroyRoom(SocketIOEvent evt) {
		JSONObject roomInfo = evt.data;
		Debug.Log ("Get the msg from server is: " +roomInfo.GetField("title")+ " OnDestroyRoom");
		SendLeaveRoomMSg (roomInfo.GetField ("title").str);
	}

	private void OnJoinedRoom(SocketIOEvent evt) {
		JSONObject roomInfo = evt.data;
		Debug.Log ("Get the msg from server is: " + evt.data.GetField("title") + " OnJoinedRoom");
		Debug.Log ("Get the msg from server is: " + evt.data.GetField("master") + " OnJoinedRoom");
		Debug.Log ("Get the msg from server is: " + evt.data.GetField("attendants") + " OnJoinedRoom");

		//tableHockeyGameManager.SetCurrentServerInfo (currentServerInfo);
		tableHockeyGameManager.SetCurrentJoinedRoom (roomInfo);

		tableHockeyGameManager.SetCurrentState (TableHockeyGameManager.State.READY);
		tableHockeyGameManager.SetGameView ();

	}

	private void OnLeftRoom(SocketIOEvent evt) {
		JSONObject currentServerInfo = evt.data.GetField("currentServerInfo");
		JSONObject roomInfo = evt.data.GetField ("roomInfo");
		List<JSONObject> currentServer_rooms = currentServerInfo.GetField ("rooms").list;

		Debug.Log ("Get the msg from server is: " +currentServerInfo.GetField("clientsLength").n + " OnLeftRoom");
		Debug.Log ("Get the msg from server is: " + currentServerInfo.GetField("rooms") + " OnLeftRoom");

		tableHockeyGameManager.SetCurrentServerInfo (currentServerInfo);

		if (tableHockeyGameManager.getCurrentState ().Equals (TableHockeyGameManager.State.WAIT)) {
			tableHockeyGameManager.SetGameView ();
			return;
		}
			
		//currentJoinedroom is destroyed
		if (roomInfo == null) {
			JSONObject joinedRoom = currentServer_rooms.Find ((v) => {
				return v.GetField("title").str == tableHockeyGameManager.getCurrentJoinedRoom().GetField("title").str;
			});

			if (joinedRoom == null) {
				tableHockeyGameManager.SetCurrentState (TableHockeyGameManager.State.WAIT);
				tableHockeyGameManager.SetCurrentJoinedRoom (null);
				tableHockeyGameManager.SetGameView ();	
				return;
			}
			return;
		}
			
		if(tableHockeyGameManager.getCurrentJoinedRoom().GetField("title").str.Equals(roomInfo.GetField("title").str)) {
			JSONObject attendant = roomInfo.GetField ("attendants").list.Find ((v) => {
				return v.GetField("name").str == name;
			});

			if (attendant == null) {
				tableHockeyGameManager.SetCurrentState (TableHockeyGameManager.State.WAIT);
				tableHockeyGameManager.SetCurrentJoinedRoom (null);
			} else 
				tableHockeyGameManager.SetCurrentJoinedRoom (roomInfo);
			
			tableHockeyGameManager.SetGameView ();
		}
	}
		
	public void SendCreateRoomMSg() {
		Dictionary<string, string> data = new Dictionary<string, string> ();
		data ["title"] = "Room of "+name;
		data ["master"] = name;
		socket.Emit ("CREATE_ROOM", new JSONObject (data));
	}

	public void SendJoinRoomMSg(string roomTitle) {
		Dictionary<string, string> data = new Dictionary<string, string> ();
		data ["title"] = roomTitle;
		data ["attendant"] = name;
		socket.Emit ("JOIN_ROOM", new JSONObject (data));
	}

	public void SendLeaveRoomMSg(string roomTitle) {
		Dictionary<string, string> data = new Dictionary<string, string> ();
		data ["title"] = roomTitle;
		data ["attendant"] = name;
		socket.Emit ("LEAVE_ROOM", new JSONObject (data));
	}

	public void SendPlayReadyMSg() {
		JSONObject currentJoinedRoom = tableHockeyGameManager.getCurrentJoinedRoom ();
		Dictionary<string, string> data = new Dictionary<string, string> ();

		data ["title"] = currentJoinedRoom.GetField("title").str;
		data ["attendant"] = name;
		socket.Emit ("PLAY_READY", new JSONObject (data));
	}

	public void SendPlayReadyCancelMSg() {
		JSONObject currentJoinedRoom = tableHockeyGameManager.getCurrentJoinedRoom ();
		Dictionary<string, string> data = new Dictionary<string, string> ();

		data ["title"] = currentJoinedRoom.GetField("title").str;
		data ["attendant"] = name;
		socket.Emit ("PLAY_READY_CANCEL", new JSONObject (data));
	}

	public void SendPlayTimeMSg(float time) {
		JSONObject currentJoinedRoom = tableHockeyGameManager.getCurrentJoinedRoom ();
		Dictionary<string, string> data = new Dictionary<string, string> ();

		data ["title"] = currentJoinedRoom.GetField("title").str;
		data ["time"] = time.ToString();
		socket.Emit ("PLAY_TIME_CHANGE", new JSONObject (data));
	}

	public void SendPointInfoMSg() {
		JSONObject currentJoinedRoom = tableHockeyGameManager.getCurrentJoinedRoom ();
		Dictionary<string, string> data = new Dictionary<string, string> ();

		data ["title"] = currentJoinedRoom.GetField("title").str;
		data ["name"] = name;

		socket.Emit ("PLAY_POINT_CHANGE", new JSONObject (data));
	}

	public void SendPlayEndMSg() {
		JSONObject currentJoinedRoom = tableHockeyGameManager.getCurrentJoinedRoom ();
		Dictionary<string, string> data = new Dictionary<string, string> ();

		data ["title"] = currentJoinedRoom.GetField("title").str;
		data ["name"] = name;

		socket.Emit ("PLAY_END", new JSONObject (data));
	}

	public void SendPlayExitMSg() {
		Dictionary<string, string> data = new Dictionary<string, string> ();
		data ["name"] = name;

		socket.Emit ("disconnect", new JSONObject (data));
	}
		
	Vector3 JsonToVector3(string target) {
		Vector3 newVector;
		string[] newString = Regex.Split (target, ",");

		newVector = new Vector3 (float.Parse (newString [0]), float.Parse (newString [1]), float.Parse (newString [2]));

		return newVector;
	}

	public bool isBallOwner() {
		return ballOwner.Equals (name);
	}

	public string getOtehrPlayerName() {
		JSONObject currentJoinedRoom = tableHockeyGameManager.getCurrentJoinedRoom ();
		JSONObject otherPlayer;
		otherPlayer = currentJoinedRoom.GetField ("attendants").list.Find ((v) => {
			return !v.GetField("name").str.Equals(name);
		});
		return otherPlayer.GetField ("name").str;
	}
}

