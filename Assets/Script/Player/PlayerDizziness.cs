using Movement;
using UnityEngine;
using System.Collections;
using System.Linq;

public class PlayerDizziness : MonoBehaviour
{
    [SerializeField] private ParticleSystem Parti_Dizz;
    private PlayerMovement playerMovement;
    private float playermovespeed;
    private bool isDizziness = false;
    [SerializeField] private float dizzinessTime = 3.00f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip dizzinessClip;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playermovespeed = playerMovement.speed;

    // If AudioSource not assigned in inspector, try to get one from the same GameObject.
    if (audioSource == null)
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if(!isDizziness && playerMovement.speed != playermovespeed)
        {
            playerMovement.speed = playermovespeed;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyDamage")
        {
            if (!isDizziness)
            {
                // Play immediate sound on trigger enter if available.
                if (audioSource == null)
                    audioSource = GetComponent<AudioSource>();

                if (audioSource != null && dizzinessClip != null)
                {
                    try
                    {
                        audioSource.PlayOneShot(dizzinessClip);
                    }
                    catch
                    {
                        // Swallow audio exceptions to avoid breaking game loop.
                    }
                }

                StartCoroutine(dizziness());
            }
        }
    }
    IEnumerator dizziness()
    {
        Parti_Dizz.gameObject.SetActive(true);

        // Play the dizziness clip when the effect starts.
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource != null && dizzinessClip != null)
        {
            try
            {
                audioSource.PlayOneShot(dizzinessClip);
            }
            catch
            {
                // Swallow audio exceptions to avoid breaking game loop.
            }
        }

        playerMovement.speed = 0;
        isDizziness = true;
        yield return new WaitForSeconds(dizzinessTime); // �� timeScale �v�T
        playerMovement.speed = playermovespeed;
        isDizziness = false;
        Parti_Dizz.gameObject.SetActive(false);
    }
}
