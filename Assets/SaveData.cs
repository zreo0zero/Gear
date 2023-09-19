using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetClue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FindClue(int clueNum)
    {
        PlayerPrefs.SetInt("clue_" + clueNum.ToString(), 1);
    }
    public GameObject[] ClueList;
    void GetClue()
    {
        for (int i = 0; i < ClueList.Length; i++)
        {
            if(PlayerPrefs.GetInt("clue_" + i.ToString(), 0) == 1)
            {
                ClueList[i].SetActive(true);
            }
        }
    }

    public void ClearData()
    {
        PlayerPrefs.DeleteAll();
    }
}
