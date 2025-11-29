using UnityEngine;

public class EnemyC : EnemyBase
{
    [SerializeField] private float xSpeed = -1.2f;
    [SerializeField] private float ySpeed = -0.5f;

    protected override void Move()
    {
        rb.linearVelocity = new Vector2(xSpeed, ySpeed) * speed;
    }
}
