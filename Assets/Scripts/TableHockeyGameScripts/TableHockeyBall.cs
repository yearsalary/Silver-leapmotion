using UnityEngine;
using System.Collections;

public class TableHockeyBall : MonoBehaviour {
	public GameObject netWorkCtrl;
	private Vector3 moveDirection;
	private float speed = 10f;

	void Start() {
		this.moveDirection = new Vector3 (0, 0, 0);
	}

	void Update () {
		if(moveDirection!=Vector3.zero && transform.position.z < -4f && netWorkCtrl.GetComponent<TableHockeySocketIOController>().isBallOwner())
			transform.Translate (moveDirection * speed* Time.deltaTime);
	}

	public void SetMoveDirection(Vector3 moveDirection) {
		this.moveDirection = moveDirection;
	}

	public void SetSpeed(float speed) {
		this.speed = speed;
	}

	public Vector3 getMoveDirection() {
		return this.moveDirection;
	}
}
