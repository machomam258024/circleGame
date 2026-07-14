using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5f;
    private Transform[] waypoints; 
    private int currentWaypointIndex = 0;

    [Header("체력 설정")]
    public int maxHealth = 3;  // ★ 적의 최대 체력 (인스펙터에서 수정 가능)
    private int currentHealth; // 현재 체력

    void Start()
    {
        // ★ 게임이 시작되면 현재 체력을 최대 체력으로 꽉 채웁니다.
        currentHealth = maxHealth;
    }

    public void SetPath(Transform[] path)
    {
        waypoints = path;
        currentWaypointIndex = 0;
    }

    void Update()
    {
        // 경로 이동 로직 (기존과 동일)
        if (waypoints == null || waypoints.Length == 0) return;

        if (currentWaypointIndex < waypoints.Length)
        {
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            transform.position = Vector2.MoveTowards(
                transform.position, 
                targetWaypoint.position, 
                moveSpeed * Time.deltaTime
            );

            if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                currentWaypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    // ★ 데미지를 받는 함수를 따로 만듭니다. (확장성을 위해 분리)
    public void TakeDamage(int damageAmount)
    {
        // 현재 체력에서 데미지 가감
        currentHealth -= damageAmount;
        
        // 유니티 콘솔(Console) 창에서 실시간 체력 변화를 확인하기 위한 로그
        Debug.Log($"{gameObject.name} 피격! 남은 체력: {currentHealth}/{maxHealth}");

        // 체력이 0 이하가 되면 사망 처리
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // ★ 사망 처리 함수
    void Die()
    {
        // 나중에 여기에 폭발 이펙트 생성이나 점수 획득 코드가 들어갑니다.
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. 플레이어의 총알에 맞았을 때
        if (other.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);
            }
            // 부딪힌 총알 삭제
            Destroy(other.gameObject);
        }
        
        // 2. 플레이어 본체와 직접 부딪혔을 때 (몸통 박치기)
        else if (other.CompareTag("Player"))
        {
            // 부딪힌 오브젝트(플레이어)에서 PlayerHealth 스크립트를 가져옵니다.
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            
            if (playerHealth != null)
            {
                // 플레이어에게 1의 데미지를 입힙니다.
                playerHealth.TakeDamage(19);
            }

            // 적 기체는 플레이어와 부딪히면 파괴되도록, 자신의 남은 체력만큼 데미지를 입힙니다.
            TakeDamage(maxHealth); 
        }
    }
}