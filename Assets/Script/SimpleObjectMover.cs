using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObjectMover : MonoBehaviour
{
    // Pin들의 배열
    public Pin[] pins;

    // 각 레이어에 대한 레이어 마스크
    int layerMask_plan = 1 << 9;     // 지면 레이어 마스크
    int layerMask_object = 1 << 8;   // 오브젝트 레이어 마스크
    int layerMask_pin = 1 << 10;     // 핀 레이어 마스크

    void Update()
    {
        // 만약 pickedObject가 null이 아니면 (즉, 드래그 중이면)
        if (pickedObject != null)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            // 지면에 레이를 발사하여 물체의 위치를 결정
            if (Physics.Raycast(ray, out hit, 10000, layerMask_plan))
            {
                pickedObject.transform.position = hit.point;
            }
        }
    }

    public Camera camera;                // 카메라 참조
    public GameObject pickedObject;      // 현재 드래그 중인 오브젝트
    Vector3 initialPosition;             // 드래그 시작 위치
    Gear pickedGear;                     // 드래그 중인 기어 참조
    Pin pickedPin;                       // 드래그 중인 기어의 핀 참조

    public void StartDrag()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        // 드래그 시작할 오브젝트 검색
        if (Physics.Raycast(ray, out hit, 10000, layerMask_object))
        {
            // 만약 오브젝트가 시작 기어가 아니면 드래그 시작
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

        // 드랍할 위치의 핀 검색
        if (Physics.Raycast(ray, out hit, 10000, layerMask_pin))
        {
            Pin hitPin = hit.transform.GetComponent<Pin>();
            // 핀에 기어가 연결되어 있지 않으면 핀에 기어 연결
            if (hitPin && hitPin.gear == null)
            {
                pickedObject.transform.position = hit.transform.position;
                hitPin.gear = pickedObject.GetComponent<Gear>();
                pickedObject.GetComponent<Gear>().pin = hitPin;
            }
            else
            {
                // 아니면 원래 위치로 되돌림
                pickedObject.transform.position = initialPosition;
                pickedGear.pin = pickedPin;
                if (pickedPin)
                    pickedPin.gear = pickedGear;
            }
        }
        else
        {
            // 드랍 위치가 핀이 아니면 원래 위치로 되돌림
            pickedObject.transform.position = initialPosition;
            pickedGear.pin = pickedPin;
            if (pickedPin)
                pickedPin.gear = pickedGear;
        }

        pickedObject = null;
    }
}
