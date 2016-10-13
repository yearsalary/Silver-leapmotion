using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class LoginManager : MonoBehaviour {

	public GameObject ID_inputText;
	public GameObject PW_inputText;
	string id;
	string pw;

	public void Login() {
		id = ID_inputText.GetComponent<InputField> ().text;
		pw = PW_inputText.GetComponent<InputField> ().text;
		Debug.Log ("id:"+id+" pw:"+pw);

		//로그인 처리


		//로그인 성공

		List<TraineeModel> traineeList= new List<TraineeModel> ();
		traineeList.Add(new TraineeModel("01","김복순"));
		traineeList.Add(new TraineeModel("02","이말자"));
		traineeList.Add(new TraineeModel("03","박순자"));

		GameStatusModel.assistant = new AssistantModel (id, "아무개", traineeList);
		SceneManager.LoadScene("TraineeSelectScene");


		//로그인 실패

	}
}
