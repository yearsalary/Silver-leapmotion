using UnityEngine;
using System.Collections;

public class GunGameManager : MonoBehaviour {

    GrabbingHand.PinchState currentPinchState;
    GrabbingHand gh;
    Transform[] v;

    public GameObject handController;
    public GameObject bullet;
    GameObject cloneBullet;

    float delayTimer = 0f;
    public float shootDelayTime = 1f;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        gh = GameObject.Find("LRigidHand(Clone)").GetComponentInChildren<GrabbingHand>();
        v = GameObject.Find("LRigidHand(Clone)").GetComponentsInChildren<Transform>();

        //Debug.Log(gh.GetPinchState());

        
        foreach(Transform child in v)
        {
            delayTimer += Time.deltaTime;
            if (child.name.Contains("palm") && gh.GetPinchState().Equals(GrabbingHand.PinchState.kPinched) && delayTimer > shootDelayTime)
            {
                cloneBullet = (GameObject)Instantiate(bullet, child.position, Quaternion.LookRotation(Vector3.forward));
                delayTimer = 0f;
            }
        }

    }

    void ChaseHand()
    {
        GameObject hand = getChildGameObject(handController, "LRigidHand(Clone)");
        if (hand == null)
            return;

        GameObject palm = getChildGameObject(hand, "palm");
        if (palm != null)
        {
            gameObject.transform.position = new Vector3(palm.transform.position.x, gameObject.transform.position.y, palm.transform.position.z);
        }

    }

    static public GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {

        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }
}
