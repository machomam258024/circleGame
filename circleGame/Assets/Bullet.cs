using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("총알 설정")]
    public float speed = 10f;       
    public float lifeTime = 3f;     
    public int damage = 1; // ★ 총알의 데미지 수치를 추가합니다 (기본값 1)
    private Vector2 moveDirection = Vector2.up;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void SetDirection(Vector2 newDirection)
    {
        // 방향 벡터를 길이 1짜리(normalized)로 만들어 일정한 속도를 보장합니다.
        moveDirection = newDirection.normalized;
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }
}
