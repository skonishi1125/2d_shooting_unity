using UnityEngine;

public class EnemyJ : EnemyBase
{
    protected override void Move()
    {
        rb.linearVelocity = Vector2.left * speed;
    }
}
