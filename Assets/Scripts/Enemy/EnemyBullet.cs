using UnityEngine;

public class EnemyBullet : BulletBase
{

    private Vector2 moveDir = Vector2.left; // デフォルト: 左に発射


    // Shooter からInstantiateした際に、その弾にパラメータを設定する際に呼ぶ
    public void Init(Vector2 direction)
    {
        moveDir = direction.normalized;
        rb.linearVelocity = moveDir.normalized * speed;

        ScheduleDespawn();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != Layers.Player)
            return;

        Despawn();
    }

}
