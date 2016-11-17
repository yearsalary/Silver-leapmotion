using UnityEngine;
using System.Collections;

public class BulletManager : MonoBehaviour {
    public float speed = 1f;

    void Update()
    {
        GetComponent<Transform>().Translate(Vector3.forward * speed * Time.deltaTime);
        //Debug.Log(Vector3.forward);
    }

}
