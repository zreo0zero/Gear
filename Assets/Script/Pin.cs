using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
    public bool startPin;
    public bool clock;
    public Pin[] conectpins;
    public Gear gear;
    public bool checkPin;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RotateGear(bool OnOff)
    {
        for (int i = 0; i < conectpins.Length; i++)
        {
            if(conectpins[i] != null)
            {
                if(conectpins[i].gear != null && !conectpins[i].checkPin)
                {
                    conectpins[i].gear.GetComponent<Animator>().enabled = OnOff;
                    conectpins[i].checkPin = true;
                    for (int j = 0; j < conectpins[i].conectpins.Length; j++)
                    {
                        conectpins[i].conectpins[j].RotateGear(true);
                    }
                }
            }
        }
    }


}
