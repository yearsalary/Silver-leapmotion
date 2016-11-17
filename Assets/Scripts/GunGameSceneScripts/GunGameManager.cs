using UnityEngine;
using System.Collections;

public class GunGameManager : MonoBehaviour {

    GrabbingHand.PinchState currentPinchState;
    GrabbingHand gh;
    Transform[] v;
    Transform[] target;

    public GameObject handController;
    public GameObject bullet;
    //public GameObject target;
    GameObject cloneBullet;

    int num;
    float delayTimer;
    public float shootDelayTime;

    // Use this for initialization
    void Start () {
        num = 0;
        delayTimer = 0f;
        shootDelayTime = 20f;
	}
	
	// Update is called once per frame
	void Update () {
        FirePoint();
    }

    //총알을 복제해서 손바닥에서 나가게 하는 코드
    void FirePoint()
    {
        gh = GameObject.Find("LRigidHand(Clone)").GetComponentInChildren<GrabbingHand>();
        v = GameObject.Find("LRigidHand(Clone)").GetComponentsInChildren<Transform>();

        foreach (Transform child in v)
        {
            delayTimer += Time.deltaTime;
            if (child.name.Contains("palm") && gh.GetPinchState().Equals(GrabbingHand.PinchState.kPinched) && delayTimer > shootDelayTime)
            {
                cloneBullet = (GameObject)Instantiate(bullet, child.position, Quaternion.LookRotation(Vector3.forward));
                delayTimer = 0f;
            }
        }
    }

    

}
