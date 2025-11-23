using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerInputSet input;
    private PlayerStatus status;
    private PlayerShooter shooter;

    [Header("Movement")]
    [SerializeField] public Vector2 moveInput;
    [SerializeField] public bool attackPressed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = new PlayerInputSet();
        status = GetComponent<PlayerStatus>();
        shooter = GetComponent<PlayerShooter>();
    }

    private void Update()
    {
        if (GameManager.Instance.IsStageClear)
            return;

        if (attackPressed)
        {
            shooter.Fire();
        }
    }


    private void FixedUpdate()
    {
        if (GameManager.Instance.IsStageClear)
            return;

        // 上下左右移動
        var next = rb.position + new Vector2(
            (moveInput.x * status.MoveSpeed * Time.fixedDeltaTime),
            (moveInput.y * status.MoveSpeed * Time.fixedDeltaTime)
        );
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
