using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerInputSet input;

    [Header("Movement")]
    [SerializeField] public float speed = 3f;
    [SerializeField] public Vector2 moveInput;
    [SerializeField] public bool attackPressed;

    [Header("Attack Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 0.2f;
    private float fireTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = new PlayerInputSet();
    }

    private void Update()
    {
        if (attackPressed)
        {
            fireTimer -= Time.deltaTime;

            if (fireTimer <= 0f)
            {
                Fire();
                fireTimer = fireInterval;
            }
        }
        else
        {
            fireTimer = 0f;
        }
    }

    private void Fire()
    {
        if (bulletPrefab == null || firePoint == null)
            return;

        Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
    }

    private void FixedUpdate()
    {
        // 上下左右移動
        var next = rb.position + new Vector2((moveInput.x * speed * Time.fixedDeltaTime), (moveInput.y * speed * Time.fixedDeltaTime));
        rb.MovePosition(next);
    }


    // SetActiveがtrueになったときのメソッド
    // Awakeの次に実行される
    // inputはAwakeでクラス生成して、OnEnableで有効化するという流れ
    private void OnEnable()
    {
        input.Enable();

        // started: 触れた瞬間 performed: その間 canceled: 離したとき
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCanceled;
        input.Player.Attack.performed += OnAttackPerformed;
        input.Player.Attack.canceled += OnAttackCanceled;
    }

    private void OnDisable()
    {
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCanceled;
        input.Player.Attack.performed -= OnAttackPerformed;
        input.Player.Attack.canceled -= OnAttackCanceled;

        input.Disable();

    }

    private void OnMovementPerformed(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnMovementCanceled(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
    }

    private void OnAttackPerformed(InputAction.CallbackContext ctx)
    {
        attackPressed = true;
    }

    private void OnAttackCanceled(InputAction.CallbackContext ctx)
    {
        attackPressed = false;
    }


}
