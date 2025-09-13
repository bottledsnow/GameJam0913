using System;
using UnityEngine;

namespace Movement
{
    /// <summary>
    /// Reads player input (WASD / arrow keys) and broadcasts a movement vector each frame.
    /// Other systems (e.g. PlayerMovement) can subscribe to PlayerInput.OnMoveInput to receive updates.
    /// </summary>
    public class PlayerInput : MonoBehaviour
    {
        // Subscribers receive a Vector2 where X is horizontal (-1..1) and Y is vertical (-1..1).
        public static event Action<Vector2> OnMoveInput;

        private void Update()
        {
            var horizontal = 0f;
            var vertical = 0f;

            if (Input.GetKey(KeyCode.A)) horizontal -= 1f;
            if (Input.GetKey(KeyCode.D)) horizontal += 1f;
            if (Input.GetKey(KeyCode.W)) vertical += 1f;
            if (Input.GetKey(KeyCode.S)) vertical -= 1f;

            var move = new Vector2(horizontal, vertical);

            // Normalize so diagonal movement isn't faster, but keep zero case safe.
            if (move.sqrMagnitude > 1f)
                move.Normalize();

            OnMoveInput?.Invoke(move);
        }
    }
}
