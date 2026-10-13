using UnityEngine;
using System.Collections;

public class EnemyShooter : MonoBehaviour
{
    [Header("발사 설정")]
    public GameObject enemyBulletPrefab; // 적이 쏠 총알 프리팹
    public Transform firePoint;          // 발사 위치
    public float fireRate = 2f;          // 발사 간격 (예: 2초마다 1발)

    private Transform playerTarget; // 플레이어의 위치 정보를 담을 변수

    void Start()
    {
        // 1. 씬에서 "Player" 태그를 가진 오브젝트를 찾아 목표로 설정합니다.
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTarget = playerObj.transform;
        }

        // 2. 일정 시간마다 발사하는 코루틴 시작
        StartCoroutine(ShootRoutine());
    }

    IEnumerator ShootRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireRate);

            // 플레이어가 씬에 살아있을 때만 쏩니다.
            if (playerTarget != null) 
            {
                ShootAtPlayer();
            }
        }
    }

    void ShootAtPlayer()
    {
        if (enemyBulletPrefab != null && firePoint != null)
        {
            // 총알을 생성합니다.
            GameObject bulletObj = Instantiate(enemyBulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bullet = bulletObj.GetComponent<Bullet>();

            if (bullet != null)
            {
                // ★ 수학 공식: (목표의 위치) - (내 위치) = 목표를 향하는 방향 벡터
                Vector2 direction = playerTarget.position - firePoint.position;
                
                // 생성된 총알에게 계산한 방향으로 날아가라고 명령합니다.
                bullet.SetDirection(direction);
            }
        }
    }
}
