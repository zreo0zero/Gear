using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject clearPanel;  // 게임 클리어 시 보여줄 패널
    private Example watch;

    private void Awake() // 또는 Start() 메서드를 사용할 수도 있습니다
    {
        watch = FindObjectOfType<Example>();
    }


    // 게임 클리어 조건을 확인하는 함수
    public void CheckGameClearCondition(bool isEndGearActive)
    {
        if (isEndGearActive)
        {
            GameClear();
        }
    }

    void GameClear()
    {
        watch.StopTimer();
        StartCoroutine(ShowClearPanelAfterDelay(3.0f));
    }

    IEnumerator ShowClearPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        clearPanel.SetActive(true);
    }
    public void Restart()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // 지금하고 있는 씬의 인덱스를 가져옴
        SceneManager.LoadScene(currentSceneIndex);
    }
}
