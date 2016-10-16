using UnityEngine;
using System.Collections;

public class CubeManager : MonoBehaviour {
    Color boxColor;
    GameObject cube;

    public enum COLOR
    {
        red = 0, blue = 1, green = 2
    }

	void Awake()
    {
        int colorNumber = Random.Range(0, 3);

        switch (colorNumber)
        {
            case 0 :
                boxColor = Color.red;
                break;
            case 1 :
                boxColor = Color.blue;
                break;
            case 2 :
                boxColor = Color.green;
                break;
        }

        cube = GameObject.FindWithTag("CubeSet");

        if (cube.tag == "CubeSet")
        {
            GetComponent<Renderer>().material.color = boxColor;
        }
        
    }
}
