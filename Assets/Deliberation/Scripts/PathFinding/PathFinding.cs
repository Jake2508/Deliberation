using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Threading;
using UnityEngine;


public abstract class PathFinding
{
    protected GridNode[] m_GridNodes;
    protected bool m_AllowDiagonal;
    protected bool m_CutCorners;

    public int m_PathIndex;
    public List<Vector2> m_Path { get; protected set; }


    public PathFinding(bool allowDiagonal, bool cutCorners)
    {
        m_Path = new List<Vector2>();
        m_AllowDiagonal = allowDiagonal;
        m_CutCorners = cutCorners;
        m_GridNodes = Grid.GridNodes;
    }

    public abstract void GeneratePath(GridNode start, GridNode end);

    public Vector2 GetClosestPointOnPath(Vector2 position)
    {
        float distance = float.MaxValue;
        int closestPoint = int.MaxValue;

        for (int i = 0; i < m_Path.Count; ++i)
        {
            float tempDistance = Maths.Magnitude(m_Path[i] - position);
            if (tempDistance < distance)
            {
                closestPoint = i;
                distance = tempDistance;
            }
        }

        for (int j = 0; j < closestPoint - 1; ++j)
        {
            m_Path.RemoveAt(0);
        }

        return m_Path[0];
    }

    public Vector2 GetNextPointOnPath(Vector2 position)
    {
        Vector2 pos = position;
        if (m_Path.Count > 0)
        {
            m_Path.RemoveAt(0);

            if (m_Path.Count > 0)
                pos = m_Path[0];
        }

        return pos;
    }


    protected float Heuristic_Manhattan(GridNode start, GridNode end)
    {
        // float to calculate the amount of grid squares moved
        float distance;
        float distanceX;
        float distanceY;

        // check if start and end node are both valid
        if ((start != null) && (end != null))
        {
            distanceX = (end.transform.position.x) - (start.transform.position.x);
            distanceY = (end.transform.position.y) - (start.transform.position.y);

            distance = distanceX + distanceY;

            // return distance between 2 tiles
            return distance;
        }
        return 0f;
    }

    protected float Heuristic_Euclidean(GridNode start, GridNode end)
    {
        float distanceZ;
        float distanceX;
        float distanceY;

        // check if start and end node are both valid
        if ((start != null) && (end != null))
        {
            // Get the X and Y distance
            distanceX = (end.transform.position.x) - (start.transform.position.x);
            distanceY = (end.transform.position.y) - (start.transform.position.y);

            distanceX = distanceX * distanceX;
            distanceY = distanceY * distanceY;

            // Figure out sqr 'z' axis 
            distanceZ = Mathf.Sqrt(distanceX + distanceY);

            return distanceZ;
        }
        return 0.0f;
    }
}
