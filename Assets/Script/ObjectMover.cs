using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    // ����� �� �迭
    public Pin[] pins;
    public Pin startPin;
    // ����ĳ���ÿ� ����� �� ���̾� ����ũ
    int layerMask_plan = 1 << 9;
    int layerMask_object = 1 << 8;
    int layerMask_pin = 1 << 10;

    void Update()
    {
        // �巡�� ���� ������Ʈ�� ���� ���
        if (pickObject != null)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            // ����ĳ��Ʈ�Ͽ� ���콺 ��ġ�� �ִ� ������Ʈ �˻�
            if (Physics.Raycast(ray, out hit, 10000, layerMask_plan))
            {
                // ������Ʈ�� ��ġ�� ����ĳ��Ʈ�� ������ ��ġ�� ����
                pickObject.transform.position = hit.point;
            }
        }
    }

    public Camera camera;  // ī�޶� ����
    public GameObject pickObject;  // ���� �巡�� ���� ������Ʈ
    Vector3 pickPosition;  // �巡�� ������ ���� ������Ʈ ��ġ
    Gear pickGear;  // ���� �巡�� ���� ��� ������Ʈ ����
    Pin pickPin;  // ���� �巡�� ���� ����� �� ����

    public void Pick()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        // ����ĳ��Ʈ�Ͽ� ���콺 ��ġ�� �ִ� ������Ʈ �˻�
        if (Physics.Raycast(ray, out hit, 10000, layerMask_object))
        {
            // ������Ʈ�� ���� �� �ƴ� ��쿡�� �巡�׸� ����
            if (!hit.collider.GetComponent<Gear>().startGear)
            {
                pickObject = hit.collider.gameObject;
                pickPosition = pickObject.transform.position;
                pickGear = pickObject.GetComponent<Gear>();
                pickPin = pickObject.GetComponent<Gear>().pin;
                pickObject.GetComponent<Gear>().pin.gear = null;
                pickObject.GetComponent<Gear>().pin = null;
            }
        }
    }

    Pin hitPin;  // ����� ��ġ�� ��

    public void Realse()
    {
        RaycastHit hit;

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 10000, layerMask_pin))
        {
            hitPin = hit.transform.GetComponent<Pin>();
        }

        // ����ĳ��Ʈ�� �ɿ� �浹�ϰ� �ش� �ɿ� ����� �� ���� ���
        if (Physics.Raycast(ray, out hit, 10000, layerMask_pin) && hitPin.gear == null)
        {
            pickObject.transform.position = hit.transform.position;
            hitPin.gear = pickObject.transform.GetComponent<Gear>();
            pickObject.GetComponent<Gear>().pin = hitPin;
        }
        else
        {
            // ������ ��ġ�� ������Ʈ�� �ǵ���
            pickObject.transform.position = pickPosition;
            if (pickObject.GetComponent<Gear>().pin != null)
            {
                pickObject.GetComponent<Gear>().pin.gear = pickGear;
                pickObject.GetComponent<Gear>().pin = pickPin;
            }
        }

        pickObject = null;

        RotateGear();
    }

    void RotateGear()
    {
        startPin.checkPin = true;
        for (int i = 0; i < pins.Length; i++)
        {
            pins[i].checkPin = false;
            if (pins[i].gear != null)
            {
                pins[i].gear.GetComponent<Animator>().enabled = false;
            }
        }
        //startPin.RotateGear(true);  // ���� �ּ� ó���� ȸ�� ����
    }
}
