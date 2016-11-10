using UnityEngine;
using System.Collections;

public class BulletManager : MonoBehaviour {
    public float speed = 1f;

    /*void Start()
    {
        GetComponent<Transform>().Translate(transform.forward * speed * Time.deltaTime);
    }*/

    void Update()
    {
        GetComponent<Transform>().Translate(Vector3.forward * speed * Time.deltaTime);
        //Debug.Log(Vector3.forward);
    }

}
