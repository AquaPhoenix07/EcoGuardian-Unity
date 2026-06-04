using UnityEngine;
using UnityEngine.UIElements;

public class PathNode
{

    [Header("Basic Distance Cost")] 
    public Vector3 position { get; }
    public int GCost;  // Chi phí từ điểm bắt đầu to Node hiênj tại 
    public int HCost;  // Khoảng cách từ điểm hiện tại tới đích
    public int FCost{
        get { return GCost + HCost; }
    } 
    
    public PathNode previousNode;

    public PathNode(Vector3 pos)
    {
        position = pos;
    }
    
    
}
