using UnityEngine;

public class EnemyK : EnemyBase
{
    [SerializeField] private float xSpeed = -2f;
    [SerializeField] private bool isXZero = false;
    private void Update()
    {
        if (isXZero) return;

        xSpeed += Time.deltaTime * 0.5f;
        if (xSpeed >= 0f)
        {
            xSpeed = 0f;
            isXZero = true;
        }
    }
    protected override void Move()
    {
        rb.linearVelocity = new Vector2(xSpeed, 0f) * speed;
    }
}
