using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SilkyRobot.GearSystem
{
    public class Gear_Motor : MonoBehaviour
    {

        [SerializeField] float m_speed = 20;
        [SerializeField] float m_onDuration = 0;
        [SerializeField] float m_offDuration = 0;
        [SerializeField] bool m_invert;
        [SerializeField] float m_angle;
        [SerializeField] GearComponent m_driven;
        [SerializeField] bool m_useUnscaledTime;
        public GearComponent Driven { get => m_driven; set => m_driven = value; }
        public float Speed { get => m_speed; set => m_speed = value; }
        public float Angle { get => m_angle; set => m_angle = value; }
        public bool Invert { get => m_invert; set => m_invert = value; }
        public bool IsRunning => running;

        float onTimer;
        float offTimer;
        bool running = true;

        const int RESET_ANGLE = 360000;

        void Update()
        {

            if (m_onDuration > 0 && m_offDuration > 0)
            {
                if (running)
                {
                    offTimer = 0;
                    onTimer += GetDeltaTime();
                    running = onTimer < m_onDuration;
                }
                else
                {
                    onTimer = 0;
                    offTimer += GetDeltaTime();
                    running = offTimer >= m_offDuration;
                }
            }
            else
                running = true;

            if (running)
            {
                m_angle += GetDeltaTime() * (m_invert ? -m_speed : m_speed);
                m_angle %= RESET_ANGLE;
            }

            if (m_driven != null)
                m_driven.SetAngle(m_angle);
        }

        float GetDeltaTime() => (m_useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);

        public void SetSpeed(float value)
        {
            m_speed = value;
        }

        public void SetAngle(float value)
        {
            m_angle = value;
        }

        public void SetDurations(float onDuration, float offDuration)
        {
            m_onDuration = onDuration;
            m_offDuration = offDuration;
        }

        public void InvertDirection(bool value)
        {
            m_invert = value;
        }

        public void Restart()
        {
            m_angle = 0;
            running = true;
            onTimer = offTimer = 0;
        }

        void OnValidate()
        {
            if (m_driven != null)
                m_driven.SetAngle(m_angle);
        }
    }



}
