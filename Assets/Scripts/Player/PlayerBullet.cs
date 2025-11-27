using UnityEngine;

public class PlayerBullet : BulletBase
{
    // 弾丸を呼び出したときの初期化メソッド
    public void Init(int damage, float lifetime)
    {
        this.damage = damage;
        this.lifeTime = lifetime;

        rb.linearVelocity = (Vector2)transform.right * speed;

        ScheduleDespawn();
    }


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // 弾が敵レイヤーに当たった場合は、自身を削除(弾自身の責務)
        // ダメージ自体はIDamageSourceの責務、そのダメージでのライフ計算はEnemyHealthの責務
        //if (collision.gameObject.layer == Layers.Enemy)
        //    Destroy(gameObject);

        if (collision.gameObject.layer != Layers.Enemy)
            return;

        Despawn();
    }
}
