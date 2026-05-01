using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    
    [Header("Map Settings")] // Tạo ra một header trong bảng Inspector
    public Tilemap map;
    //Box Tile
    public TileBase boxTile;
    public GameObject boxPrefab;
    //Wall Tile
    public TileBase trashCanTile;
    public GameObject trashCanPrefab;
    //Target
    private int totalTrashCan = 0;
    private int currentGarbageOnTaget = 0; 
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ScanMap();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void ScanMap()
    {
        // Lấy kích thước bao trùm map
        BoundsInt bounds = map.cellBounds;
        
        // Trả về Vector3Int là toạ độ số thứ tự ô trong Cell,
        // Ví dụ: "Ô ở cột số 5, hàng số 2". (Chỉ số: 5, 2, 0).
        foreach (var pos in bounds.allPositionsWithin) 
        {
            TileBase currentTile = map.GetTile(pos);
            if (currentTile == boxTile)
            { 
                // Trả về toạ độ gốc của ô trong world map từ toạ độ số thứ tự ô trong Cell
                Vector3 currentPos = map.CellToWorld(pos);
                
                Instantiate(boxPrefab, currentPos, boxPrefab.transform.rotation);
                // Set viên gạch tại vị trí cũ biến mất cho thùng đứng
                map.SetTile(pos, null);
            }
            else if (currentTile == trashCanTile)
            {
                Vector3 currentPos = map.CellToWorld(pos);
                Instantiate(trashCanPrefab, currentPos, trashCanPrefab.transform.rotation);
                totalTrashCan++;
                map.SetTile(pos, null);
            }
        }
    }
    
    private void CheckTarget()
    {
        Debug.Log(currentGarbageOnTaget);
        if (totalTrashCan == currentGarbageOnTaget)
        {
            Debug.Log("You win");
        }
    }

    public void UpdateScore(int value)
    {
        currentGarbageOnTaget += value;
        Debug.Log($"[GameManager] Điểm hiện tại: {currentGarbageOnTaget} / {totalTrashCan}");
        CheckTarget();
    }
}
