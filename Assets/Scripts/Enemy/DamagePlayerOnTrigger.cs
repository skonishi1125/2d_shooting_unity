using UnityEngine;

public class DamagePlayerOnTrigger : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private bool destroySelfOnHit = false; // 弾は被弾するとDestroy()させるので、それ用

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != Layers.Player)
            return;

        var health = collision.GetComponent<PlayerHealth>();
        if (health == null)
        {
            Debug.LogWarning($"PlayerHealth が見つかりません。: {collision.name}");
            return;
        }

        health.TakeDamage(damage);

        if (destroySelfOnHit)
            Destroy(gameObject);
    }

}
