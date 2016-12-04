using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using LitJson;
using System.Text;

public class WAVFileUploader : MonoBehaviour {
	public GameObject cam;
	WAVRecorder rc;
	public string uploadURL;
	public string wavFilePath = "";

	public Button recStartBtn;
	public Button recStopBtn;
	public Button gameStopBtn;
	public Canvas recCanvas;
	public Canvas dialougeCanvas;
	public Canvas titleCanvas;

	public Text recTimeText;
	public Text recUploadText;
	public InputField titleInputField;
	public Button checkBtn;
	private float recTime;

	private string contentsName;
	private string startTime;
	private string endTime;
	public string originFileName;
	public string title;

	int min = 0;
	int sec = 0;
	int milSec = 0;

	void Start() {
		rc = cam.GetComponent<WAVRecorder> ();
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
		wavFilePath = Application.persistentDataPath + "\\"+originFileName+".wav";
		endTime = DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss");

		WWW wavFile = new WWW ("file:///"+wavFilePath);
		yield return wavFile;
		Debug.Log (wavFile.bytes.Length);
		WWWForm postForm = new WWWForm ();

		PlayRecordData playData = new PlayRecordData (GameStatusModel.trainee.getId(), GameStatusModel.assistant.id, contentsName,"0", "0", startTime, endTime);
		postForm.AddField ("result", Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonUtility.ToJson(playData, true))));
		postForm.AddField ("title", Convert.ToBase64String (Encoding.UTF8.GetBytes (title)));
		postForm.AddBinaryData("wav",wavFile.bytes, originFileName+".wav" );

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
		this.titleCanvas.enabled = false;
		this.contentsName = "에어 드럼";
	}

	public void EndGame() {
		SceneManager.LoadScene ("MainMenuScene2");
	}

	public void OnSaveBtn() {
		titleInputField.text = "";
		titleCanvas.enabled = true;
		checkBtn.interactable = false;
	}

	public void CheckTitleInput() {
		if (titleInputField.text.Length > 0)
			checkBtn.interactable = true;
		else
			checkBtn.interactable = false;
	}

	public void OnRec() {
		titleCanvas.enabled = false;
		gameStopBtn.interactable = false;

		DateTime startDateTime = DateTime.Now;
		startTime = startDateTime.ToString ("yyyy-MM-dd HH:mm:ss");
		originFileName = GameStatusModel.trainee.getId() +"_"+ startDateTime.ToString ("yyyyMMddHHmmss"); 
		title = titleInputField.text;
		rc.fileName = originFileName + ".wav";
		rc.OnRecordStartBtn ();
	}

}
