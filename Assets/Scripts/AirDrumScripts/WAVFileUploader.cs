using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WAVFileUploader : MonoBehaviour {
	public GameObject cam;
	WAVRecorder rc;
	public string uploadURL = "http://localhost:8888/spring_test/wavfileUpload";
	public string wavFilePath = "";
	public string fileName = "";

	public Button recStartBtn;
	public Button recStopBtn;
	public Button gameStopBtn;
	public Canvas recCanvas;
	public Canvas dialougeCanvas;
	public Text recTimeText;
	public Text recUploadText;
	private float recTime;

	int min = 0;
	int sec = 0;
	int milSec = 0;

	void Start() {
		rc = cam.GetComponent<WAVRecorder> ();
		wavFilePath = Application.persistentDataPath + "\\newRec.wav";
		recTime = 0;
		InitGame ();
	}
	// Update is called once per frame
	void Update () {
		//Debug.Log (rc.recState);
		if (rc.recState == "idle")
			return;

		if (rc.recState == "recStart") {
			CheckRecTime ();
			recUploadText.text = "녹음중...";
			gameStopBtn.interactable = false;
		}

		if (rc.recState == "recEnd") {
			StartCoroutine (WavfileUplaod ());
			min = sec = milSec =  0;
			recTime = 0f;
			recUploadText.text = "녹음완료...업로드중...";
		}
	}

	private IEnumerator WavfileUplaod() {
		rc.recState = "recUpload";
		recStartBtn.interactable = false;
		recStopBtn.interactable = false;

		WWW wavFile = new WWW ("file:///"+wavFilePath);
		yield return wavFile;
		WWWForm postForm = new WWWForm ();
		postForm.AddBinaryData("WAVfile",wavFile.bytes,"test.wav");
		WWW upload = new WWW (uploadURL, postForm);
		yield return upload;

		recStartBtn.interactable = true;
		recStopBtn.interactable = false;
		gameStopBtn.interactable = true;

		if (upload.error == null) {
			Debug.Log (upload.text);
		} else {
			Debug.Log (upload.error);
		}
		recUploadText.text = "업로드 완료";
		rc.recState = "idle";
	}

	private void CheckRecTime() {
		recTime += Time.deltaTime;
		min = ((int)recTime / 60);
		sec = ((int)recTime % 60);
		milSec = (int)((recTime * 100) % 100);
		recTimeText.text = string.Format ("{0,2:D2}:{1,2:D2}:{2,2:D2}", min, sec, milSec);
	}

	public void StartGame() {
		this.dialougeCanvas.enabled = false;
		this.recCanvas.enabled = true;
	}

	public void InitGame() {
		this.dialougeCanvas.enabled = true;
		this.recCanvas.enabled = false;
	}

	public void EndGame() {
		SceneManager.LoadScene ("MainMenuScene2");
	}
}
