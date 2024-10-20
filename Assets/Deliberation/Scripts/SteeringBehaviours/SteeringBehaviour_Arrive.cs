using UnityEngine;


public class SteeringBehaviour_Arrive : SteeringBehaviour
{
    public Vector2 m_TargetPosition;
    public float m_SlowingRadius; 


    public override Vector2 CalculateForce()
    {
        // calculate m_DesiredVelocity to be the distance between your m_TargetPosition and current position
        m_DesiredVelocity = m_TargetPosition - (Vector2)transform.position;

        Vector2 distance = m_TargetPosition - (Vector2)transform.position;

        // Then we normalise the m_desiredVelocity and multiply it by the max speed of the agent (m_manager.m_entity.m_Max speed)
        m_DesiredVelocity = Maths.Normalise(m_DesiredVelocity);
        m_DesiredVelocity = m_DesiredVelocity * m_Manager.m_Entity.m_MaxSpeed;


        // calculate your m_Steering to be the m_DesiredVelocity - The AI agaents velocity (m_Manager.m_Entity.m_Velocity)
        m_Steering = m_DesiredVelocity - m_Manager.m_Entity.m_Velocity;
        m_Steering = Maths.Normalise(m_Steering);


        // see if distacne from target is less than or equal to slowing radius
        if(Maths.Magnitude(distance) < m_SlowingRadius && Maths.Magnitude(distance) > 0.3f)
        {
            return m_Steering * Mathf.Lerp(m_Weight, 0, Mathf.Min(Maths.Magnitude(distance), m_SlowingRadius / m_SlowingRadius));

        }
        else
        {
            return m_Steering * m_Weight;
        }

    }
}
