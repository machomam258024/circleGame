using UnityEngine;
using System.Collections;

public class AutoShooter : MonoBehaviour
{
    [Header("발사 설정")]
    public GameObject bulletPrefab; // 발사할 총알 프리팹
    public Transform firePoint;     // 총알이 생성될 위치
    public float fireRate = 0.5f;   // 발사 간격 (초 단위, 0.5면 1초에 2발)

    void Start()
    {
        // 게임 시작과 동시에 자동 발사 코루틴을 실행합니다.
        StartCoroutine(AutoShootRoutine());
    }

    // 일정한 시간 간격으로 발사를 반복하는 코루틴 함수
    IEnumerator AutoShootRoutine()
    {
        while (true) // 무한 반복
        {
            Shoot();
            // fireRate에 설정된 시간(초)만큼 기다렸다가 다음 줄로 넘어갑니다.
            yield return new WaitForSeconds(fireRate); 
        }
    }

    void Shoot()
    {
        // 총알 프리팹과 발사 위치가 할당되어 있는지 확인 후 생성 (Instantiate)
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}
