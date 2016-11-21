using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGameManager : MonoBehaviour{
	public Dropdown dropDwon;

	public void StartGame() {
		string selectedGame = dropDwon.captionText.text;
		SceneManager.LoadScene(selectedGame+"Scene");
	}

	public void GotoBackScene() {
		SceneManager.LoadScene ("TraineeSelectScene");
	}
}
