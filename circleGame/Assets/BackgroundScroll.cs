using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    [Header("스크롤 설정")]
    public float scrollSpeed = 2f; // 배경이 내려오는 속도
    
    [Tooltip("배경 이미지의 정확한 세로 길이를 입력하세요.")]
    public float backgroundHeight = 10f; // 이 값은 유니티 화면에서 직접 측정해서 넣어야 합니다.

    void Update()
    {
        // 1. 매 프레임마다 배경을 아래로 이동시킵니다.
        transform.Translate(Vector2.down * scrollSpeed * Time.deltaTime);

        // 2. 만약 배경의 Y 좌표가 -(자신의 세로 길이)보다 더 아래로 내려갔다면 화면을 완전히 벗어난 것입니다.
        if (transform.position.y <= -backgroundHeight)
        {
            RepositionBackground();
        }
    }

    void RepositionBackground()
    {
        // 3. 현재 위치에서 (세로 길이의 2배)만큼 위로 끌어올려 맨 뒤로 순간이동시킵니다.
        Vector3 resetPosition = new Vector3(transform.position.x, transform.position.y + backgroundHeight * 2f, 10);
        transform.position = resetPosition;
    }
}
