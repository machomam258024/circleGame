using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
   [Header("플레이어 체력 설정")]
    public int maxHealth = 3;  // 최대 체력
    private int currentHealth; // 현재 체력

    void Start()
    {
        // 게임 시작 시 체력을 꽉 채워줍니다.
        currentHealth = maxHealth;
    }

    // 데미지를 받는 함수
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        
        Debug.Log($"🚨 플레이어 피격! 남은 체력: {currentHealth}/{maxHealth}");

        // 체력이 0 이하가 되면 사망 처리
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 부딪힌 오브젝트의 태그가 "적의 총알"인지 확인합니다.
        if (other.CompareTag("EnemyBullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);
            }
            
            // 맞은 총알은 삭제합니다.
            Destroy(other.gameObject);
        }
    }

    void Die()
    {
        Debug.Log("플레이어 사망...");

        // ★ GameManager에게 게임 오버 신호를 보냅니다!
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }

        Destroy(gameObject); // 플레이어 기체 파괴
    }
}
