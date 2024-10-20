using UnityEngine;


public class SteeringBehaviour_Wander : SteeringBehaviour
{
    public float m_WanderRadius = 2; 
    public float m_WanderOffset = 2;
    public float m_AngleDisplacement = 2;

    Vector2 m_CirclePosition;
    Vector2 m_PointOnCircle;
    float m_Angle = 0.0f;


    public override Vector2 CalculateForce()
    {
        // Calculate random point on circle
        float radians = Mathf.Acos(Random.Range(-1, 1f));
        radians = radians + (Random.value < 0.5f ? 0 : Mathf.PI);

        // calculate the m_Point on circle Vector2 by multiplying the m_WanderRadius by the sin and cos of the radians
        m_PointOnCircle = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

        // Set the circles position to be the objects position + objects normalised velocity vector (heading direction and then multiply this by the wandering offset
        m_CirclePosition = ((Vector2)m_Manager.m_Entity.transform.position + (Maths.Normalise(m_Manager.m_Entity.m_Velocity) * m_WanderOffset));

        Vector2 headingTarget = m_PointOnCircle + m_CirclePosition;

        // calculate m_DesiredVelocity to be the distance between your heading target and current position
        m_DesiredVelocity = (Vector2) headingTarget - (Vector2)m_Manager.m_Entity.transform.position;

        // Then we normalise the m_desiredVelocity and multiply it by the max speed of the agent (m_manager.m_entity.m_Max speed)
        m_DesiredVelocity = Maths.Normalise(m_DesiredVelocity);
        m_DesiredVelocity = m_DesiredVelocity * m_Manager.m_Entity.m_MaxSpeed;

        // calculate your m_Steering to be the m_DesiredVelocity - The AI agaents velocity (m_Manager.m_Entity.m_Velocity)
        m_Steering = m_DesiredVelocity - m_Manager.m_Entity.m_Velocity;
        m_Steering = Maths.Normalise(m_Steering);

        return m_Steering * m_Weight;
    }


	protected override void OnDrawGizmosSelected()
	{
        if (Application.isPlaying)
        {
            if (m_Debug_ShowDebugLines && m_Active && m_Manager.m_Entity)
            {
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(m_CirclePosition, m_WanderRadius);

				Gizmos.color = Color.blue;
				Gizmos.DrawLine(transform.position, m_CirclePosition);

				Gizmos.color = Color.green;
				Gizmos.DrawLine(m_CirclePosition, m_PointOnCircle);

				Gizmos.color = Color.red;
				Gizmos.DrawLine(transform.position, m_PointOnCircle);

                base.OnDrawGizmosSelected();
			}
        }
	}
}
