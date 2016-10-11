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

		AssistantModel.id = id;
		AssistantModel.name = "아무개";
		List<TraineeModel> traineeList= new List<TraineeModel> ();
		traineeList.Add(new TraineeModel("김복순"));
		traineeList.Add(new TraineeModel("이말자"));
		traineeList.Add(new TraineeModel("박순자"));
		AssistantModel.traineeList = traineeList;
		SceneManager.LoadScene(1);


		//로그인 실패

	}
}
