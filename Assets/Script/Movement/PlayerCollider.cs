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

        private void OnValidate()
        {
            movement = GetComponent<PlayerMovement>();
        }

        private void OnTriggerEnter(Collider other)
        {
            StartCoroutine(InterruptMovement());
        }

        private IEnumerator InterruptMovement()
        {
            movement.gameObject.SetActive(false);
            yield return new WaitForSeconds(interruptTime);
            movement.gameObject.SetActive(true);
        }
    }
}