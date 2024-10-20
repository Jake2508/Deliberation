using UnityEngine;


public class SteeringBehaviour_Pursuit : SteeringBehaviour
{
    public MovingEntity m_PursuingEntity;

    public override Vector2 CalculateForce()
    {
        // calculate the distance between the AI agent and pursuing entity
        Vector2 distance = m_Manager.m_Entity.transform.position - m_PursuingEntity.transform.position;

        // combine their speeds together in a float - Magnitude of a velocity vector is the speed
        float CombinedSpeed = Maths.Magnitude(m_DesiredVelocity) + Maths.Magnitude(m_PursuingEntity.m_Velocity);

        // calculate the prediction time which is the magnitude of your newly created distance vector divided by your combined speed float
        float PredictionTime = Maths.Magnitude(distance) / CombinedSpeed;

        // calculate m_DesiredVelocity to be the distance between your m_TargetPosition and current position
        m_DesiredVelocity = ((((Vector2)m_PursuingEntity.transform.position + ((Vector2)m_PursuingEntity.m_Velocity) * PredictionTime)) - (Vector2)transform.position);

        // Then we normalise the m_desiredVelocity and multiply it by the max speed of the agent (m_manager.m_entity.m_Max speed)
        m_DesiredVelocity = Maths.Normalise(m_DesiredVelocity);
        m_DesiredVelocity = m_DesiredVelocity * m_Manager.m_Entity.m_MaxSpeed;

        // calculate your m_Steering to be the m_DesiredVelocity - The AI agaents velocity (m_Manager.m_Entity.m_Velocity)
        m_Steering = m_DesiredVelocity - m_Manager.m_Entity.m_Velocity;
        m_Steering = Maths.Normalise(m_Steering);

        return m_Steering * m_Weight;
    }
}
