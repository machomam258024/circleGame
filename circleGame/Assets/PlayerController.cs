using UnityEngine;

public class PlayerController : MonoBehaviour
{
   [Header("이동 설정")]
    public float moveSpeed = 5f;

    [Header("화면 경계 (이동 제한)")]
    public Vector2 minBounds = new Vector2(-8f, -4.5f);
    public Vector2 maxBounds = new Vector2(8f, 4.5f);

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. 입력 받기
        // GetAxisRaw를 사용하여 입력 시 즉각적인 반응을 이끌어냅니다.
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // 대각선 이동 시 속도 보정 (normalized를 사용하면 대각선 이동 시 1.4배 빨라지는 현상을 막습니다)
        if (movement.sqrMagnitude > 1)
        {
            movement = movement.normalized;
        }
    }

    void FixedUpdate()
    {
        // 2. 물리 이동 처리
        rb.linearVelocity = movement * moveSpeed;
        
        // 3. 화면 경계 제한
        LimitMovement();
    }

    void LimitMovement()
    {
        Vector2 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
        pos.y = Mathf.Clamp(pos.y, minBounds.y, maxBounds.y);
        transform.position = pos;
    }
}