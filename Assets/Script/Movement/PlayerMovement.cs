using System;
using UnityEngine;

namespace Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [Tooltip("Movement speed in units per second.")]
         public float speed = 5f;

        [SerializeField] private Rigidbody rb;
        [SerializeField] private AudioSource aud;

        // Frontend-facing event: subscribers receive the current move vector (X,Y) whenever it changes.
        public event Action<Vector2> onMoveChanged;

        // Latest movement vector provided by input event (X,Y)
        private Vector2 _currentMove = Vector2.zero;
        private PlayerState playerState;
        public event Action<Move> OnSateChange;
        public event Action OnLightClose;
         [SerializeField] private CharacterAnimationController characterAnimationController;

        public void PlayerCloseTheLight()
        {
            OnLightClose?.Invoke();
        }
        private void Awake()
        {
            playerState = GetComponent<PlayerState>();
        }

        private void OnValidate()
        {
            rb = GetComponent<Rigidbody>();
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

            
            //ChangePlayerDirection
            if(_currentMove.x > 0)
            {
                OnSateChange?.Invoke(Move.Right);
                playerState.ChangeDirection(Move.Right);
                characterAnimationController.ChangeFacing(Move.Right);
            }
            else if(_currentMove.x < 0)
            {
                OnSateChange?.Invoke(Move.Left);
                playerState.ChangeDirection(Move.Left);
                characterAnimationController.ChangeFacing(Move.Left);
            }
            else if(_currentMove.y > 0)
            {
                OnSateChange?.Invoke(Move.Up);
                playerState.ChangeDirection(Move.Up);
                characterAnimationController.ChangeFacing(Move.Up);
            }
            else if(_currentMove.y < 0)
            {
                OnSateChange?.Invoke(Move.Down);
                playerState.ChangeDirection(Move.Down);
                characterAnimationController.ChangeFacing(Move.Down);                playerState.ChangeDirection(Move.Down);
            }



            // Simple footsteps check
            if (_currentMove.sqrMagnitude > 0.01f)
            {
                if (!aud.isPlaying) aud.Play();
            }
            else
            {
                if (aud.isPlaying) aud.Stop();
            }
        }
    }
}