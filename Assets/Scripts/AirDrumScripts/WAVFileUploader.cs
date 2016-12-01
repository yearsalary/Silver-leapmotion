using UnityEngine;
using System.Collections;

public class WAVFileUploader : MonoBehaviour {
	public GameObject cam;
	WAVRecorder rc;
	public string uploadURL = "http://localhost:8888/spring_test/wavfileUpload";
	public string wavFilePath = "";
	public string fileName = "";


	void Start() {
		rc = cam.GetComponent<WAVRecorder> ();
		wavFilePath = Application.persistentDataPath + "\\newRec.wav";
	}
	// Update is called once per frame
	void Update () {
		Debug.Log (rc.recState);
		if (rc.recState == "idle")
			return;
		if (rc.recState == "recEnd")
			StartCoroutine(WavfileUplaod());
	}

	private IEnumerator WavfileUplaod() {
		rc.recState = "recUpload";
		WWW wavFile = new WWW ("file:///"+wavFilePath);
		yield return wavFile;
		WWWForm postForm = new WWWForm ();
		postForm.AddBinaryData("WAVfile",wavFile.bytes,"test.wav");
		WWW upload = new WWW (uploadURL, postForm);
		yield return upload;

		if (upload.error == null) {
			Debug.Log (upload.text);
		} else {
			Debug.Log (upload.error);
		}

		rc.recState = "idle";
	}
}
