using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SilkyRobot.GearSystem
{

    public class Gear_Axle : GearComponent
    {

        protected override float CalculateAngle(GearComponent driver)
        {
            if (driver != null && driver.enabled)
                return driver.Angle;

            return m_angle;
        }

        public override int GetTeethCount() => 0;


    }
}
