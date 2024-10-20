using UnityEngine;

public class SteeringBehaviour_Seek : SteeringBehaviour
{
    public Vector2 m_TargetPosition;

    public override Vector2 CalculateForce()
    {
        // calculate m_DesiredVelocity to be the distance between your m_TargetPosition and current position
        m_DesiredVelocity = m_TargetPosition - (Vector2)transform.position;

        // Then we normalise the m_desiredVelocity and multiply it by the max speed of the agent (m_manager.m_entity.m_Max speed)
        m_DesiredVelocity = Maths.Normalise(m_DesiredVelocity);
        m_DesiredVelocity = m_DesiredVelocity * m_Manager.m_Entity.m_MaxSpeed;

        // calculate your m_Steering to be the m_DesiredVelocity - The AI agaents velocity (m_Manager.m_Entity.m_Velocity)
        m_Steering = m_DesiredVelocity - m_Manager.m_Entity.m_Velocity;
        m_Steering = Maths.Normalise(m_Steering);

        if(m_TargetPosition == (Vector2) transform.position)
        {
            return Vector2.zero;
        }

        return m_Steering * m_Weight;
    }
}
