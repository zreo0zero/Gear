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

        // ���� �� ������ �� �ִ� �����̸�, �ٸ� �� ���� ���°� �ƴ� ��
        if (this.canDrive && otherGear != null && !otherGearSpin.canDrive)
        {
            // ���� ������Ʈ �߰�
            GetComponent<GearComponent>().AddDrivenComponent(otherGear);
            // �ٸ� �� ���� ���·� ����
            otherGearSpin.canDrive = true;
        }

    }

    void OnTriggerExit(Collider other)
    {
        GearComponent otherGear = other.GetComponent<GearComponent>();
        GearSpin otherGearSpin = other.GetComponent<GearSpin>();

        // ���� �� ���� ���̰�, �ٸ� �� ���� ������ ��
        if (this.canDrive && otherGearSpin && otherGearSpin.canDrive)
        {
            // ���� ������Ʈ ����
            GetComponent<GearComponent>().RemoveDrivenComponent(otherGear);
            // �ٸ� �� �񱸵� ���·� ����
            otherGearSpin.canDrive = false;
        }
    }
    
}