using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


[System.Serializable]
public class Pathfinding_AStar : PathFinding
{
    [System.Serializable]
    class NodeInformation
    {
        public GridNode node;
        public NodeInformation parent;
        public float gCost;
        public float hCost;
        public float fCost;

        public NodeInformation(GridNode node, NodeInformation parent, float gCost, float hCost)
        {
            this.node = node;
            this.parent = parent;
            this.gCost = gCost;
            this.hCost = hCost;
            fCost = gCost + hCost;
        }

        public void UpdateNodeInformation(NodeInformation parent, float gCost, float hCost)
        {
            this.parent = parent;
            this.gCost = gCost;
            this.hCost = hCost;
            fCost = gCost + hCost;
        }
    }

    public Pathfinding_AStar(bool allowDiagonal, bool cutCorners) : base(allowDiagonal, cutCorners) { }

    int GetCheapestNode(List<NodeInformation> aList)
    {
        int lowestIndex = 0;
        for(int i = 1; i < aList.Count; i++)
        {
            if (aList[i].fCost < aList[lowestIndex].fCost)
            {
                lowestIndex = i;
            }
        }
        return lowestIndex;
    }

    int IsOnList(GridNode currentNode, List<NodeInformation> aList)
    {
        for (int i = 0; i < aList.Count; i++)
        {
            if ((Vector2)aList[i].node.transform.position == (Vector2)currentNode.transform.position)
            {
                return i;
            }
        }

        //If not in list.
        return -1;
    }


    NodeInformation GetNeighbour(NodeInformation b)
    {
        NodeInformation Current_Neightbour = b;
        List<NodeInformation> neighnoursList = new List<NodeInformation>();

        if (Current_Neightbour != null)
        {
            Current_Neightbour.node = Current_Neightbour.node.Neighbours[0];

            return Current_Neightbour;

        }
        return Current_Neightbour;
    }
    
    public bool HasReachedEnd(Vector2 currentPosition)
    {
        if(m_Path.Count == 0)
        {
            return true;
        }

        // Check if the player's position is close enough to the target position (end of the path)
        Vector2 targetPosition = m_Path[m_Path.Count - 1]; // Assuming the last point in the path is the target
        float distance = Vector2.Distance(currentPosition, targetPosition);

        // Adjust the threshold based on your game's requirements
        float reachThreshold = 0.5f;

        return distance < reachThreshold;
    }

    public override void GeneratePath(GridNode start, GridNode end)
    {
        List<NodeInformation> openList = new List<NodeInformation>();
        List<NodeInformation> closedList = new List<NodeInformation>();
        List<NodeInformation> pathNodes = new List<NodeInformation>();
        List<Vector2> path = new List<Vector2>();
        

        // assign g, h and f values to n
        NodeInformation n = new NodeInformation(start, null, 0, Heuristic_Euclidean(start, end));
        NodeInformation BestNode = null;

        // add n to the open list  
        openList.Add(n);

        // For loop for Open List - Keep going until Current Node == End Node // OR Current list is empty
        while (openList.Count > 0)
        {
            int cheapestIndex = GetCheapestNode(openList);
            BestNode = openList[cheapestIndex];
            openList.RemoveAt(cheapestIndex);

            // if current node == destination node then quit
            if (BestNode.node == end)
            {
                //Debug.Log("Path Found");
                break;
            }

            // Let c Equal a valid node connected to current node // this may need to be re-worked into a neighbours function
            foreach(GridNode c in BestNode.node.Neighbours)
            {
                if (c != null && c.m_Walkable)
                {
  
                    
                    // calculate g, h and f values for c
                    NodeInformation CurrentNode = new NodeInformation(c, BestNode, c.m_Cost, Heuristic_Euclidean(c, end));


                    // is c on the open or closed list
                    int indexOnOpenList = IsOnList(c, openList);

                    //Is on the OPEN list.
                    if (indexOnOpenList != -1)
                    {
                        // c is on open list - Check if new path is more efficient
                        if (CurrentNode.fCost < openList[indexOnOpenList].fCost)
                        {
                            // set parent pointer to current path + set new g, h and f values
                            openList[indexOnOpenList].fCost = CurrentNode.fCost;
                            openList[indexOnOpenList].gCost = CurrentNode.gCost;
                            openList[indexOnOpenList].hCost = CurrentNode.hCost;
                            openList[indexOnOpenList].parent = CurrentNode.parent;
                        }
                    }
                    else
                    {
                        int indexOnClosedList = IsOnList(c, closedList);

                        //Is NOT on the CLOSED list.
                        if (indexOnClosedList == -1)
                        {
                            openList.Add(CurrentNode);
                        }
                    }
                }
            }

            // add current node to closed list 
            closedList.Add(BestNode);
        }

        // Create the path from parent nodes.
        while(BestNode != null)
        {
            path.Add(BestNode.node.transform.position);
            BestNode = BestNode.parent;
        };

        //drawPath
        Grid.ResetGridNodeColours();

		foreach (NodeInformation node in closedList)
		{
			node.node.SetClosedInPathFinding();
		}

		foreach (NodeInformation node in openList)
		{
			node.node.SetOpenInPathFinding();
		}

		foreach (NodeInformation node in pathNodes)
		{
			node.node.SetPathInPathFinding();
		}

		m_Path = path;
        // Path needs to be reversed as we figure out our route by going from end node to start node
        m_Path.Reverse();
    }
}

