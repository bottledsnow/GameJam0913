using System;
using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour
{
    // Fired when the timer reaches zero.
    public event Action TimerEnded;

    // Whether the timer is currently running.
    public bool IsRunning { get; private set; }

    // Remaining time in seconds (read-only externally).
    public float Remaining { get; private set; }

    // Starts (or restarts) the timer for the provided duration in seconds.
    // If seconds <= 0 the TimerEnded event is invoked immediately.
    public void StartTimer(float seconds)
    {
        StopTimerInternal(); // stop any existing timer
        if (seconds <= 0f)
        {
            Remaining = 0f;
            IsRunning = false;
            TimerEnded?.Invoke();
            return;
        }

        Remaining = seconds;
        IsRunning = true;
        StartCoroutine(RunTimer());
    }

    // Stops the timer without invoking the end event.
    public void StopTimer()
    {
        StopTimerInternal();
        IsRunning = false;
    }

    // Internal helper to stop any running coroutine.
    private void StopTimerInternal()
    {
        // Stop all coroutines started on this MonoBehaviour that belong to this class.
        // This is simple and safe for this component since it only starts one coroutine.
        StopAllCoroutines();
    }

    // Coroutine that updates the remaining time and fires the event when finished.
    private IEnumerator RunTimer()
    {
        while (Remaining > 0f)
        {
            // Wait for the next frame and decrease by unscaledDeltaTime so it's unaffected by timeScale.
            // If you prefer to be affected by timeScale, replace with Time.deltaTime.
            yield return null;
            Remaining -= Time.unscaledDeltaTime;
        }

        Remaining = 0f;
        IsRunning = false;
        TimerEnded?.Invoke();
    }
}