using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class DropBoxGameManager : MonoBehaviour
{
    public GameObject cubeFactory;
    public GameObject cube;

    public int cubeCount;
    public int score;
    public int totalScore;
    bool isStopGame;
    public float time;
    string contentsName;
    string cookie;
    string startTime;
    string endTime;

    public Canvas dialogueCanvas;
    public Canvas gamePlayUI;
   
    public Text levelText;
    public Text timeText;
    public Text pointText;
    public Text scoreMessage;
    public Text dialogMessage;
    public List<GameObject> dropBoxList;

    // Use this for initialization
    void Start()
    {
        startTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        contentsName = "드랍 박스 게임";
        cubeCount = 1;
        dialogMessage.text = "시작\n 큐브를 같은 색깔 박스에 넣어주시기 바랍니다.\n";
        dialogueCanvas.enabled = true;
        gamePlayUI.enabled = false;
        isStopGame = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStopGame)
            return;

        CheckTime();
        DrawGameInfo();
        CheckCubeCount();
    }

    void InitGame()
    {
        score = 0;
        dropBoxList = new List<GameObject>();
        time = 120f;
        MakeCube(cubeCount);
        levelText.text = "Level: " + cubeCount;
        pointText.text = "Score : " + score;
        isStopGame = false;
    }

    void CheckTime()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = 0;
            FinishGame(false);
        }
    }

    void DrawGameInfo()
    {
        timeText.text = "Time : " + (int)time;
    }

    void CheckCubeCount()
    {
        for (int i = 0; i < dropBoxList.Count; i++)
        {
            if (dropBoxList[i] == null)
            {
                dropBoxList.RemoveAt(i);
                score += 10;
                totalScore += score;
                pointText.text = "Score : "+ score;
            }
        }

        if(dropBoxList.Count == 0)
        {
            FinishGame(true);
        }
        
    }

    void FinishGame(bool isSucceed)
    {
        isStopGame = true;
        foreach (GameObject cube in dropBoxList)
            Destroy(cube);
        dropBoxList.Clear();

        dialogMessage.text = "";
        scoreMessage.text = "";

        if (isSucceed)
        {
            GameObject.Find("DialogueCanvas").GetComponentInChildren<ChangeImage>().changeImage(isSucceed);
            scoreMessage.text = score.ToString();
            cubeCount++;
        }
        else
        { 
            GameObject.Find("DialogueCanvas").GetComponentInChildren<ChangeImage>().changeImage(isSucceed);
            scoreMessage.text = score.ToString();
        }

        dialogueCanvas.enabled = true;
        gamePlayUI.enabled = false;
    }

    void MakeCube(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject cubeClone = (GameObject)Instantiate(cube, cubeFactory.transform.position, cube.transform.rotation);
            dropBoxList.Add(cubeClone);

            switch (i % 4)
            {
                case 0 :
                    cubeClone.GetComponent<Renderer>().material.color = Color.yellow;
                    break;
                case 1 :
                    cubeClone.GetComponent<Renderer>().material.color = Color.red;
                    break;
                case 2 :
                    cubeClone.GetComponent<Renderer>().material.color = Color.green;
                    break;
                case 3:
                    cubeClone.GetComponent<Renderer>().material.color = Color.blue;
                    break;
            }
                
        }
    }

    public void OnPlayButton()
    {
        dialogueCanvas.enabled = false;
        gamePlayUI.enabled = true;
        InitGame();
    }

    public void OnPlayEndButton()
    {
        //게임 기록 데이터 정리 및 전송..
        endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        string cubeCount = this.cubeCount.ToString();
        string totalScore = this.totalScore.ToString();

        if (score != 0)
        {
            PlayRecordData data = new PlayRecordData(GameStatusModel.trainee.getId(), GameStatusModel.assistant.id,
                                                this.contentsName, cubeCount, totalScore, this.startTime, this.endTime);

            PlayRecordDataServiceManager.SendPlayRecordData(data);
        }
        
        SceneManager.LoadScene("MainMenuScene2");
    }

    public void OnGameStopButton() {
        FinishGame(false);
    }

}
