using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerInputSet input;
    private PlayerStatus status;
    private PlayerShooter shooter;

    [SerializeField] private SpriteRenderer graphics;

    [Header("Movement")]
    [SerializeField] public Vector2 moveInput;
    private bool attackPressed;
    private float slowMoveSpeed = 2f;
    private bool slowMovePressed = false;
    private Coroutine beingTransparencyCo;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = new PlayerInputSet();
        status = GetComponent<PlayerStatus>();
        shooter = GetComponent<PlayerShooter>();

        if (graphics == null)
        {
            Debug.LogWarning("graphicsが未取得だったため、コード側で割り当てました。");
            graphics = GetComponentInChildren<SpriteRenderer>();
        }

    }

    private void Update()
    {
        // ステージクリアしたら、弾だけ打たないようにする
        if (GameManager.Instance.IsStageClear)
            return;

        if (attackPressed)
            shooter.Fire();
    }


    private void FixedUpdate()
    {
        var finalMoveSpeed = GetMoveSpped();
        UpdateTransparency(slowMovePressed);

        // 上下左右移動
        var next = rb.position + new Vector2(
            (moveInput.x * finalMoveSpeed * Time.fixedDeltaTime),
            (moveInput.y * finalMoveSpeed * Time.fixedDeltaTime)
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
        input.Player.SlowMove.performed += OnSlowMovePerformed;
        input.Player.SlowMove.canceled += OnSlowMoveCanceled;
    }

    private void OnDisable()
    {
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCanceled;
        input.Player.Attack.performed -= OnAttackPerformed;
        input.Player.Attack.canceled -= OnAttackCanceled;
        input.Player.SlowMove.performed -= OnSlowMovePerformed;
        input.Player.SlowMove.canceled -= OnSlowMoveCanceled;

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

    private void OnSlowMovePerformed(InputAction.CallbackContext ctx)
    {
        slowMovePressed = true;
    }

    private void OnSlowMoveCanceled(InputAction.CallbackContext ctx)
    {
        slowMovePressed = false;
    }


    private float GetMoveSpped()
    {
        if (slowMovePressed)
            return slowMoveSpeed;

        return status.MoveSpeed;

    }

    private void UpdateTransparency(bool isSlow)
    {
        if (isSlow)
        {
            if (beingTransparencyCo != null)
            {
                StopCoroutine(beingTransparencyCo);
            }
            beingTransparencyCo = StartCoroutine(BeingTransparencyCo());
        }
        else
        {
            var c = graphics.color;
            c.a = 1f;
            graphics.color = c;

            // 透明度を戻すとき、Coroutineは使わないので切っておく
            if (beingTransparencyCo != null)
            {
                StopCoroutine(beingTransparencyCo);
                beingTransparencyCo = null;
            }
        }
    }

    private IEnumerator BeingTransparencyCo()
    {
        float t = 0f;
        float duration = .2f;

        while (t < duration && slowMovePressed)
        {
            t += Time.deltaTime;
            var c = graphics.color;
            c.a = Mathf.Lerp(1f, 0.5f, t / duration);
            graphics.color = c;
            yield return null;
        }
    }

}
