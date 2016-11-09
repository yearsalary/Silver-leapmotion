using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

    GrabbingHand.PinchState currentPinchState;
    GrabbingHand gh;
    Transform[] v;
    public GameObject cube;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        gh = GameObject.Find("LRigidHand(Clone)").GetComponentInChildren<GrabbingHand>();
        v = GameObject.Find("LRigidHand(Clone)").GetComponentsInChildren<Transform>();

        Debug.Log(gh.GetPinchState());

        
        foreach(Transform child in v)
        {
            if (child.name.Contains("palm") && gh.GetPinchState().Equals(GrabbingHand.PinchState.kPinched))
            {
                Instantiate(cube,child.position,Quaternion.LookRotation(Vector3.forward));
            }
        }

    }
}
