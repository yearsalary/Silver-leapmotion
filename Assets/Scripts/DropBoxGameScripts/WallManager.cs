using UnityEngine;
using System.Collections;

public class WallManager : MonoBehaviour {
    public float playTime;
    public GameObject wall;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        playTime += Time.deltaTime;

        if (playTime >= 3f)
        {
            Destroy(wall);
        }
	}
}
