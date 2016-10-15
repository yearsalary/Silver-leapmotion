using UnityEngine;
using System.Collections;

public class BoxManager : MonoBehaviour
{

    public GameObject controlObject;
    private SetColor component;

    // Use this for initialization
    void Start()
    {
        controlObject = GameObject.Find("Cube");

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        component = controlObject.GetComponent<SetColor>();
        component.SettingColor();
    }
}
