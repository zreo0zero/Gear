using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    // Start is called before the first frame update

    public Pin[] pins;
    public Pin startPin;
    void Start()
    {
        
    }
    int layerMask_plan = 1 << 9;
    int layerMask_object = 1 << 8;

    int layerMask_pin = 1 << 10;
    // Update is called once per frame
    void Update()
    {
        if(pickObject != null)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 10000, layerMask_plan))
            {
                pickObject.transform.position = hit.point;
            }

        }
    }
    public Camera camera;
    public GameObject pickObject;
    Vector3 pickPosition;
    Gear pickGear;
    Pin pickPin;
    public void Pick()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 10000, layerMask_object))
        {
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
    Pin hitPin;
    public void Realse()
    {
        RaycastHit hit;

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 10000, layerMask_pin))
        {
            hitPin = hit.transform.GetComponent<Pin>();
        }
        if (Physics.Raycast(ray, out hit, 10000, layerMask_pin) && hitPin.gear == null)
        {
            pickObject.transform.position = hit.transform.position;
            hitPin.gear = pickObject.transform.GetComponent<Gear>();
            pickObject.GetComponent<Gear>().pin = hitPin;
        }
        else
        {
            pickObject.transform.position = pickPosition;
            if(pickObject.GetComponent<Gear>().pin != null)
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
            if(pins[i].gear != null)
            {
                pins[i].gear.GetComponent<Animator>().enabled = false;
            }
        }
        //startPin.RotateGear(true);
    }
}
