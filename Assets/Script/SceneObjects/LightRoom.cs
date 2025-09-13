using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Movement;

namespace SceneObjects
{
    /// <summary>
    /// LightRoom monitors a trigger collider for a Player presence.
    /// - If a Player stays inside the collider for more than secBeforeLightOut seconds,
    ///   the room becomes lightout and rate is set to 1.
    /// - External actors can call LightOn() to clear lightout and start the rate rising
    ///   at riseRate after a configurable delay (lightOnTime). After LightOn is called
    ///   the room is invincible for invincibleTime seconds and will not go dark.
    /// - A static method provides the average rate across all LightRoom instances.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class LightRoom : MonoBehaviour
    {
        [Tooltip("Time (in seconds) the player must stay in the trigger before the room goes dark.")] [SerializeField]
        private float secBeforeLightOut = 3f;

        [Tooltip("How fast the rate rises (units per second) when LightOn is called. No limit on how large this value can be; the room's rate itself is capped by Max Rate.")]
        [SerializeField] private float riseRate = 1f;

        [Tooltip("Maximum allowed value for 'rate'. When rising, rate will not exceed this value.")]
        [SerializeField] private float maxRate = 5f;

        [Tooltip("Delay (in seconds) after LightOn is called before the rate actually starts to rise.")]
        [SerializeField] private float lightOnTime = 0.5f;

        [Tooltip("During this time (in seconds) after LightOn is called the room is invincible and will not go dark.")]
        [SerializeField] private float invincibleTime = 1.5f;

        // Current rate value for this room.
        [SerializeField] private float rate = 0f;

        // Whether the room is currently dark.
        [SerializeField] private bool lightout = false;

        // Static registry of all LightRoom instances.
        private static readonly List<LightRoom> allRooms = new List<LightRoom>();

        // Coroutines handles so we can cancel them when needed.
        private Coroutine playerWaitCoroutine;
        private Coroutine riseCoroutine;
        private Coroutine invincibleCoroutine;

        // Tracks temporary invincibility after LightOn is called.
        private bool invincible = false;

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
            invincibleCoroutine = null;
            invincible = false;
        }

        private void Awake()
        {
            // Ensure collider is a trigger (common for area detection).
            var col = GetComponent<Collider>();
            if (col != null && !col.isTrigger)
                col.isTrigger = true;

            // Default the current rate to the configured maxRate at startup.
            // This ensures 'rate' starts at its maximum unless overridden at runtime.
            rate = maxRate;
        }

        private void OnTriggerEnter(Collider other)
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

        private void OnTriggerExit(Collider other)
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

            // Respect invincibility: don't turn off the light if we are in invincible state.
            if (!invincible)
            {
                SetLightOut();
            }
        }

        private void SetLightOut()
        {
            // If we're invincible, ignore attempts to set light out.
            if (invincible)
                return;

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
        /// It clears lightout, makes the room invincible for invincibleTime, and after
        /// lightOnTime begins rising the rate toward the maximum (maxRate).
        /// </summary>
        public void LightOn()
        {
            // Clear dark state immediately.
            lightout = false;

            // Cancel any pending player-triggered coroutine — once light is forcibly turned on,
            // we don't want it immediately to be set dark again by an already-pending timer.
            if (playerWaitCoroutine != null)
            {
                StopCoroutine(playerWaitCoroutine);
                playerWaitCoroutine = null;
            }

            // Start invincibility window: stop existing one and begin a fresh timer.
            if (invincibleCoroutine != null)
            {
                StopCoroutine(invincibleCoroutine);
                invincibleCoroutine = null;
            }
            invincibleCoroutine = StartCoroutine(InvincibleCoroutine());

            // Start rising coroutine; cancel if already running to reset behaviour.
            if (riseCoroutine != null)
            {
                StopCoroutine(riseCoroutine);
                riseCoroutine = null;
            }

            // Start rise coroutine which waits for lightOnTime before increasing the rate.
            riseCoroutine = StartCoroutine(RiseRateCoroutine());
        }

        private IEnumerator InvincibleCoroutine()
        {
            invincible = true;
            // Keep invincibility for the configured time.
            yield return new WaitForSeconds(invincibleTime);
            invincible = false;
            invincibleCoroutine = null;
        }

        private IEnumerator RiseRateCoroutine()
        {
            // Wait the configured delay before starting to increase rate.
            if (lightOnTime > 0f)
                yield return new WaitForSeconds(lightOnTime);

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
