using UnityEngine;
using UnityEngine.SceneManagement; // 씬을 다시 불러오기 위해 필요합니다!

public class GameManager : MonoBehaviour
{
    // ★ 싱글톤 인스턴스 (어디서든 GameManager.Instance로 접근 가능)
    public static GameManager Instance;

    [Header("UI 패널 연결")]
    public GameObject startUI;      // 시작 버튼 UI
    public GameObject gameOverUI;   // 게임 오버 패널 (재시작 버튼 포함)
    public GameObject gameClearUI;  // 게임 클리어 패널 (재시작 버튼 포함)

    [Header("게임 오브젝트 제어")]
    public SpawnManager spawnManager;       
    public MonoBehaviour playerController; 
    public AutoShooter autoShooter;        
    public MonoBehaviour[] backgroundScrollers; 

    void Awake()
    {
        // 싱글톤 설정
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 게임 시작 전 초기 상태 세팅
        if (playerController != null) playerController.enabled = false;
        if (autoShooter != null) autoShooter.enabled = false;
        
        if (backgroundScrollers != null)
        {
            foreach (var scroller in backgroundScrollers)
                if (scroller != null) scroller.enabled = false;
        }

        // UI 상태 초기화
        if (startUI != null) startUI.SetActive(true);
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (gameClearUI != null) gameClearUI.SetActive(false);

        Time.timeScale = 1f; // 게임 속도 정상화
    }

    // [게임 시작] 버튼을 눌렀을 때
    public void OnClickGameStart()
    {
        if (startUI != null) startUI.SetActive(false);
        if (spawnManager != null) spawnManager.StartSpawn();

        if (playerController != null) playerController.enabled = true;
        if (autoShooter != null) autoShooter.enabled = true;
        
        if (backgroundScrollers != null)
        {
            foreach (var scroller in backgroundScrollers)
                if (scroller != null) scroller.enabled = true;
        }

        Debug.Log("🎮 게임 시작!");
    }

    // 💀 게임 오버 처리 (플레이어 사망 시 호출)
    public void GameOver()
    {
        Debug.Log("💀 게임 오버!");
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
        Time.timeScale = 0f; // 게임 시간 정지 (움직임 일시정지)
    }

    // 🎉 게임 클리어 처리 (보스 처치 시 호출)
    public void GameClear()
    {
        Debug.Log("🎉 게임 클리어!");
        if (gameClearUI != null)
        {
            gameClearUI.SetActive(true);
        }
        Time.timeScale = 0f; // 게임 시간 정지
    }

    // 🔄 [재시작] 버튼을 눌렀을 때 호출될 함수
    public void RestartGame()
    {
        Time.timeScale = 1f; // 멈췄던 시간을 반드시 원래대로 되돌려야 합니다!
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 현재 씬을 다시 로드합니다.
    }
}