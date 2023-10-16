using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Example : MonoBehaviour        //타이머 스크립트
{
    bool watch_active;                          
    public TextMeshProUGUI timerText;

    public float timer = 0;

    public GameObject fail;

    public void StopTimer()
    {
        watch_active = false;
    }

    private void Start()
    {
        watch_active = true;
        timerText.text = ((int)timer / 60 % 60).ToString("D2") + ":" + ((int)timer % 60).ToString("D2");

    }


    private void Update()
    {
        Click();
    }

    void Click()
    {
        if (watch_active)
        {
            if (timer > 0.00)
            {
                timer -= Time.deltaTime;
                timerText.text = ((int)timer / 60 % 60).ToString("D2") + ":" + ((int)timer % 60).ToString("D2");
            }
            else
            {
                fail.SetActive(true);
            }

        }
    }

    public void Fail()
    {
        fail.SetActive (false);
        timer = 11;
        watch_active = false;
    }
}
