using UnityEngine;
using System.Collections;

// 1. 개별 적 한 마리의 스폰 정보를 담는 클래스
[System.Serializable]
public class SpawnUnit
{
    public GameObject enemyPrefab;      // 어떤 적을 스폰할 것인가?
    public Transform spawnPoint;        // 어느 위치에서 스폰할 것인가?
    public Transform[] pathWaypoints;   // 이 적이 따라갈 경로
}

// 2. 한 번에(동시에) 일어날 스폰 이벤트 클래스
[System.Serializable]
public class SpawnEvent
{
    public string name = "Group Spawn";
    
    // ★ 핵심: 이제 단 한 마리가 아니라, 동시에 소환할 적 유닛들을 배열로 받습니다!
    public SpawnUnit[] spawnUnits;      
    
    public float delayAfterSpawn = 1f;  // 이 그룹을 모두 스폰한 뒤 다음 이벤트까지 대기할 시간 (초)
}

// 3. 하나의 웨이브(묶음) 클래스
[System.Serializable]
public class Wave
{
    public string waveName = "Wave 1";
    public SpawnEvent[] spawnEvents;
}

public class SpawnManager : MonoBehaviour
{
   [Header("웨이브 설정")]
    public Wave[] waves;

    public void StartSpawn()
    {
        StartCoroutine(SpawnWavesRoutine());
    }

    IEnumerator SpawnWavesRoutine()
    {
        for (int i = 0; i < waves.Length; i++)
        {
            Debug.Log($"★★★ {waves[i].waveName} 시작! ★★★");
            yield return StartCoroutine(PlayWave(waves[i]));
            
            Debug.Log($"{waves[i].waveName} 클리어! 다음 웨이브 대기 중...");
            yield return new WaitForSeconds(2.5f); 
        }
        Debug.Log("모든 웨이브가 종료되었습니다!");
    }

    IEnumerator PlayWave(Wave wave)
    {
        for (int j = 0; j < wave.spawnEvents.Length; j++)
        {
            SpawnEvent ev = wave.spawnEvents[j];

            // ★ 그룹 내의 모든 적 유닛들을 반복문을 통해 '동시에' 생성합니다.
            if (ev.spawnUnits != null && ev.spawnUnits.Length > 0)
            {
                foreach (SpawnUnit unit in ev.spawnUnits)
                {
                    if (unit.enemyPrefab != null && unit.spawnPoint != null)
                    {
                        // 적 생성
                        GameObject go = Instantiate(unit.enemyPrefab, unit.spawnPoint.position, unit.spawnPoint.rotation);
                        
                        // 경로 주입
                        Enemy enemyComponent = go.GetComponent<Enemy>();
                        if (enemyComponent != null && unit.pathWaypoints != null && unit.pathWaypoints.Length > 0)
                        {
                            enemyComponent.SetPath(unit.pathWaypoints);
                        }
                    }
                }
            }

            // 모든 적이 동시에 소환된 직후, 지정된 시간만큼 대기합니다.
            yield return new WaitForSeconds(ev.delayAfterSpawn);
        }
    }
}