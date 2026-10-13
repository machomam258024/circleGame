using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("등장 설정 (Phase 1)")]
    public Transform targetPosition; 
    public float enterSpeed = 2f;    

    [Header("이동 패턴 설정 (Phase 2)")]
    public float moveSpeed = 2f;     
    public float moveRangeX = 4f;    

    [Header("보스 체력")]
    public int maxHealth = 50;       
    private int currentHealth;

    [Header("공격 설정")]
    public GameObject bossBulletPrefab; // 보스가 쏠 총알 프리팹 (이전에 만든 적 총알 재사용 가능)
    public Transform[] firePoints;       // 총알이 발사될 총구들 (여러 개 등록 가능)
    public float fireRate = 1.5f;        // 공격 주파수 (몇 초마다 쏠 것인가?)

    private Vector2 centerPosition;  
    private bool isHovering = false; 
    private Transform playerTarget;

    void Start()
    {
        currentHealth = maxHealth;

        // 1. 등장 위치(BossTargetPos) 자동 탐색
        GameObject targetObj = GameObject.Find("BossTargetPos");
        if (targetObj != null)
        {
            targetPosition = targetObj.transform;
        }

        // 2. 플레이어 위치 자동 탐색
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTarget = playerObj.transform;
        }
    }

    void Update()
    {
        if (!isHovering)
        {
            // Phase 1: 목표 지점까지 이동
            if (targetPosition != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition.position, enterSpeed * Time.deltaTime);

                if (Vector2.Distance(transform.position, targetPosition.position) < 0.1f)
                {
                    isHovering = true;
                    centerPosition = transform.position; 
                    
                    // ★ 목표 지점에 도착하는 순간 공격 루틴을 시작합니다!
                    StartCoroutine(BossAttackRoutine());
                }
            }
            else
            {
                isHovering = true;
                centerPosition = transform.position;
                StartCoroutine(BossAttackRoutine());
            }
        }
        else
        {
            // Phase 2: 도착 후 좌우 반복 이동 (진자 운동)
            float newX = centerPosition.x + Mathf.Sin(Time.time * moveSpeed) * moveRangeX;
            transform.position = new Vector2(newX, centerPosition.y);
        }
    }

    // 보스 공격 코루틴
    IEnumerator BossAttackRoutine()
    {
        // 보스가 살아있는 동안 무한 반복
        while (currentHealth > 0)
        {
            yield return new WaitForSeconds(fireRate);

            // 총알 프리팹과 총구가 등록되어 있을 때만 발사
            if (bossBulletPrefab != null && firePoints != null && firePoints.Length > 0)
            {
                // 배열에 등록된 모든 총구(FirePoint)에서 동시에 발사합니다!
                foreach (Transform fp in firePoints)
                {
                    GameObject bulletObj = Instantiate(bossBulletPrefab, fp.position, fp.rotation);
                    Bullet bullet = bulletObj.GetComponent<Bullet>();

                    if (bullet != null && playerTarget != null)
                    {
                        // 플레이어를 향하는 방향 벡터 계산 후 총알에 전달
                        Vector2 direction = playerTarget.position - fp.position;
                        bullet.SetDirection(direction);
                    }
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"보스 피격! 남은 체력: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("🎉 보스 처치 성공!");
        
        // ★ GameManager에게 게임 클리어 신호를 보냅니다!
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameClear();
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            if (bullet != null)
            {
                TakeDamage(bullet.damage);
            }
            Destroy(other.gameObject);
        }
    }
}
