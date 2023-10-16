using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject clearPanel;  // 게임 클리어 시 보여줄 패널

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
        StartCoroutine(ShowClearPanelAfterDelay(3.0f));
    }

    IEnumerator ShowClearPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        clearPanel.SetActive(true);
    }

}
