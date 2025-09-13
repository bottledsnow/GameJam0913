using System.Collections;
using Movement;
using UnityEngine;

namespace Collider
{
    [RequireComponent(typeof(Collider2D),typeof(PlayerMovement))]
    public class PlayerCollider : MonoBehaviour
    {
        [SerializeField] private float interruptTime = 3f;

        [SerializeField] private PlayerMovement movement;

        private void OnValidate()
        {
            movement = GetComponent<PlayerMovement>();
        }

        private void OnTriggerEnter2D(Collider2D other)
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