using UnityEngine;


public class SteeringBehaviour_Cohesion : SteeringBehaviour
{
    public float m_CohesionRange;
    
    [Range(1,-1)]
    public float m_FOV;
    public override Vector2 CalculateForce()
    {
        Vector2 AgentPositions = Vector2.zero;
        Vector2 AccumulatedPosition = Vector2.zero;
        Vector2 CohesionForce = Vector2.zero;

        // Circle collider to see which entities are nearby
        Collider2D[] entities = Physics2D.OverlapCircleAll(transform.position, m_CohesionRange);

        // If theres a collision
        if (entities.Length > 0)
        {
            // run for loop to find all overlapping entities
            for (int i = 0; i < entities.Length; i++)
            {
                // for each valid agent, accumulate their pos
                AccumulatedPosition += (Vector2) entities[i].transform.position;

                // all agents detected
                if(entities.Length == i)
                {
                    // divide accumulated pos by total num of agents
                    CohesionForce += AccumulatedPosition / i;


                    // calculate m_DesiredVelocity to be the distance between your m_TargetPosition and current position
                    m_DesiredVelocity = (Vector2)transform.position - CohesionForce;

                    // Then we normalise the m_desiredVelocity and multiply it by the max speed of the agent (m_manager.m_entity.m_Max speed)
                    m_DesiredVelocity = Maths.Normalise(m_DesiredVelocity);
                    m_DesiredVelocity = m_DesiredVelocity * m_Manager.m_Entity.m_MaxSpeed;

                    // calculate your m_Steering to be the m_DesiredVelocity - The AI agaents velocity (m_Manager.m_Entity.m_Velocity)
                    m_Steering = m_DesiredVelocity - m_Manager.m_Entity.m_Velocity;
                    m_Steering = Maths.Normalise(m_Steering);

                    return m_Steering * m_Weight;
                }
            }
        }
        return CohesionForce;
    }
}
