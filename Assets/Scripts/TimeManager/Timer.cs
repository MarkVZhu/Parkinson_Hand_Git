using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    string name;
    public Timer(string name)
    {
        this.name = name;
    }

    bool isRecording = false;
    float currentTime = 0;
    float startTime;
    float pauseTime = 0;

    public void StartRecording()
    {
        isRecording = true;
        startTime = Time.time;
    }

    public void PauseRecording()
    {
        pauseTime = Time.time - pauseTime; // Include the last pauseTime 
        isRecording = false;
    }

    public void RestartRecording()
    {
        pauseTime = Time.time - pauseTime; // In this formula, the last pauseTime has been added [ Tp = (T - T') + Tp' ] 
        isRecording = true;
    }

    public float GetCurrentRecord()
    {
        if (isRecording)
        {
            currentTime = Time.time - startTime - pauseTime;
            return currentTime;
        }
        else return pauseTime - startTime ;
    }

    public void Reset()
    {
        isRecording = false;
        startTime = 0;
    }

    //Change float time into the actual time format 
    public static string ToTimeFormat(float time)
    {
        int tenMs = (int)(time * 100 % 100);
        int seconds = (int)time;
        int min = seconds % 3600 / 60;
        seconds = seconds % 3600 % 60;
        return string.Format("{0:D2}:{1:D2}:{2:D2}", min, seconds, tenMs);
    }
}