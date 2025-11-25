using UnityEngine;

public class PooledBullet : MonoBehaviour
{
    private BulletPool pool;

    // しまう先のpoolの指定
    // BulletPoolでInstantiateしたときに併せて呼んで紐づけておく。
    public void SetPool(BulletPool pool)
    {
        this.pool = pool;
    }

    // 弾丸の在庫格納処理
    public void Despawn()
    {
        if (pool != null)
        {
            pool.Return(gameObject);
        }
        else
        {
            // 万一プールが設定されていない時の保険
            Destroy(gameObject);
        }
    }
}
