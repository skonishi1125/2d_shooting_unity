using UnityEngine;

public class EnemyF : EnemyBase
{
    [SerializeField] private float xSpeed = -1f;
    [SerializeField] private float ySpeed = 0.5f;
    protected override void Move()
    {
        rb.linearVelocity = new Vector2(xSpeed, ySpeed) * speed;
    }
}
