using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SocketIO;

public class SocketIOController : MonoBehaviour {

	public SocketIOComponent socket;
	public GameObject playerBar;
	public GameObject opponentBar;
	public GameObject ball;
	private string ballOwner = "";
	string name = "A";


	// Use this for initialization
	void Start () {
		StartCoroutine (ConnectToServer ());
		socket.On ("USER_CONNECTED", OnUserConnected);
		socket.On ("PLAY", OnUserPlay);
		socket.On ("MOVE", OnUSerMove);
		socket.On ("BALL_COLLISON", OnBallCollison);
		socket.On ("BALL_MOVE", OnBallMove);
	}

	IEnumerator ConnectToServer() {
		yield return new WaitForSeconds (0.5f);
		socket.Emit ("USER_CONNECT");
		yield return new WaitForSeconds (1f);

		Dictionary<string, string> data = new Dictionary<string, string> ();
		data ["name"] = name;
		Vector3 position = new Vector3 (0, 0, 0);
		data ["position"] = position.x + "," + position.y + "," + position.z;
		socket.Emit ("PLAY", new JSONObject (data));
	}

	// Update is called once per frame
	void Update () {
		SendBarMoveMSg();
		if (ballOwner.Equals (name))
			SendBallMoveMSg ();


	}
	
	private void OnUserConnected(SocketIOEvent evt) {
		Debug.Log ("Get the msg from server is: " + evt.data +" OnUserConnected ");
	}

	private void OnUserPlay(SocketIOEvent evt) {
		Debug.Log ("Get the msg from server is: " + evt.data + " OnUserPlay ");
	}

	private void OnUSerMove(SocketIOEvent evt) {
		if (name != evt.data.GetField ("name").str)
			opponentBar.transform.position  = JsonToVector3 (evt.data.GetField ("position").str);
		
	}
		
	private void OnBallCollison(SocketIOEvent evt) {
		Debug.Log ("ballOWner: "+evt.data.GetField ("name").str);
		ballOwner = evt.data.GetField ("name").str;
	}
		
	private void OnBallMove(SocketIOEvent evt) {
		Debug.Log ("ballMove: "+evt.data.GetField ("name").str);
		ball.transform.position  = JsonToVector3 (evt.data.GetField ("position").str);
	}




	public void SendBarMoveMSg() {
		Transform transform = playerBar.GetComponent<Transform> ();
		Dictionary<string, string> data = new Dictionary<string, string> ();

		data["name"] = name;
		data["position"] = transform.position.x + "," + transform.position.y + "," + transform.position.z;
		socket.Emit ("MOVE", new JSONObject(data));
	}

	public void SendBallCollisonMsg(){
		Dictionary<string, string> data = new Dictionary<string, string> ();
		data["name"] = name;
		socket.Emit ("BALL_COLLISON", new JSONObject (data));
	}

	public void SendBallMoveMSg() {
		Transform transform = ball.GetComponent<Transform> ();
		Dictionary<string, string> data = new Dictionary<string, string> ();

		this.ballOwner = name;// BallOwner 자신으로 수정..

		data["name"] = name;
		data["position"] = transform.position.x + "," + transform.position.y + "," + transform.position.z;
		socket.Emit ("BALL_MOVE", new JSONObject(data));
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

