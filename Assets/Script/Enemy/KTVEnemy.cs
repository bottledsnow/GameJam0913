using UnityEngine;
using System;
using Movement;
using System.Collections;

public class KTVEnemy : MonoBehaviour
{
    public static KTVEnemy instance;

    private Transform ktvEnemy;
    private Rigidbody rb;

    private Vector3 dir;
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed;
    private float speedDelta;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    private float audioTimer;
    [SerializeField] private float audioInterval = 0.15f; // how often to play the clip (seconds)

    // One-shot clip to play once during ToTurnLightOn
    [SerializeField] private AudioClip audioClipOneShot;
    // Suppress periodic audio while playing the one-shot in the coroutine
    private bool suppressPeriodicAudio = false;

    [Header("Direction")]
    [SerializeField] private ColorStateChange Up;
    [SerializeField] private ColorStateChange Down;
    [SerializeField] private ColorStateChange Left;
    [SerializeField] private ColorStateChange Right;
    [Header("Room")]
    public float stayTime = 1f;

    public CharacterAnimationController characterAnimationController;
    public event Action<Move> OnEnemyChangeDirection;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        speedDelta = moveSpeed;
        ktvEnemy = this.transform;
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = speedDelta * dir;

        // If suppressed (e.g. during ToTurnLightOn), skip periodic audio playback
        if (suppressPeriodicAudio)
            return;

        // Decrease timer using fixedDeltaTime and play audio when it reaches zero
        audioTimer -= Time.fixedDeltaTime;
        if (audioTimer <= 0f)
        {
            if (audioSource != null && audioClip != null)
            {
                audioSource.PlayOneShot(audioClip);
            }
            // Reset timer to configured interval (small minimum to avoid zero)
            audioTimer = Mathf.Max(0.01f, audioInterval);
        }
    }
    public void GiveTarget(Transform target)
    {
        this.target = target;
        dir = (target.position - ktvEnemy.position).normalized;
        changeState(dir);
    }
    public void changeState(Vector3 dir)
    {
        if(dir.x > 0.5f)
        {
            characterAnimationController.ChangeFacing(Move.Right);            Right.ColorToGreen();
            Left.ColorToRed();
            Up.ColorToRed();
            Down.ColorToRed();
        }
        else if(dir.x < -0.5f)
        {
            characterAnimationController.ChangeFacing(Move.Left);
            Right.ColorToRed();
            Left.ColorToGreen();
            Up.ColorToRed();
            Down.ColorToRed();
        }
        else if(dir.y > 0.5f)
        {
            characterAnimationController.ChangeFacing(Move.Up);
             Right.ColorToRed();
            Left.ColorToRed();
            Up.ColorToGreen();
            Down.ColorToRed();
        }
        else if(dir.y < -0.5f)
        {
            characterAnimationController.ChangeFacing(Move.Down);
            Right.ColorToRed();
            Left.ColorToRed();
            Up.ColorToRed();
            Down.ColorToGreen();
        }

    }
    public IEnumerator ToTurnLightOn()
    {
        speedDelta = 0;
        characterAnimationController.ChangeState(AnimationStateEnum.Use);

        // Suppress periodic audio and play the one-shot clip once
        suppressPeriodicAudio = true;
        if (audioSource != null && audioClipOneShot != null)
        {
            audioSource.PlayOneShot(audioClipOneShot);
        }

        yield return new WaitForSeconds(stayTime); // �� timeScale �v�T

        // Re-enable periodic audio and reset the timer so periodic audio doesn't immediately fire
        suppressPeriodicAudio = false;
        audioTimer = Mathf.Max(0.01f, audioInterval);

        characterAnimationController.ChangeState(AnimationStateEnum.Walk);
        speedDelta = moveSpeed;
    }
    //OnEnemyChangeDirection(Move.Down);
}
