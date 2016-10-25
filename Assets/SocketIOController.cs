using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SocketIO;

public class SocketIOController : MonoBehaviour {

	public SocketIOComponent socket;

	// Use this for initialization
	void Start () {
		StartCoroutine (ConnectToServer ());
		socket.On ("USER_CONNECTED", OnUserConnected);
		socket.On ("PLAY", OnUserPlay);
	}

	IEnumerator ConnectToServer() {
		yield return new WaitForSeconds (0.5f);
		socket.Emit ("USER_CONNECT");
		yield return new WaitForSeconds (1f);

		Dictionary<string, string> data = new Dictionary<string, string> ();
		data ["name"] = "tester1";
		Vector3 position = new Vector3 (0, 0, 0);
		data ["position"] = position.x + "," + position.y + "," + position.z;
		socket.Emit ("PLAY", new JSONObject (data));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private void OnUserConnected(SocketIOEvent evt) {
		Debug.Log ("Get the msg from server is: " + evt.data +" OnUserConnected ");
	}

	private void OnUserPlay(SocketIOEvent evt) {
		Debug.Log ("Get the msg from server is: " + evt.data + " OnUserPlay ");
	}

	Vector3 JsonToVector3(string target) {
		Vector3 newVector;
		string[] newString = Regex.Split (target, ",");
		newVector = new Vector3 (float.Parse (newString [0]), float.Parse (newString [1]), float.Parse (newString [2]));

		return newVector;
	}

}

