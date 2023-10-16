using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject clearPanel;  // ���� Ŭ���� �� ������ �г�

    // ���� Ŭ���� ������ Ȯ���ϴ� �Լ�
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
