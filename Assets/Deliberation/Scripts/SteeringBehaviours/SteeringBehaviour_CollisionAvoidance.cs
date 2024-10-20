using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;


public class SteeringBehaviour_CollisionAvoidance : SteeringBehaviour
{
    [System.Serializable]
    public struct Feeler
    {
        [Range(0, 360)]
        public float m_Angle;
        public float m_MaxLength;
        public Color m_Colour;
    }

    public Feeler[] m_Feelers;
    Vector2[] m_FeelerVectors;
    float[] m_FeelersLength;

    [SerializeField]
    LayerMask m_FeelerLayerMask;

    private void Start()
    {
        m_FeelersLength = new float[m_Feelers.Length];
        m_FeelerVectors = new Vector2[m_Feelers.Length];
    }

    public override Vector2 CalculateForce()
    {
        // reset steering and desired velocity to 0
        m_Steering = Vector2.zero;
        m_DesiredVelocity = Vector2.zero;

        Vector2 hitPosition = Vector2.zero;

        // set distance float to highest possible value
        float distance = float.MaxValue;

        // Update feelers 
        UpdateFeelers();

        for (int i = 0; i < m_Feelers.Length; i++)
        {
            // Raycast to see if any given feeler has connected
            RaycastHit2D tempHit = Physics2D.Raycast(transform.position, m_FeelerVectors[i], m_FeelersLength[i], m_FeelerLayerMask.value);

            // If raycast has hit 
            if (tempHit)
            {
                Vector2 hitLocation = tempHit.point - (Vector2)transform.position;

                if (Maths.Magnitude(hitLocation) < distance)
                {
                    distance = Maths.Magnitude(hitLocation);

                    // keep hit position - point we flee away from
                    hitPosition = tempHit.point;
                }
            }
        }
        // check if its got a value (null wont work)
        if (hitPosition != Vector2.zero)
        {
            // calcualte distance between you and target pos
            m_DesiredVelocity = (Vector2)transform.position - (Vector2)hitPosition;

            // normalise your desired velocity then multiply it by the max speed of the AI agent
            m_DesiredVelocity = Maths.Normalise(m_DesiredVelocity);
            m_DesiredVelocity = m_DesiredVelocity * m_Manager.m_Entity.m_MaxSpeed;

            // calculate steering to be your [desired velocity - AI agents velocity]
            m_Steering = m_DesiredVelocity - m_Manager.m_Entity.m_Velocity;

            // return the normalised steering vector that has been multipled by your weight
            //m_Steering = Maths.Normalise(m_Steering);

            return m_Steering * m_Weight;
        }

        return Vector2.zero;
    }


    public void UpdateFeelers()
    {
        for (int i = 0; i < m_Feelers.Length; ++i)
        {
            m_FeelersLength[i] = Mathf.Lerp(1, m_Feelers[i].m_MaxLength, Maths.Magnitude(m_Manager.m_Entity.m_Velocity) / m_Manager.m_Entity.m_MaxSpeed);
            m_FeelerVectors[i] = Maths.RotateVector(Maths.Normalise(m_Manager.m_Entity.m_Velocity), m_Feelers[i].m_Angle) * m_FeelersLength[i];
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            if (m_Debug_ShowDebugLines && m_Active && m_Manager.m_Entity)
            {
                for (int i = 0; i < m_Feelers.Length; ++i)
                {
                    Gizmos.color = m_Feelers[i].m_Colour;
                    Gizmos.DrawLine(transform.position, (Vector2)transform.position + m_FeelerVectors[i]);
                }

                base.OnDrawGizmosSelected();
            }
        }
    }
}
