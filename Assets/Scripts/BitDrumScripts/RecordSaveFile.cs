using UnityEngine;
using System.Collections;

public class RecordSaveFile : MonoBehaviour {
    //AudioSource audioSource;
    AudioClip audioClip;
    
    void Start () {
        //audioSource = new AudioSource();

    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public void record_end()
    {
        //audioSource = new AudioSource();
        //audioSource.clip = Resources.Load<AudioClip>(Application.persistentDataPath + "/recTest.wav");
        audioClip = new AudioClip();
        //audioClip = Resources.Load<AudioClip>(Application.persistentDataPath + "/recTest.wav");

        WWW www = new WWW(Application.persistentDataPath + "/recTest.wav");
        audioClip = www.GetAudioClip(false);
        Debug.Log((audioClip == null) ? "Sound is null" : "Sound is not null");
        EncodeMP3.convert(audioClip, Application.persistentDataPath + "/convertedMp3.mp3", 128);
    }
}
