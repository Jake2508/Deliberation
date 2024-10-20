using UnityEngine;


public class AStarPathfinding : MovingEntity
{
    SteeringBehaviour_Manager m_SteeringBehaviours;
    SteeringBehaviour_Seek m_Seek;
    Pathfinding_AStar m_AStar;
    BFS_Pathfinding m_BFS;


    protected override void Awake()
    {
        base.Awake();

        m_SteeringBehaviours = GetComponent<SteeringBehaviour_Manager>();

        if (!m_SteeringBehaviours)
            Debug.LogError("Object doesn't have a Steering Behaviour Manager attached", this);

        m_Seek = GetComponent<SteeringBehaviour_Seek>();

        if (!m_Seek)
            Debug.LogError("Object doesn't have a Seek Steering Behaviour attached", this);

        m_AStar = new Pathfinding_AStar(true, false);
        m_BFS = new BFS_Pathfinding(true, false);

    }

    private void Start()
    {
        // Trigger pathfinding with the selected tile
        m_AStar.GeneratePath(Grid.GetNodeClosestWalkableToLocation(transform.position), Grid.GetNodeClosestToLocation(transform.position));
    }

    protected override Vector2 GenerateVelocity()
    {
        return m_SteeringBehaviours.GenerateSteeringForce();
    }

    protected void Update()
    {
        if (Input.GetMouseButtonDown(0))  
        {
            // Get Mouse Hit Pos
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Check if the clicked position is a valid tile
            GridNode clickedNode = Grid.GetNodeClosestWalkableToLocation(mousePosition); 

            if (clickedNode != null)
            {
                // Generate Pathfinding to selected tile
                m_AStar.GeneratePath(Grid.GetNodeClosestWalkableToLocation(transform.position), clickedNode);
            }
        }

        // Continue with logic if path generated
        if (m_AStar.m_Path.Count > 0)
        {
            Vector2 closestPoint = m_AStar.GetClosestPointOnPath(transform.position);

            if (Maths.Magnitude(closestPoint - (Vector2)transform.position) < 0.5f)
            {
                if(m_AStar.HasReachedEnd(transform.position))
                {
                    // Target Reached - Disable Seek Behaviour + Reset rb Velocity
                    m_Seek.m_Active = false;
                    m_SteeringBehaviours.m_Entity.ResetVelocity();
                }
                else
                {
                    // Target not reached - Set Seek target + Enable Seek Behaviour
                    closestPoint = m_AStar.GetNextPointOnPath(transform.position);
                    m_Seek.m_Active = true;
                }
            }
            m_Seek.m_TargetPosition = closestPoint;
        }
    }
        
    

    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.DrawLine(transform.position, m_Seek.m_TargetPosition);

            if (m_AStar.m_Path.Count > 1)
            {
                for (int i = 0; i < m_AStar.m_Path.Count - 1; ++i)
                {
                    Gizmos.DrawLine(m_AStar.m_Path[i], m_AStar.m_Path[i + 1]);
                }
            }
        }
    }
}

