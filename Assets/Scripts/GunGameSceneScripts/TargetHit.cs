using UnityEngine;
using System.Collections;

public class TargetHit : MonoBehaviour {
    int num = 0;
    string color;
    //Transform[] target;
    //GameObject[] target;

    // Use this for initialization
    void Start () {
	
	}

    public void setColor(string c)
    {
        color = c;
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "BULLET")
        {
            //TargetHitScore();
            Debug.Log("aa : " + color);
            Destroy(coll.gameObject);
            Destroy(gameObject);
            
        }
    }

    //과녁 어느쪽에 맞추었는지 점수 달라지게 하는 코드
    void TargetHitScore()
    {
        switch (color)
        {
            case "Green" :
                color = "Green";
                break;
            case "Yellow":
                color = "Yellow";
                break;
            case "Blue":
                color = "Blue";
                break;
            case "Black":
                color = "Black";
                break;
        }
        //target = GameObject.Find("Target").GetComponentInChildren();
        /*target = GameObject.Find("Target").GetComponentsInChildren<Transform>();

        foreach (Transform child in target)
        {
            if (child.name.Contains("Red"))
            {
                Debug.Log("Red");
            }
            else if (child.name.Contains("Yellow"))
            {
                Debug.Log("Yellow");
            }
            else if (child.name.Contains("Blue"))
            {
                Debug.Log("Blue");
            }
            else if (child.name.Contains("Black"))
            {
                Debug.Log("Black");
            }
        }*/
        Debug.Log("aa : " + color);
    }

    /*void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "BULLET")
        {
            Destroy(other.gameObject);
        }
    }*/

    // Update is called once per frame
    void Update () {
        
    }
}
