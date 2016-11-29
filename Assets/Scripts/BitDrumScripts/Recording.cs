using UnityEngine;
using System;
using System.Collections.Generic;

public partial class Recording : MonoBehaviour
{
    AudioClip myAudioClip;
    String deviceName;

    void Start(){
        if (Microphone.devices.Length <= 0)
        { // check if there is at least 1 microphone
            Debug.Log("No mic");
        }
        else
        {
            deviceName = Microphone.devices[0]; // 0 element is the default microphone
        }
        
        //deviceName = Microphone.devices[0];
    }

    public void recoder_start_button(){
        myAudioClip = Microphone.Start(deviceName, true, 10, 44100);
        SavWav.Save("myfile", myAudioClip);
    }

    public void recoder_stop_button()
    {
        Microphone.End(deviceName);
        //myAudioClip
    }
}