using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("총알 설정")]
    public float speed = 10f;       
    public float lifeTime = 3f;     
    public int damage = 1; // ★ 총알의 데미지 수치를 추가합니다 (기본값 1)

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
}
