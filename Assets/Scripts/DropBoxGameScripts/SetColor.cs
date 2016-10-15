using UnityEngine;
using System.Collections;

public class SetColor : MonoBehaviour {

    public Color settingColor = Color.blue;

    // Use this for initialization
    void Start () {
        SettingColor();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SettingColor()
    {
        GetComponent<Renderer>().material.color = settingColor;
    }
}
