using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class SteeringBehaviour_Seperation : SteeringBehaviour
{
    public float m_SeperationRange;
    Vector2 accumulatedSeperationForce = Vector2.zero;

    [Range(1, -1)]
    public float m_FOV;

    Vector2 AverageLocation = Vector2.zero;

    public override Vector2 CalculateForce()
    {
        // factor in FOV [pre set] (uses the dot product) for the collider 2d 

        // create layer mask, that only checks enemy AI layer
        LayerMask mask = LayerMask.GetMask("GroundAI");

        // Circle collider to see which entities are nearby ++ Added in custom layer to separate player char
        Collider2D[] entities = Physics2D.OverlapCircleAll(transform.position, m_SeperationRange, mask);

        // If theres more than 1 collision (AI Self + one other agent)
        if (entities.Length > 1)
        {
            // run for loop to find all overlapping entities
            for (int i = 0; i < entities.Length; i++)
            {
                // checks all entities within collision + layer
                // Normalise Vector then - add entitiy location to average location var divided by normol location
                AverageLocation = (Vector2)Maths.Normalise(entities[i].transform.position) / (Vector2)entities[i].transform.position;

                // add average location to the accumulateSeperationForce
                accumulatedSeperationForce += AverageLocation;
            }

            // calcualte distance between you and target pos
            m_DesiredVelocity = ((Vector2)transform.position - (Vector2)accumulatedSeperationForce);

            Vector2 distance = ((Vector2)transform.position - (Vector2)accumulatedSeperationForce);

            // normalise your desired velocity then multiply it by the max speed of the AI agent
            m_DesiredVelocity = Maths.Normalise(m_DesiredVelocity);
            m_DesiredVelocity = m_DesiredVelocity * m_Manager.m_Entity.m_MaxSpeed;

            // calculate steering to be your [desired velocity - AI agents velocity]
            m_Steering = m_DesiredVelocity - m_Manager.m_Entity.m_Velocity;

            // return the normalised steering vector that has been multipled by your weight
            m_Steering = Maths.Normalise(m_Steering);
            return m_Steering * Mathf.Lerp(m_Weight, 0, Mathf.Min(Maths.Magnitude(distance), Maths.Magnitude(accumulatedSeperationForce) / Maths.Magnitude(accumulatedSeperationForce)));

        }

        // return nothing as no collisions were detected
        return Vector2.zero;
    }
}