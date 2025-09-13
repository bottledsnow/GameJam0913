using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;
using UnityEditor;

namespace SceneObjects
{
    /// <summary>
    /// LightRoom monitors a trigger collider for a Player presence.
    /// - If a Player stays inside the collider for more than secBeforeLightOut seconds,
    ///   the room becomes lightout and rate is set to 1.
    /// - External actors can call LightOn() to clear lightout and start the rate rising
    ///   at riseRate until it reaches a maximum of 5.
    /// - A static method provides the average rate across all LightRoom instances.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class LightRoom : MonoBehaviour
    {
        [Tooltip("Time (in seconds) the player must stay in the trigger before the room goes dark.")] [SerializeField]
        private float secBeforeLightOut = 3f;

        [Tooltip("How fast the rate rises (units per second) when LightOn is called. No limit on how large this value can be; the room's rate itself is capped by Max Rate.")]
        [SerializeField] private float riseRate = 1f;

        [Tooltip("Maximum allowed value for 'rate'. When rising, rate will not exceed this value.")]
        [SerializeField] private float maxRate = 5f;

        // Current rate value for this room.
        [SerializeField] private float rate = 0f;

        // Whether the room is currently dark.
        [SerializeField] private bool lightout = false;

        // Static registry of all LightRoom instances.
        private static readonly List<LightRoom> allRooms = new List<LightRoom>();

        // Coroutines handles so we can cancel them when needed.
        private Coroutine playerWaitCoroutine;
        private Coroutine riseCoroutine;

        // Public accessors
        public bool LightOut => lightout;
        public float Rate => rate;

        private void OnEnable()
        {
            if (!allRooms.Contains(this))
                allRooms.Add(this);
        }

        private void OnDisable()
        {
            allRooms.Remove(this);
            StopAllCoroutines();
            playerWaitCoroutine = null;
            riseCoroutine = null;
        }

        private void Awake()
        {
            // Ensure collider is a trigger (common for area detection).
            var col = GetComponent<Collider2D>();
            if (col != null && !col.isTrigger)
                col.isTrigger = true;

            // Default the current rate to the configured maxRate at startup.
            // This ensures 'rate' starts at its maximum unless overridden at runtime.
            rate = maxRate;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Detect player by presence of PlayerMovement component.
            if (other.TryGetComponent<PlayerMovement>(out var _))
            {
                // Start the timer to make the room lightout if the player stays.
                if (playerWaitCoroutine == null)
                    playerWaitCoroutine = StartCoroutine(PlayerStayedCoroutine());
            }

            // Note: We intentionally do not implement detection for the other entity here,
            // because that entity will call LightOn() directly when it enters; we provide LightOn() for it.
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            // If player leaves before the timer elapses, cancel the lightout sequence.
            if (other.TryGetComponent<PlayerMovement>(out var _))
            {
                if (playerWaitCoroutine != null)
                {
                    StopCoroutine(playerWaitCoroutine);
                    playerWaitCoroutine = null;
                }
            }
        }

        private IEnumerator PlayerStayedCoroutine()
        {
            // Wait the configured time; if the player remains, mark room as lightout.
            yield return new WaitForSeconds(secBeforeLightOut);
            playerWaitCoroutine = null;

            SetLightOut();
        }

        private void SetLightOut()
        {
            lightout = true;

            // When room goes dark, set rate to 1 immediately and stop rise coroutine.
            rate = 1f;

            if (riseCoroutine != null)
            {
                StopCoroutine(riseCoroutine);
                riseCoroutine = null;
            }
        }

        /// <summary>
        /// External callers (other entities) should call this to turn the light on.
        /// It clears lightout and starts the rate rising toward the maximum (5).
        /// </summary>
        public void LightOn()
        {
            // Clear dark state
            lightout = false;

            // Cancel any pending player-triggered coroutine — once light is forcibly turned on,
            // we don't want it immediately to be set dark again by an already-pending timer.
            if (playerWaitCoroutine != null)
            {
                StopCoroutine(playerWaitCoroutine);
                playerWaitCoroutine = null;
            }

            // Start rising coroutine; cancel if already running to reset behaviour.
            if (riseCoroutine != null)
                StopCoroutine(riseCoroutine);

            riseCoroutine = StartCoroutine(RiseRateCoroutine());
        }

        private IEnumerator RiseRateCoroutine()
        {
            // Increase rate smoothly up to the configurable maximum (maxRate).
            while (rate < maxRate)
            {
                rate += riseRate * Time.deltaTime;
                if (rate > maxRate) rate = maxRate;
                yield return null;
            }

            riseCoroutine = null;
        }

        /// <summary>
        /// Compute the average rate across all active LightRoom instances.
        /// Returns 0 if there are no rooms.
        /// </summary>
        public static float AverageRateAcrossAll()
        {
            if (allRooms.Count == 0) return 0f;
            float sum = 0f;
            for (int i = 0; i < allRooms.Count; i++)
            {
                sum += allRooms[i].Rate;
            }

            return sum / allRooms.Count;
        }
    }
}
