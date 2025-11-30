using UnityEngine;

public class EnemyI : EnemyBase
{
    protected override void Move()
    {
        rb.linearVelocity = Vector2.left * speed;
    }
}
