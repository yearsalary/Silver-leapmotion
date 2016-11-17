using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net;
using System.Text;
using System;
using LitJson;
using System.Globalization;


class LoginData
{
    public string id;
    public string pw;
    public List<TraineeModel> traineeList = new List<TraineeModel>();
}

public class LoginManager : MonoBehaviour {

    public WWW www;
	public GameObject ID_inputText;
	public GameObject PW_inputText;
    public AssistantModel assistantmodel;
    public string cookie = "";

    string id;
	string pw;

    public void Login() {
        //텍스트값 얻어오기
        id = ID_inputText.GetComponent<InputField> ().text;
		pw = PW_inputText.GetComponent<InputField> ().text;
        
        //서버에게 Json을 넘겨주고 쿠키값 받아오는거
        var data = new LoginData();
        var webAddr = "http://117.17.158.201:8080/vrain/client/login";
        //var sessionID = "";
        
        data.id = id;
        data.pw = pw;

        WWWForm form = new WWWForm();

        form.AddField("LoginInfo", JsonUtility.ToJson(data, true));
        //form.headers.Add("Cookie", cookie);

        WWW www = new WWW(webAddr, form);
        StartCoroutine(WaitForRequest(www));

        form.headers.Add("Cookie", cookie);
    }

    private IEnumerator WaitForRequest(WWW www)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            List<TraineeModel> traineeList = new List<TraineeModel>();

            //디코딩 해주는 부분
            UTF8Encoding encoder = new UTF8Encoding();
            Decoder utf8Decode = encoder.GetDecoder();

            byte[] todecode_byte = Convert.FromBase64String(www.text);
            char[] decoded_char = new char[utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length)];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            
            //string을 제이슨으로 바꿔서 제이슨을 트레이니리스트에 넣어주는 부분
            JsonData json = JsonMapper.ToObject(result);

            for (int i = 0; i < json.Count; i++)
            {
                JsonData item = json[i];
                string id = item["ID"].ToString();
                string name = item["NAME"].ToString();
                traineeList.Add(new TraineeModel(id, name));
            }
            string assID = json[0]["assID"].ToString(); // 사용자 번호이면 json[0]["ass_usrID"]로 변경
            string assName = json[0]["assName"].ToString();

            cookie = "assID=" + assID + "&assName=" + assName;

            DateTime now = DateTime.Now;
            Debug.Log(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            GameStatusModel.assistant = new AssistantModel(assID, assName, traineeList);

            SceneManager.LoadScene("TraineeSelectScene");
        }
        else
        {
            //로그인 실패
            Debug.Log("WWW Error: " + www.error);
        }
    }
}