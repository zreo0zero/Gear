using UnityEngine;

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
        clearPanel.SetActive(true);
    }
}
