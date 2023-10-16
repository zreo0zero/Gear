using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public bool startGear;
    public Pin pin;
    // Start is called before the first frame update
    void Start()
    {
        if(pin != null)
        {
            startGear = pin.startPin;
        }
    }
}
