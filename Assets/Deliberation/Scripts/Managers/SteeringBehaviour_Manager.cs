using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class SteeringBehaviour_Manager : MonoBehaviour
{
    public MovingEntity m_Entity { get; private set; }
    public float m_MaxForce = 100;
    public float m_RemainingForce;
    public List<SteeringBehaviour> m_SteeringBehaviours;
    public bool allowMovement = true;


	private void Awake()
	{
        m_Entity = GetComponent<MovingEntity>();

        if(!m_Entity)
            Debug.LogError("Steering Behaviours only working on type moving entity", this);
    }

    private void Start()
    {
        var tex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/tex.png");
        Cursor.SetCursor(tex, new Vector2(0.5f, 0.5f), CursorMode.ForceSoftware);
    }

    public Vector2 GenerateSteeringForce()
    {
        if(allowMovement)
        {
            // set remaining force to equal max force
            m_RemainingForce = m_MaxForce;

            // create vector 2 CombinedForce
            Vector2 combinedForce = Vector2.zero;

            // loop through attached behaviours
            for (int i = 0; i < m_SteeringBehaviours.Count; i++)
            {
                // check for any remaining force left and that the steering behaviour is active
                if (m_RemainingForce > 0 && m_SteeringBehaviours[i].m_Active)
                {
                    // create new variable tempForce
                    Vector2 tempForce;

                    // call calculate force function on the current steering behaviour
                    tempForce = m_SteeringBehaviours[i].CalculateForce();

                    // check if magnitude of this vector is greater than your remaining force
                    if (Maths.Magnitude(tempForce) > m_RemainingForce)
                    {
                        // normalise tempForce and multiply by remaining force
                        tempForce = Maths.Normalise(tempForce) * m_RemainingForce;
                    }
                    // reduce remaining force by magnitude or tempForce
                    m_RemainingForce -= Maths.Magnitude(tempForce);

                    // add tempForce to combinedForce
                    combinedForce = tempForce;
                }
            }

            return combinedForce;
        }

        return Vector2.zero;
    }
    
    public void EnableExclusive(SteeringBehaviour behaviour)
	{
        if(m_SteeringBehaviours.Contains(behaviour))
		{
            foreach(SteeringBehaviour sb in m_SteeringBehaviours)
			{
                sb.m_Active = false;
			}

            behaviour.m_Active = true;
		}
        else
		{
            Debug.Log(behaviour + " doesn't not exist on object", this);
		}
	}
    public void DisableAllSteeringBehaviours()
    {
        foreach (SteeringBehaviour sb in m_SteeringBehaviours)
        {
            sb.m_Active = false;
        }
    }
}
