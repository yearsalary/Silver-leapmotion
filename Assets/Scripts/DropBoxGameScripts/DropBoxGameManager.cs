using UnityEngine;
using System.Collections;

public class DropBoxGameManager : MonoBehaviour {
    public GameObject cubeFactory;
    public GameObject cube;

	// Use this for initialization
	void Start () {
        MakeBox(6);

    }
	
	// Update is called once per frame
	void Update () {

	}

    void MakeBox(int count)
    {
        GameObject cube;

        for(int i=0; i<count; i++)
        {
            cube = (GameObject)Instantiate(cube, cubeFactory.transform.position, cube.transform.rotation);
        }
    }
}
