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
	private string ballOwner = "";
	public string name;


	// Use this for initialization
	void Start () {
		StartCoroutine (ConnectToServer ());
		socket.On ("USER_CONNECTED", OnUserConnected);
		socket.On ("USER_DISCONNECTED", OnUserDisConnected);
		//room related...
		socket.On ("CREATED_ROOM", OnCreatedRoom);
		socket.On ("DESTROY_ROOM", OnDestroyRoom);
		socket.On ("JOINED_ROOM", OnJoinedRoom);
		socket.On ("LEFT_ROOM", OnLeftRoom);
		//socket.On ("PLAY", OnUserPlay);
		//socket.On ("MOVE", OnUSerMove);
		//socket.On ("BALL_OWNER_CHANGE", OnBallOwnerChange);
		//socket.On ("BALL_MOVE", OnBallMove);
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

		if (!gameManager.GetComponent<TableHockeyGameManager> ().currentState.Equals (TableHockeyGameManager.State.PLAY))
			return;
		
		SendBarMoveMSg();
		if (ballOwner.Equals (name))
			SendBallMoveMSg ();

		if ((Vector3.Distance (playerBar.transform.position, ball.transform.position) <
			Vector3.Distance (opponentBar.transform.position, ball.transform.position)) && !isBallOwner())
			SendBallOwnerChangeMsg ();

	}
	
	private void OnUserConnected(SocketIOEvent evt) {
		Debug.Log ("Get the msg from server is: " + evt.data.GetField("clientsLength") +" OnUserConnected ");
		Debug.Log ("Get the msg from server is: " + evt.data.GetField("rooms") +" OnUserConnected ");
		float connectedUserCount = evt.data.GetField ("clientsLength").n;
		List<JSONObject> rooms = evt.data.GetField("rooms").list;

		gameManager.GetComponent<TableHockeyGameManager> ().SetServerInfo (connectedUserCount, rooms);
	}

	private void OnUserDisConnected(SocketIOEvent evt) {
		Debug.Log ("Get the msg from server is: " + evt.data.GetField("clientsLength") +" OnUserConnected ");
		Debug.Log ("Get the msg from server is: " + evt.data.GetField("rooms") +" OnUserConnected ");
		float connectedUserCount = evt.data.GetField ("clientsLength").n;
		List<JSONObject> rooms = evt.data.GetField("rooms").list;

		gameManager.GetComponent<TableHockeyGameManager> ().SetServerInfo (connectedUserCount, rooms);

	}

	private void OnUserPlay(SocketIOEvent evt) {
		Debug.Log ("Get the msg from server is: " + evt.data + " OnUserPlay ");
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
		Debug.Log (evt.data.GetField ("moveDirection").str);
		ball.GetComponent<TableHockeyBall> ().SetMoveDirection (-JsonToVector3(evt.data.GetField ("moveDirection").str));
		ball.transform.position  = - JsonToVector3 (evt.data.GetField ("position").str);
	}

	private void OnCreatedRoom(SocketIOEvent evt) {
		JSONObject currentServerInfo = evt.data.GetField("currentServerInfo");
		JSONObject roomInfo = evt.data.GetField("roomInfo");

		Debug.Log ("Get the msg from server is: " +currentServerInfo.GetField("clientsLength").n + " OnCreatedRoom");
		Debug.Log ("Get the msg from server is: " + currentServerInfo.GetField("rooms") + " OnCreatedRoom");

	
		if (roomInfo.GetField ("master").str.Equals (name))
			gameManager.GetComponent<TableHockeyGameManager> ().ReadyGame (roomInfo);
		else 
			gameManager.GetComponent<TableHockeyGameManager> ().SetServerInfo (currentServerInfo.GetField("clientsLength").n ,currentServerInfo.GetField("rooms").list);
	}

	private void OnDestroyRoom(SocketIOEvent evt) {
		JSONObject roomInfo = evt.data;
	
		Debug.Log ("Get the msg from server is: " +roomInfo.GetField("title")+ " OnDestroyRoom");
		SendLeaveRoomMSg (roomInfo.GetField ("title").str);

	}

	private void OnJoinedRoom(SocketIOEvent evt) {
		Debug.Log ("Get the msg from server is: " + evt.data.GetField("title") + " OnJoinedRoom");
		Debug.Log ("Get the msg from server is: " + evt.data.GetField("master") + " OnJoinedRoom");
		Debug.Log ("Get the msg from server is: " + evt.data.GetField("attendants") + " OnJoinedRoom");
		gameManager.GetComponent<TableHockeyGameManager> ().ReadyGame (evt.data);
	}

	private void OnLeftRoom(SocketIOEvent evt) {
		JSONObject currentServerInfo = evt.data.GetField("currentServerInfo");
		JSONObject roomInfo = evt.data.GetField("roomInfo");

		Debug.Log ("Get the msg from server is: " +currentServerInfo.GetField("clientsLength").n + " OnCreatedRoom");
		Debug.Log ("Get the msg from server is: " + currentServerInfo.GetField("rooms") + " OnCreatedRoom");

		JSONObject attendant = roomInfo.GetField ("attendants").list.Find ((v) => {
			return v.str == name;
		});

		if(attendant==null)
			gameManager.GetComponent<TableHockeyGameManager> ().WaitGame (currentServerInfo);
		else
			gameManager.GetComponent<TableHockeyGameManager> ().ReadyGame (roomInfo);
	}
		
	public void SendBarMoveMSg() {
		Transform transform = playerBar.GetComponent<Transform> ();
		Dictionary<string, string> data = new Dictionary<string, string> ();

		data["name"] = name;
		data["position"] = transform.position.x + "," + transform.position.y + "," + transform.position.z;

		socket.Emit ("MOVE", new JSONObject(data));
	}

	public void SendBallOwnerChangeMsg(){
		Dictionary<string, string> data = new Dictionary<string, string> ();
		this.ballOwner = name;// BallOwner 자신으로 수정..

		data["name"] = name;
		socket.Emit ("BALL_OWNER_CHANGE", new JSONObject (data));
	}

	public void SendBallMoveMSg() {
		Transform transform = ball.GetComponent<Transform> ();
		Dictionary<string, string> data = new Dictionary<string, string> ();
		Vector3 ballMoveDirection = ball.GetComponent<TableHockeyBall> ().getMoveDirection ();
		data["moveDirection"] = ballMoveDirection.x + "," + ballMoveDirection.y + "," + ballMoveDirection.z;
		data["position"] = transform.position.x + "," + transform.position.y + "," + transform.position.z;
		socket.Emit ("BALL_MOVE", new JSONObject(data));
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
		
	Vector3 JsonToVector3(string target) {
		Vector3 newVector;
		string[] newString = Regex.Split (target, ",");

		newVector = new Vector3 (float.Parse (newString [0]), float.Parse (newString [1]), float.Parse (newString [2]));

		return newVector;
	}

	public bool isBallOwner() {
		return this.ballOwner.Equals(name);
	}

	public void SetBallOwnerByMe() {
		this.ballOwner = name;
	}

		
}

