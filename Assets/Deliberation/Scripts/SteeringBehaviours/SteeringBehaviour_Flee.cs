using UnityEngine;


public class SteeringBehaviour_Flee : SteeringBehaviour
{
    public Transform m_FleeTarget;
    public float m_FleeRadius;

    public override Vector2 CalculateForce()
    {
        if(m_FleeTarget != null)
        {
            // calcualte distance between you and target pos
            m_DesiredVelocity = ((Vector2)transform.position - (Vector2)m_FleeTarget.position);

            Vector2 distance = ((Vector2)transform.position - (Vector2)m_FleeTarget.position);

            // normalise your desired velocity then multiply it by the max speed of the AI agent
            m_DesiredVelocity = Maths.Normalise(m_DesiredVelocity);
            m_DesiredVelocity = m_DesiredVelocity * m_Manager.m_Entity.m_MaxSpeed;

            // calculate steering to be your [desired velocity - AI agents velocity]
            m_Steering = m_DesiredVelocity - m_Manager.m_Entity.m_Velocity;

            // Lerp between 2 values so [Weight and 0] 
            // return the normalised steering vector that has been multipled by your weight
            m_Steering = Maths.Normalise(m_Steering);
            return m_Steering * Mathf.Lerp(m_Weight, 0, Mathf.Min(Maths.Magnitude(distance), m_FleeRadius) / m_FleeRadius); 
        }
        else
        {
            Debug.Log("Flee Target is Dead");
            return m_Steering;
        }
    }


    protected override void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            if (m_Debug_ShowDebugLines && m_Active && m_Manager.m_Entity)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, m_FleeRadius);

                base.OnDrawGizmosSelected();
            }
        }
    }
}
