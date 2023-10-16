using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObjectMover : MonoBehaviour
{
    // Pin���� �迭
    public Pin[] pins;

    // �� ���̾ ���� ���̾� ����ũ
    int layerMask_plan = 1 << 9;     // ���� ���̾� ����ũ
    int layerMask_object = 1 << 8;   // ������Ʈ ���̾� ����ũ
    int layerMask_pin = 1 << 10;     // �� ���̾� ����ũ

    void Update()
    {
        // ���� pickedObject�� null�� �ƴϸ� (��, �巡�� ���̸�)
        if (pickedObject != null)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            // ���鿡 ���̸� �߻��Ͽ� ��ü�� ��ġ�� ����
            if (Physics.Raycast(ray, out hit, 10000, layerMask_plan))
            {
                pickedObject.transform.position = hit.point;
            }
        }
    }

    public Camera camera;                // ī�޶� ����
    public GameObject pickedObject;      // ���� �巡�� ���� ������Ʈ
    Vector3 initialPosition;             // �巡�� ���� ��ġ
    Gear pickedGear;                     // �巡�� ���� ��� ����
    Pin pickedPin;                       // �巡�� ���� ����� �� ����

    public void StartDrag()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        // �巡�� ������ ������Ʈ �˻�
        if (Physics.Raycast(ray, out hit, 10000, layerMask_object))
        {
            // ���� ������Ʈ�� ���� �� �ƴϸ� �巡�� ����
            if (!hit.collider.GetComponent<Gear>().startGear)
            {
                pickedObject = hit.collider.gameObject;
                initialPosition = pickedObject.transform.position;
                pickedGear = pickedObject.GetComponent<Gear>();
                pickedPin = pickedGear.pin;
                pickedPin.gear = null;
                pickedGear.pin = null;
            }
        }
    }

    public void EndDrag()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        // ����� ��ġ�� �� �˻�
        if (Physics.Raycast(ray, out hit, 10000, layerMask_pin))
        {
            Pin hitPin = hit.transform.GetComponent<Pin>();
            // �ɿ� �� ����Ǿ� ���� ������ �ɿ� ��� ����
            if (hitPin && hitPin.gear == null)
            {
                pickedObject.transform.position = hit.transform.position;
                hitPin.gear = pickedObject.GetComponent<Gear>();
                pickedObject.GetComponent<Gear>().pin = hitPin;
            }
            else
            {
                // �ƴϸ� ���� ��ġ�� �ǵ���
                pickedObject.transform.position = initialPosition;
                pickedGear.pin = pickedPin;
                if (pickedPin)
                    pickedPin.gear = pickedGear;
            }
        }
        else
        {
            // ��� ��ġ�� ���� �ƴϸ� ���� ��ġ�� �ǵ���
            pickedObject.transform.position = initialPosition;
            pickedGear.pin = pickedPin;
            if (pickedPin)
                pickedPin.gear = pickedGear;
        }

        pickedObject = null;
    }
}
