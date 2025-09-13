using UnityEngine;
using System;
using Movement;

public class KTVEnemy : MonoBehaviour
{
    Transform ktvEnemy;
    Rigidbody rb;
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed;

    public event Action<Move> OnEnemyChangeDirection;

    private void Awake()
    {
        ktvEnemy = this.transform;
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        //move
        Vector3 dir = (target.position - ktvEnemy.position).normalized;
        rb.linearVelocity = moveSpeed * dir;
    }
    public void GiveTarget(Transform target)
    {
        this.target = target;
    }

    //OnEnemyChangeDirection(Move.Down);
}
