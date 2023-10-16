using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace SilkyRobot.GearSystem
{
    public abstract class GearComponent : MonoBehaviour
    {
        [Tooltip("The transform that is being rotated. Should be a child of this GameObject")]
        [SerializeField] protected Transform m_transform;
        [SerializeField] protected float m_angle;
        [SerializeField] protected bool m_invert;
        [SerializeField] protected Axis m_axis = Axis.Y;
        [SerializeField] protected Vector3 m_offset;
        [SerializeField] protected List<GearComponent> m_driven = new();
        public ReadOnlyCollection<GearComponent> Driven => m_driven.AsReadOnly();

        public float Angle => m_angle; 
        protected abstract float CalculateAngle(GearComponent driver);
        public abstract int GetTeethCount();

        void Awake()
        {
            if (m_transform == null)
                m_transform = transform;
        }

        public Transform GetTransform() => m_transform != null ? m_transform : transform;
        public void SetTransform(Transform value) { m_transform = value; }


        public virtual void Evaluate(GearComponent driver)
        {
            m_angle = CalculateAngle(driver);

            Vector3 axis = Vector3.zero;
            switch (m_axis)
            {
                case Axis.X:
                    axis = Vector3.left;
                    break;
                case Axis.Y:
                    axis = Vector3.up;
                    break;
                case Axis.Z:
                    axis = Vector3.forward;
                    break;
                default:
                    break;
            }

            GetTransform().localRotation = Quaternion.Euler((axis * (m_invert ? m_angle * -1 : m_angle)) + m_offset);

            for (int i = 0; i < m_driven.Count; i++)
            {
                if (m_driven[i] != null)
                    m_driven[i].Evaluate(this);
            }
        }

        public virtual void SetAngle(float value)
        {
            m_angle = value;
            Evaluate(null);
        }

        public void AddDrivenComponent(GearComponent driven)
        {
            if (!m_driven.Contains(driven) && !driven.m_driven.Contains(this))
            {
                m_driven.Add(driven);
            }
        }
        /*public void AddDrivenComponent(GearComponent driven)
        {
            if (!m_driven.Contains(driven)) //원본 코드 이거 그대로 쓰면 서로가 서로를 추가하면서 오버플로우 일어남, 한마디로 W된다는 뜻임
                m_driven.Add(driven);
        }*/
        

        public void RemoveDrivenComponent(GearComponent driven)
        {
            if (m_driven.Contains(driven))
                m_driven.Remove(driven);
        }

        public void RemoveAllDriven()
        {
                m_driven.Clear();
        }

        void OnValidate()
        {
            Evaluate(null);
        }

        void OnDrawGizmosSelected()
        { 
            if (m_driven != null && m_driven.Count > 0)
            {
               
                Gizmos.color = Color.yellow;
                Vector3 start = transform.position;
                for (int i = 0; i < m_driven.Count; i++)
                {
                    if (m_driven[i] == null)
                        continue;
                    Vector3 end = m_driven[i].transform.position;
                    Gizmos.DrawLine(start, end);
                    Gizmos.DrawSphere(start, .1f);
                    Gizmos.DrawSphere(end, .1f);
                }
            }
        }

    }

    public enum Axis
    {
        X, Y, Z
    }
}


