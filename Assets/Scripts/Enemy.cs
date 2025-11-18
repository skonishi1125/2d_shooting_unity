using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float lifeTime = 30f;
    [SerializeField] private float hitPoint = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        rb.linearVelocity = Vector2.left * speed;
        Destroy(gameObject, lifeTime);
    }
}
