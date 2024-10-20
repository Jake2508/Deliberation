using UnityEngine;


public class SteeringBehaviour_Evade : SteeringBehaviour
{
    public MovingEntity m_EvadingEntity;
    public float m_EvadeRadius;

    public override Vector2 CalculateForce()
    {
        if (m_EvadingEntity != null)
        {
            // calcualte distance between you and target pos
            m_DesiredVelocity = ((Vector2)transform.position - (Vector2)m_EvadingEntity.transform.position);

            Vector2 distance = ((Vector2)transform.position - (Vector2)m_EvadingEntity.transform.position);

            // combine their speeds together in a float
            float CombinedSpeed = Maths.Magnitude(m_DesiredVelocity) + Maths.Magnitude(m_EvadingEntity.m_Velocity);

            // Calculate prediction time which is magnitude of your newly created distance vector divded by combined speed
            float PredictionTime = Maths.Magnitude(distance) / CombinedSpeed;

            // calculate m_DesiredVelocity to be the distance between your m_TargetPos and current pos
            m_DesiredVelocity = ((((Vector2)m_EvadingEntity.transform.position + ((Vector2)m_EvadingEntity.m_Velocity) * PredictionTime)) - (Vector2)transform.position);

            // normalise your desired velocity then multiply it by the max speed of the AI agent
            m_DesiredVelocity = Maths.Normalise(m_DesiredVelocity);
            m_DesiredVelocity = m_DesiredVelocity * m_Manager.m_Entity.m_MaxSpeed;

            // calculate steering to be your [desired velocity - AI agents velocity]
            m_Steering = m_Manager.m_Entity.m_Velocity - m_DesiredVelocity;

            // Lerp between 2 values so [Weight and 0] 
            // return the normalised steering vector that has been multipled by your weight
            m_Steering = Maths.Normalise(m_Steering);
            return m_Steering * Mathf.Lerp(m_Weight, 0, Mathf.Min(Maths.Magnitude(distance), m_EvadeRadius) / m_EvadeRadius); ;
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
                Gizmos.DrawWireSphere(transform.position, m_EvadeRadius);

                base.OnDrawGizmosSelected();
            }
        }
    }
}
