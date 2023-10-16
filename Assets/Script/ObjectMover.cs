using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    // 사용할 핀 배열
    public Pin[] pins;
    public Pin startPin;
    // 레이캐스팅에 사용할 각 레이어 마스크
    int layerMask_plan = 1 << 9;
    int layerMask_object = 1 << 8;
    int layerMask_pin = 1 << 10;

    void Update()
    {
        // 드래그 중인 오브젝트가 있을 경우
        if (pickObject != null)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            // 레이캐스트하여 마우스 위치에 있는 오브젝트 검색
            if (Physics.Raycast(ray, out hit, 10000, layerMask_plan))
            {
                // 오브젝트의 위치를 레이캐스트가 검출한 위치로 변경
                pickObject.transform.position = hit.point;
            }
        }
    }

    public Camera camera;  // 카메라 참조
    public GameObject pickObject;  // 현재 드래그 중인 오브젝트
    Vector3 pickPosition;  // 드래그 시작할 때의 오브젝트 위치
    Gear pickGear;  // 현재 드래그 중인 기어 컴포넌트 참조
    Pin pickPin;  // 현재 드래그 중인 기어의 핀 참조

    public void Pick()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        // 레이캐스트하여 마우스 위치에 있는 오브젝트 검색
        if (Physics.Raycast(ray, out hit, 10000, layerMask_object))
        {
            // 오브젝트가 시작 기어가 아닌 경우에만 드래그를 시작
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

    Pin hitPin;  // 드랍할 위치의 핀

    public void Realse()
    {
        RaycastHit hit;

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 10000, layerMask_pin))
        {
            hitPin = hit.transform.GetComponent<Pin>();
        }

        // 레이캐스트가 핀에 충돌하고 해당 핀에 연결된 기어가 없을 경우
        if (Physics.Raycast(ray, out hit, 10000, layerMask_pin) && hitPin.gear == null)
        {
            pickObject.transform.position = hit.transform.position;
            hitPin.gear = pickObject.transform.GetComponent<Gear>();
            pickObject.GetComponent<Gear>().pin = hitPin;
        }
        else
        {
            // 원래의 위치로 오브젝트를 되돌림
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
        //startPin.RotateGear(true);  // 현재 주석 처리된 회전 로직
    }
}
