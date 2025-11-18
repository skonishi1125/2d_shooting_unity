using UnityEngine;

public class EnemyA : EnemyBase
{
    protected override void Move()
    {
        rb.linearVelocity = Vector2.left * speed;
    }

}
