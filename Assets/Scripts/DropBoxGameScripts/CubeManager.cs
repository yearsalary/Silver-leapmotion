using UnityEngine;
using System.Collections;

public class CubeManager : MonoBehaviour {
    public Color cubeColor;
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
                cubeColor = Color.red;
                break;
            case 1 :
                cubeColor = Color.blue;
                break;
            case 2 :
                cubeColor = Color.green;
                break;
        }

        cube = GameObject.FindWithTag("CubeSet");

        if (cube.tag == "CubeSet")
        {
            GetComponent<Renderer>().material.color = cubeColor;
        }
        
    }
}
