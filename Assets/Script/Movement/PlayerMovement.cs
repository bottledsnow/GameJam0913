using System;
using UnityEngine;

namespace Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [Tooltip("Movement speed in units per second.")]
        [SerializeField] private float speed = 5f;

        [SerializeField] private Rigidbody2D rb;

        // Frontend-facing event: subscribers receive the current move vector (X,Y) whenever it changes.
        public event Action<Vector2> onMoveChanged;

        // Latest movement vector provided by input event (X,Y)
        private Vector2 _currentMove = Vector2.zero;

        private void OnValidate()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            PlayerInput.OnMoveInput += HandleMoveInput;
        }

        private void OnDisable()
        {
            PlayerInput.OnMoveInput -= HandleMoveInput;
        }

        // Event handler called by PlayerInput every frame with the desired movement
        private void HandleMoveInput(Vector2 move)
        {
            // Only notify subscribers when the movement actually changes
            if (move != _currentMove)
            {
                _currentMove = move;
                onMoveChanged?.Invoke(_currentMove);
            }
        }

        private void FixedUpdate()
        {
            // Convert 2D move to 3D vector for position movement (z = 0)
            var move3 = new Vector3(_currentMove.x, _currentMove.y, 0f);

            // Apply movement using position (frame-rate independent).
            rb.MovePosition(transform.position + move3 * (speed * Time.deltaTime));
        }
    }
}