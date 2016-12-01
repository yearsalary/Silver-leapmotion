using UnityEngine;

public class DrumManager : MonoBehaviour {
    AudioSource audioSource;
    Light drumLight;
    bool isCheck;
    float delayTimer;
    float delayTime;

    // Use this for initialization
    void Start () {
        audioSource = this.GetComponent<AudioSource>();
        drumLight = this.GetComponent<Light>();
        delayTimer = 0f;
        delayTime = 0.15f;
    }
	
	// Update is called once per frame
	void Update () {
        delayTimer += Time.deltaTime;
	}

    void OnTriggerEnter(Collider other){

        if (delayTimer > delayTime){
            delayTimer = 0f;
            audioSource.Play();
            drumLight.intensity = 1;
        }
    }


    void OnTriggerExit(Collider other)
    {
        drumLight.intensity = 0;
    }
}
