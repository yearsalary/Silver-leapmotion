using UnityEngine;
using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using LitJson;

public static class PlayRecordDataServiceManager {

	static public bool SendPlayRecordData(PlayRecordData data) {
		bool isSuccess = false;
		var webAddr = "http://117.17.158.66:8080/vrain/client/record";

		WWWForm form = new WWWForm();
		form.AddField("result", Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonUtility.ToJson(data, true))));
		WWW www = new WWW(webAddr, form);
		WaitForRequest(www);

		if (www.error == null) {
			UnityEngine.Debug.Log ("msg send success");
			isSuccess = true;
		} else {
			UnityEngine.Debug.Log ("msg send fail");
			isSuccess = false;
		}
		return isSuccess;

	}

	static private IEnumerator WaitForRequest(WWW www) {
		yield return www;
	}


}
