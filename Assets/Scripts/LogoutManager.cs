using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LogoutManager : MonoBehaviour{

	public void Logout() {
		GameStatusModel.assistant = null;
		GameStatusModel.trainee = null;
		SceneManager.LoadScene ("TitleScene");
	}
}
