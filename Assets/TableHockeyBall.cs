using UnityEngine;
using System.Collections;

public class TableHockeyBall : MonoBehaviour {
	private Vector3 moveDirection;
	private float speed = 2f;

	void Update () {
		if(moveDirection!=null)
			transform.Translate (moveDirection * speed* Time.deltaTime);
	}

	public void SetMoveDirection(Vector3 moveDirection) {
		Debug.Log ("qqqq"+moveDirection);
		this.moveDirection = moveDirection;
	}

	public void SetSpeed(float speed) {
		this.speed = speed;
	}

	public Vector3 getMoveDirection() {
		return this.moveDirection;
	}
}
