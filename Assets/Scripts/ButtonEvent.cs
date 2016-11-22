using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonEvent : MonoBehaviour {
    public string gameName;

    public void onBackButton()
    {
        SceneManager.LoadScene("TraineeSelectScene");
    }

    public void gameStartButton()
    {
        string gameScene = gameName + "Scene";
        SceneManager.LoadScene(gameScene);
    }
}
