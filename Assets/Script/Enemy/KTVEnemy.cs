using UnityEngine;
using System;
using Movement;
using System.Collections;

public class KTVEnemy : MonoBehaviour
{
    private Transform ktvEnemy;
    private Rigidbody rb;

    private Vector3 dir;
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed;
    private float speedDelta;

    [Header("Direction")]
    [SerializeField] private ColorStateChange Up;
    [SerializeField] private ColorStateChange Down;
    [SerializeField] private ColorStateChange Left;
    [SerializeField] private ColorStateChange Right;
    [Header("Room")]
    public float stayTime = 1f;

    public event Action<Move> OnEnemyChangeDirection;

    private void Awake()
    {
        speedDelta = moveSpeed;
        ktvEnemy = this.transform;
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        //move
        rb.linearVelocity = speedDelta * dir;
    }
    public void GiveTarget(Transform target)
    {
        this.target = target;
        dir = (target.position - ktvEnemy.position).normalized;
        changeState(dir);
    }
    public void changeState(Vector3 dir)
    {
        if(dir.x > 0.5f)
        {
            OnEnemyChangeDirection?.Invoke(Move.Right);
            Right.ColorToGreen();
            Left.ColorToRed();
            Up.ColorToRed();
            Down.ColorToRed();
        }
        else if(dir.x < -0.5f)
        {
            OnEnemyChangeDirection?.Invoke(Move.Left);
            Right.ColorToRed();
            Left.ColorToGreen();
            Up.ColorToRed();
            Down.ColorToRed();
        }
        else if(dir.y > 0.5f)
        {
            OnEnemyChangeDirection?.Invoke(Move.Up);
            Right.ColorToRed();
            Left.ColorToRed();
            Up.ColorToGreen();
            Down.ColorToRed();
        }
        else if(dir.y < -0.5f)
        {
            OnEnemyChangeDirection?.Invoke(Move.Down);
            Right.ColorToRed();
            Left.ColorToRed();
            Up.ColorToRed();
            Down.ColorToGreen();
        }
         
    }
    public IEnumerator ToTurnLightOn()
    {
        speedDelta = 0;
        yield return new WaitForSeconds(stayTime); // ¨ü timeScale ¼vÅT
        speedDelta = moveSpeed;
    }
    //OnEnemyChangeDirection(Move.Down);
}
