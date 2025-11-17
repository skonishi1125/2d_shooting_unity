using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D co;

    [Header("Movement")]
    private float xInputDir = 0;
    private float yInputDir = 0;
    private float speed = 3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        co = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        xInputDir = 0f;
        yInputDir = 0f;

        if (Input.GetKey(KeyCode.W))
            yInputDir = 1f;

        if (Input.GetKey(KeyCode.S))
            yInputDir = -1f;

        if (Input.GetKey(KeyCode.D))
            xInputDir = 1f;

        if (Input.GetKey(KeyCode.A))
            xInputDir = -1f;
    }

    private void FixedUpdate()
    {
        // 上下左右移動
        var next = rb.position + new Vector2((xInputDir * speed * Time.fixedDeltaTime) , (yInputDir * speed * Time.fixedDeltaTime));
        rb.MovePosition(next);
    }


}
