using System;
using UnityEngine;
using System.Collections;
using TMPro;

public class Timer : MonoBehaviour
{
    // Fired when the timer reaches zero.
    public event Action TimerEnded;

    // Fired whenever the displayed integer seconds change (useful for UI number displays).
    // Sends the whole seconds remaining (ceil(Remaining) while >0, and 0 at end).
    public event Action<int> TimerNumberChanged;

    // Optional TextMeshPro text to update automatically (assign in Inspector).
    [SerializeField] private TMP_Text textMeshPro;

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
            TimerNumberChanged?.Invoke(0);
            UpdateText(0);
            TimerEnded?.Invoke();
            return;
        }

        Remaining = seconds;
        IsRunning = true;

        // Fire initial integer display value immediately.
        int initial = Mathf.CeilToInt(Remaining);
        TimerNumberChanged?.Invoke(initial);
        UpdateText(initial);

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
        int previousDisplayed = Mathf.CeilToInt(Remaining);

        while (Remaining > 0f)
        {
            // Wait for the next frame and decrease by unscaledDeltaTime so it's unaffected by timeScale.
            // If you prefer to be affected by timeScale, replace with Time.deltaTime.
            yield return null;
            Remaining -= Time.unscaledDeltaTime;

            // Compute the integer value suitable for display (ceiling so "5" shows until it goes below 5.0).
            int currentDisplayed = Mathf.CeilToInt(Mathf.Max(0f, Remaining));
            if (currentDisplayed != previousDisplayed)
            {
                previousDisplayed = currentDisplayed;
                TimerNumberChanged?.Invoke(currentDisplayed);
                UpdateText(currentDisplayed);
            }
        }

        Remaining = 0f;
        IsRunning = false;

        // Ensure display shows 0 at the end.
        TimerNumberChanged?.Invoke(0);
        UpdateText(0);
        TimerEnded?.Invoke();
    }

    // Safely update the assigned TextMeshPro text (no-op if not assigned).
    private void UpdateText(int seconds)
    {
        if (textMeshPro != null)
            textMeshPro.text = seconds.ToString();
    }
}