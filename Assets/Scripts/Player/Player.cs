using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Camera cam;
    private PlayerInputSet input;
    private PlayerStatus status;
    private PlayerShooter shooter;

    [Header("Graphics")]
    [SerializeField] private SpriteRenderer graphics;
    private float halfWidth;
    private float halfHeight;

    [Header("Movement")]
    [SerializeField] public Vector2 moveInput;
    private bool attackPressed;
    private float slowMoveSpeed = 2f;
    private bool slowMovePressed = false;
    private Coroutine beingTransparencyCo;
    private bool wasSlow; // 1フレーム前の slow 状態

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

        var extents = graphics.bounds.extents; // ワールド座標での半径
        halfWidth = extents.x;
        halfHeight = extents.y;
    }

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        // Pause中
        if (GameManager.Instance.IsPausing)
            return;

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

        // 画面内に収める処理
        if (cam != null)
        {
            // Camera z: -10 Player z:0 なので、距離10とする
            // ViewportToWorldPointには、カメラから見てz方向にどれだけ離れているかを渡す
            // 今回Playerはz=0なので、カメラのz位置のマイナスを渡すだけで良い
            float zDistance = transform.position.z - cam.transform.position.z;

            Vector3 leftBottom = cam.ViewportToWorldPoint(new Vector3(0f, 0f, zDistance));
            Vector3 rightTop = cam.ViewportToWorldPoint(new Vector3(1f, 1f, zDistance));

            next.x = Mathf.Clamp(next.x, leftBottom.x + halfWidth, rightTop.x - halfWidth);
            next.y = Mathf.Clamp(next.y, leftBottom.y + halfHeight, rightTop.y - halfHeight);
        }

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
        input.Player.Pause.started += OnPausePushed;
    }

    private void OnDisable()
    {
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCanceled;
        input.Player.Attack.performed -= OnAttackPerformed;
        input.Player.Attack.canceled -= OnAttackCanceled;
        input.Player.SlowMove.performed -= OnSlowMovePerformed;
        input.Player.SlowMove.canceled -= OnSlowMoveCanceled;
        input.Player.Pause.started -= OnPausePushed;

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

    private void OnPausePushed(InputAction.CallbackContext ctx)
    {
        GameManager.Instance.TogglePausing();
    }


    private float GetMoveSpped()
    {
        if (slowMovePressed)
            return slowMoveSpeed;

        return status.MoveSpeed;

    }

    private void UpdateTransparency(bool isSlow)
    {
        // OFF → ON になった瞬間だけコルーチンを始める
        if (isSlow && !wasSlow)
        {
            if (beingTransparencyCo != null)
            {
                StopCoroutine(beingTransparencyCo);
            }
            beingTransparencyCo = StartCoroutine(BeingTransparencyCo());
        }

        // ON → OFF になった瞬間、コルーチンを停止して元の透明度に戻す
        if (!isSlow && wasSlow)
        {
            var c = graphics.color;
            c.a = 1f;
            graphics.color = c;

            if (beingTransparencyCo != null)
            {
                StopCoroutine(beingTransparencyCo);
                beingTransparencyCo = null;
            }
        }

        wasSlow = isSlow;

    }

    private IEnumerator BeingTransparencyCo()
    {
        float t = 0f;
        float duration = 0.2f;
        float minAlpha = 0.5f;

        while (t < duration && slowMovePressed)
        {
            t += Time.deltaTime;
            var c = graphics.color;
            c.a = Mathf.Lerp(1f, minAlpha, t / duration);
            graphics.color = c;
            yield return null;
        }

        // 最後に 0.5 に寄せておく
        var last = graphics.color;
        last.a = minAlpha;
        graphics.color = last;

        beingTransparencyCo = null;
    }

}
