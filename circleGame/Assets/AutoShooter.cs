using UnityEngine;
using System.Collections;

public class AutoShooter : MonoBehaviour
{
    [Header("발사 설정")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.2f;

    private Coroutine shootCoroutine;

    // ★ 1. 기존의 Start() 대신 OnEnable()을 사용합니다!
    // 스크립트가 켜질 때(enabled = true) 자동으로 발사 루프가 시작됩니다.
    void OnEnable()
    {
        // 혹시 기존에 도는 코루틴이 있다면 방지 차원에서 멈추고 새로 시작
        if (shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
        }
        shootCoroutine = StartCoroutine(ShootRoutine());
    }

    // ★ 2. 스크립트가 꺼질 때(enabled = false) 발사 루프를 완벽히 멈춥니다.
    void OnDisable()
    {
        if (shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
            shootCoroutine = null;
        }
    }

    // 발사 코루틴 루프
    IEnumerator ShootRoutine()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(fireRate);
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}
