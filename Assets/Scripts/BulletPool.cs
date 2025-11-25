using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int initialSize = 50;

    private readonly Queue<GameObject> pool = new Queue<GameObject>();

    // 開始時点で、在庫を作成
    private void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            var obj = CreateNewBullet();
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
        Debug.Log($"[Awake] BulletPool {GetInstanceID()} 在庫: {pool.Count}");
    }


    private GameObject CreateNewBullet()
    {
        var obj = Instantiate(bulletPrefab, transform);
        var bullet = obj.GetComponent<PooledBullet>();
        if (bullet != null)
        {
            bullet.SetPool(this);
        }
        return obj;
    }

    // 在庫から取り出す処理
    // 足りなくなったら追加で生成して返す。
    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        if (pool.Count == 0)
        {
            Debug.Log("BulletPool: 在庫がなくなったので作成しました。");
            pool.Enqueue(CreateNewBullet());
        }

        var obj = pool.Dequeue();
        Debug.Log($"BulletPool {GetInstanceID()}: 弾丸取り出し。 在庫: {pool.Count}");

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        return obj;

    }

    // 使い終わった弾丸を在庫に戻す処理
    public void Return(GameObject obj)
    {
        Debug.Log($"BulletPool: 弾丸格納。");
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

}
