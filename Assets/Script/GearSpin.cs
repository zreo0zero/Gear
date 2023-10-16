using SilkyRobot.GearSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearSpin : MonoBehaviour
{
    [SerializeField] private bool canDrive = false;
    [SerializeField] private bool endGear = false;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (endGear&&canDrive)
        {
            gameManager.CheckGameClearCondition(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        GearComponent otherGear = other.GetComponent<GearComponent>();
        GearSpin otherGearSpin = other.GetComponent<GearSpin>();

        // 현재 기어가 구동할 수 있는 상태이며, 다른 기어가 구동 상태가 아닐 때
        if (this.canDrive && otherGear != null && !otherGearSpin.canDrive)
        {
            // 구동 컴포넌트 추가
            GetComponent<GearComponent>().AddDrivenComponent(otherGear);
            // 다른 기어를 구동 상태로 설정
            otherGearSpin.canDrive = true;
        }

    }

    void OnTriggerExit(Collider other)
    {
        GearComponent otherGear = other.GetComponent<GearComponent>();
        GearSpin otherGearSpin = other.GetComponent<GearSpin>();

        // 현재 기어가 구동 중이고, 다른 기어가 구동 상태일 때
        if (this.canDrive && otherGearSpin && otherGearSpin.canDrive)
        {
            // 구동 컴포넌트 제거
            GetComponent<GearComponent>().RemoveDrivenComponent(otherGear);
            // 다른 기어를 비구동 상태로 설정
            otherGearSpin.canDrive = false;
        }
    }
    
}