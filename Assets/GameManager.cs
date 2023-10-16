using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public GameObject clearPanel;  // ���� Ŭ���� �� ������ �г�
    private Example watch;

    private void Awake() // �Ǵ� Start() �޼��带 ����� ���� �ֽ��ϴ�.
    {
        watch = FindObjectOfType<Example>();
    }


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
        watch.StopTimer();
        StartCoroutine(ShowClearPanelAfterDelay(3.0f));
    }

    IEnumerator ShowClearPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        clearPanel.SetActive(true);
    }

}
