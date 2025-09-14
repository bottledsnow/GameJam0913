using System.Collections;
using Movement;
using UnityEngine;

namespace Movement
{
    [RequireComponent(typeof(Collider),typeof(PlayerMovement))]
    public class PlayerCollider : MonoBehaviour
    {
        [SerializeField] private float interruptTime = 3f;

        [SerializeField] private PlayerMovement movement;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip interruptClip;

        private void OnValidate()
        {
            movement = GetComponent<PlayerMovement>();
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter(Collider other)
        {
            StartCoroutine(InterruptMovement());
        }

        private IEnumerator InterruptMovement()
        {
            // Try to ensure we have an AudioSource reference
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();

            // Play the configured interrupt clip if available.
            if (audioSource != null && interruptClip != null)
            {
                try
                {
                    audioSource.PlayOneShot(interruptClip);
                }
                catch
                {
                    // Swallow audio exceptions to avoid breaking game loop.
                }
            }

            movement.gameObject.SetActive(false);
            yield return new WaitForSeconds(interruptTime);
            movement.gameObject.SetActive(true);
        }
    }
}