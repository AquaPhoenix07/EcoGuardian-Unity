using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.TerrainTools;

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
        
        List<PathNode> frontierList = new List<PathNode> {startNode}; // Danh sách các Node đang xét (neighbors)
        List<PathNode> closedList = new List<PathNode>(); //Danh sách các node đã xem xong

        while (frontierList.Count > 0)
        {
            PathNode currentNode = GetBestNodeFrontier(frontierList);
            
            // 1. Kiểm tra nếu đã chạm đích
            if (Vector3.Distance(currentNode.position, targetPos) < 0.1f)
            {
                return RetracePath(startNode, currentNode);
            }
            frontierList.Remove(currentNode);
            closedList.Add(currentNode);
            
            //2.Xét các ô láng giêng
            foreach (Vector3 direction in new Vector3[] { Vector3.up, Vector3.down, Vector3.right, Vector3.left })
            {
                Vector3 neighborPos = currentNode.position + direction;
                
                // Kiểm tra nếu là vật cản hoặc đã check rồi --> không đi lại đường cũ ( nằm trong closedList)
                if (!IsWalkable(neighborPos) || IsPositionInList(neighborPos, closedList)) continue;
               
                int newGCost = currentNode.GCost + 10;
                PathNode neighborNode = GetNodeInList(neighborPos, frontierList);
                
                // Nếu ô láng giềng này mới tinh hoặc tìm được đường tới rẻ hơn 
                if (neighborNode == null || newGCost < neighborNode.GCost)
                {
                    if (neighborNode == null)
                    {
                        neighborNode = new PathNode(neighborPos);
                        frontierList.Add(neighborNode);
                    }

                    neighborNode.GCost = newGCost;
                    neighborNode.HCost = (int)GetDistance(neighborNode, targetNode);
                    neighborNode.previousNode = currentNode;
                }
            }
        }
        return null;
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
    
    private bool IsWalkable(Vector3 checkNode)
    {
        Collider2D hit  = Physics2D.OverlapPoint(checkNode, obstacleLayer);
        return hit == null;
    }

    private float GetDistance(PathNode A, PathNode B)
    {
        float xDistance = Mathf.Abs(A.position.x - B.position.x);
        float yDistance = Mathf.Abs(A.position.y - B.position.y);
        return (int)(xDistance + yDistance )* 10 ;
    }

    private List<Vector3> RetracePath(PathNode startNode, PathNode endNode)
    {
        List<Vector3> path = new List<Vector3>();
        PathNode currentNode = endNode;
        while (currentNode != null)
        {
            path.Add(currentNode.position);
            if (currentNode == startNode) break;
            currentNode = currentNode.previousNode;
        }
        path.Reverse();
        return path;
    }

    private bool IsPositionInList(Vector3 pos, List<PathNode> list)
    {
        foreach (var node in list)
        {
            if (Vector3.Distance(node.position, pos) < 0.1f) return true;
        }
        return false;
    }

    private PathNode GetNodeInList(Vector3 pos, List<PathNode> list)
    {
        foreach (var node in list)
        {
            if (Vector3.Distance(node.position, pos) < 0.1f)
            {
                return node;
            }
        }
        return null;
    }

}
