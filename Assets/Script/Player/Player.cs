using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerController controls;

    private void Awake()
    {
        controls = new PlayerController();
        //controller.Player.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
    }
}
