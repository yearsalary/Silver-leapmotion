using UnityEngine;
using System.Collections;

public class HitBall : MonoBehaviour {
	public int speed;


	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.back * speed * Time.deltaTime);
	}
		
}
