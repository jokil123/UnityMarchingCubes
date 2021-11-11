using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private System.DateTime startTime;
    private string timerName;

    public Timer(string name)
    {
        timerName = name;

        ResetTimer();
    }

    public void End()
    {
        Debug.Log(timerName + ": " + GetDuration().Milliseconds);
        ResetTimer();
    }

    public float EndTime()      // Returns time in ms
    {
        float duration = GetDuration().Milliseconds;
        ResetTimer();
        return duration;
    }

    private System.TimeSpan GetDuration()
    {
        System.DateTime endTime = System.DateTime.Now;
        return endTime.Subtract(startTime);
    }

    private void ResetTimer()
    {
        startTime = System.DateTime.Now;
    }
}

public static class TimerDisplay
{
    static Dictionary<string, List<float>> timerTimes = new Dictionary<string, List<float>>();

    static TimerDisplay()
    {

    }
}