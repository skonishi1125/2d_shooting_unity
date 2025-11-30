using UnityEngine;

public class EnemyM : EnemyBase
{
    protected override void Move()
    {
        rb.linearVelocity = Vector2.left * speed;
    }
}
