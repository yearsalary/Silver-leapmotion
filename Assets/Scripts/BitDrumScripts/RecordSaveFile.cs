using UnityEngine;
using System.Collections;

public class RecordSaveFile : MonoBehaviour {
	AudioClip clip;
    AudioSource ad;
    // Use this for initialization
    void Start () {
        ad = gameObject.GetComponent<AudioSource>();
        ad.Play();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void record_start()
    {
        //Debug.Log("11");
        Debug.Log(Microphone.devices.Length);
        ad.clip = Microphone.Start(null, true, 10, 44100);
        //ad.clip.LoadAudioData
    }

    public void record_end()
    {
        Debug.Log(Application.dataPath);
        Microphone.End(null);
        EncodeMP3.convert(ad.clip, Application.dataPath + "/convertedMp3.mp3", 128);
    }
}
