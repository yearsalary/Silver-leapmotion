using UnityEngine;
using System.Collections;

public class DropBoxGameManager : MonoBehaviour
{
    public GameObject cubeFactory;
    public GameObject cube;

    // Use this for initialization
    void Start()
    {
        MakeBox(6);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void MakeBox(int count)
    {
        for (int i = 0; i < count; i++)
        {
            cube = (GameObject)Instantiate(cube, cubeFactory.transform.position, cube.transform.rotation);

            switch (i % 4)
            {
                case 0 :
                    cube.GetComponent<Renderer>().material.color = Color.yellow;
                    break;
                case 1 :
                    cube.GetComponent<Renderer>().material.color = Color.red;
                    break;
                case 2 :
                    cube.GetComponent<Renderer>().material.color = Color.green;
                    break;
                case 3:
                    cube.GetComponent<Renderer>().material.color = Color.blue;
                    break;
            }
                
        }
    }
}
