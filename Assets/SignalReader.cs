using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SignalReader : MonoBehaviour {

    public float pitch;
    public float gain;

    private double nextTick = 0.0F;
    private float amp = 0.0F;
    private float phase = 0.0F;
    private double sampleRate = 0.0F;
    private int accent;
    private bool running = false;

    double p = 0;
    void OnEnable()
    {
        // double startTick = AudioSettings.dspTime;
        sampleRate = AudioSettings.outputSampleRate;
        // nextTick = startTick * sampleRate;
        running = true;
    }

    void Update(){
        pitch = 440 + Mathf.Sin(Time.time)  * 40;

    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (!running)
            return;


        double sample = AudioSettings.dspTime * sampleRate;
        int dataLen = data.Length / channels;
        double increment = pitch * 2 * Math.PI / sampleRate;
        int n = 0;
        while (n < dataLen){
            p += increment;
            float x = Mathf.Sin((float)p);// < 0 ?  1 : -1;
            int i = 0;
            while (i < channels){
                data[n * channels + i] = x;
                i++;
            }
            n++;
        }
    
    }
  }