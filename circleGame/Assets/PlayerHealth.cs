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

    void Die()
    {
        Debug.Log("💀 플레이어 사망! 게임 오버!");
        
        // 당장은 플레이어 오브젝트를 파괴하도록 하지만, 
        // 나중에는 파괴 대신 '게임 오버 화면 띄우기'를 여기에 연결하면 됩니다.
        Destroy(gameObject);
    }
}
