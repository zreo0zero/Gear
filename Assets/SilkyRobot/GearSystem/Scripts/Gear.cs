using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SilkyRobot.GearSystem
{

    public class Gear : GearComponent
    {
        [Tooltip("set true for stacked gears")]
        [SerializeField] bool stacked;
        [Min(1)] public int teeth = 14;

        protected override float CalculateAngle(GearComponent driver)
        {
            if (driver != null && driver.enabled )
            {
                if(driver.GetTeethCount() > 0 && !stacked)
                    return -(driver.Angle * (driver.GetTeethCount() / (float)GetTeethCount()));
                else
                    return driver.Angle;
            }

            return m_angle;
        }

        public override int GetTeethCount() => teeth;

    }

}


