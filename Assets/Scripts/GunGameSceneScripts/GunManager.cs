using UnityEngine;
using System.Collections;

public class GunManager : MonoBehaviour {
    public GameObject handController;

    // Use this for initialization
    void Start () {
        //GetComponent<Transform>().Rotate(-90f, -45f, -140f);
	}
	
	// Update is called once per frame
	void Update () {
        ChaseHand();
    }

    void ChaseHand()
    {
        GameObject hand = getChildGameObject(handController, "LRigidHand(Clone)");
        GameObject hand2 = getChildGameObject(handController, "SaltMediumRoundedLeftHand(Clone)");
        hand2.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;

        if (hand == null)
            return;

        GameObject palm = getChildGameObject(hand, "palm");
        if (palm != null)
        {

            gameObject.transform.position = new Vector3(palm.transform.position.x, gameObject.transform.position.y, palm.transform.position.z);
            GetComponent<Transform>().position = palm.transform.position;
        }

    }

    static public GameObject getChildGameObject(GameObject fromGameObject, string withName)
    {

        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
        return null;
    }
    
}
