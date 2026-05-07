using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public class AStarManager : MonoBehaviour
{
    public LayerMask obstacleLayer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Vector3> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        PathNode startNode = new PathNode(startPos);
        PathNode targetNode = new PathNode(targetPos);
        
        List<PathNode> resultPath = new List<PathNode>();
        List<PathNode> frontierList = new List<PathNode{startNode};
        List<PathNode> closedList = new List<PathNode>();

        while (frontierList.Count > 0)
        {
            PathNode currentNode = GetBestNodeFrontier(frontierList);
            frontierList.Remove(currentNode);
            closedList.Add(currentNode);
            
        }
    }

    public PathNode GetBestNodeFrontier(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].FCost < lowestFCostNode.FCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
            else if (lowestFCostNode.FCost == pathNodeList[i].FCost)
            {
                if (pathNodeList[i].HCost < lowestFCostNode.HCost )
                {
                     lowestFCostNode = pathNodeList[i];
                }
            }
        }
        return lowestFCostNode;
    }

    private List<PathNode> GetNeighborsNode(PathNode currentNode)
    {
        foreach (Vector3 direction in new Vector3[] { Vector3.up, Vector3.down, Vector3.right, Vector3.left })
        {
            Vector3 neighborPos = currentNode.position + direction;
            if (!IsWalkable(neighborPos) || closedList.Any(n => Vec)) 

        }
    }

    private bool IsWalkable(Vector3 checkNode)
    {
        Collider2D hit  = Physics2D.OverlapPoint(checkNode, obstacleLayer);
        return hit == null;
    }

    private float GetDistane(PathNode A, PathNode B)
    {
        float xDistance = Mathf.Abs(A.position.x - B.position.x);
        float yDistance = Mathf.Abs(A.position.y - B.position.y);
        return xDistance + yDistance;
    }

}
