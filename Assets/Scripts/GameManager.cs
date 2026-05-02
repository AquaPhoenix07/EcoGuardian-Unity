using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [Header("Map Settings")] // Tạo ra một header trong bảng Inspector
    public Tilemap map;
    //Box Tile
    public TileBase garbageTile;
    public GameObject garbagePrefab;
    
    public TileBase fertilizerTile;
    public GameObject fertilizerPrefab;
    //Target Tile
    public TileBase trashcanTile;
    public GameObject emptyTrashcanPrefab;
    public GameObject trashcanPrefab;
    
    public TileBase trunkTile;
    public GameObject trunkPrefab;
    public GameObject treePrefab;
    
    // Shelter Tile
    public TileBase shelterTile;
    public GameObject shelterPrefab;
    //Target
    private int Target = 0;
    private int currentTarget = 0; 
    
    
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
            if (currentTile == garbageTile)
            { 
                // Trả về toạ độ gốc của ô trong world map từ toạ độ số thứ tự ô trong Cell
                Vector3 currentPos = map.CellToWorld(pos);
                
                Instantiate(garbagePrefab, currentPos, garbagePrefab.transform.rotation);
                // Set viên gạch tại vị trí cũ biến mất cho thùng đứng
                map.SetTile(pos, null);
            }
            else if (currentTile == fertilizerTile)
            {
                Vector3 currentPos = map.CellToWorld(pos);
                Instantiate(fertilizerPrefab, currentPos, fertilizerPrefab.transform.rotation);
                map.SetTile(pos, null);
            }
            else if (currentTile == trashcanTile)
            {
                Vector3 currentPos = map.CellToWorld(pos);
                Instantiate(emptyTrashcanPrefab, currentPos, emptyTrashcanPrefab.transform.rotation);
                Target++;
                map.SetTile(pos, null);
            }
            else if (currentTile == trunkTile)
            {
                Vector3 upperPos = new Vector3(pos.x, pos.y + 1, 0);
                
                Vector3Int upperTile = map.WorldToCell(upperPos);
                Vector3 currentPos = map.CellToWorld(pos);
                
                Instantiate(trunkPrefab, currentPos, treePrefab.transform.rotation);
                Target++; 
                
                map.SetTile(pos,null);
                map.SetTile(upperTile, null);
            }
            else if (currentTile == shelterTile)
            {
                Vector3 currentPos = map.CellToWorld(pos);
                Instantiate(shelterPrefab, currentPos, shelterPrefab.transform.rotation);
                map.SetTile(pos,null);  
            }
        }
    }
    
    private void CheckTarget()
    {
        Debug.Log($"[GameManager] Điểm hiện tại: {currentTarget} / {Target}");
        if (Target == currentTarget)
        {
            Debug.Log("You win");
        }
    }

    public void UpdateScore(int value)
    {
        currentTarget += value;
        CheckTarget();
    }
}
