using UnityEngine;


public class SteeringBehaviour_Alignment : SteeringBehaviour
{
    public float m_AlignmentRange;

    
    [Range(1,-1)]
    public float m_FOV;
    public override Vector2 CalculateForce()
    {
        Vector2 AlignmentForce = Vector2.zero;
        Vector2 facingDirection = Vector2.zero;
        Vector2 accumulatedHeading = Vector2.zero;

        // create layer mask, that only checks enemy AI layer
        LayerMask mask = LayerMask.GetMask("GroundAI");

        // Circle collider to see which entities are nearby
        Collider2D[] entities = Physics2D.OverlapCircleAll(transform.position, m_AlignmentRange, mask);

        // If theres more than 1 collision (AI Self + one other agent)
        if (entities.Length > 1)
        {
            // run for loop to find all overlapping entities 
            for (int i = 0; i < entities.Length; i++)
            {
                // Gets valid entities facing  direction   // get component then moving entity to get velocity
                facingDirection = entities[i].GetComponent<MovingEntity>().m_Velocity;

                // add forward transform to accumulated heading
                accumulatedHeading += facingDirection;

                if (entities.Length == i + 1)
                {
                    // all agents have been checked/added. Now divide by overall number of valid agents
                    AlignmentForce = accumulatedHeading / i;

                    // subtract our current facing direction
                    AlignmentForce = AlignmentForce - facingDirection;


                    // ISSUE IS HERE WITH DESIRED VELOCITY VARIABLE - TO FIX

                    // calculate m_DesiredVelocity to be the distance between your m_TargetPosition and current position
                    m_DesiredVelocity = AlignmentForce - (Vector2)transform.position;

                    // Then we normalise the m_desiredVelocity and multiply it by the max speed of the agent (m_manager.m_entity.m_Max speed)
                    m_DesiredVelocity = Maths.Normalise(m_DesiredVelocity);
                    m_DesiredVelocity = m_DesiredVelocity * m_Manager.m_Entity.m_MaxSpeed;

                    // calculate your m_Steering to be the m_DesiredVelocity - The AI agaents velocity (m_Manager.m_Entity.m_Velocity)
                    m_Steering = m_DesiredVelocity - m_Manager.m_Entity.m_Velocity;
                    m_Steering = Maths.Normalise(m_Steering);

                    //Debug.Log("I work");
                    return m_Steering * m_Weight;
                }
            }
        }

        // not in range - return nothing
        //Debug.Log("No Coll");
        return Vector2.zero;
    }
}
