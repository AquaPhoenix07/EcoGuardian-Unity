using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [Header("Map Settings")] // Tạo ra một header trong bảng Inspector
    public Tilemap map;
    //Box Tile
    [Header("Box Tile")]
    //LEFT
    public TileBase garbageTile;
    public GameObject garbagePrefab;
    public TileBase fertilizerTile;
    public GameObject fertilizerPrefab;
    //RIGHT
    public TileBase freshWaterTile;
    public GameObject freshWaterPrefab;
    public TileBase magicSeedTile;
    public GameObject magicSeedPrefab;
    
    //Target Tile
    [Header("Target")]
    //LEFT
    public TileBase trashcanTile;
    public GameObject emptyTrashcanPrefab;
    
    public TileBase trunkTile;
    public GameObject trunkPrefab;
    //RIGHT
    public TileBase holeToPlantTile;
    public GameObject holeToPlantPrefab;

    public TileBase posionLakeTile;
    public GameObject posionLakePrefab;
    
    // Shelter Tile
    [Header("Shelter")]
    public TileBase caveTile;
    public GameObject cavePrefab;

    public TileBase treeHouseTile;
    public GameObject treeHousePrefab;
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
            
            
            //Đổi tile thành GameObject cho Items
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
            else if (currentTile == freshWaterTile)
            {
                Vector3 currentPos = map.CellToWorld(pos);
                Instantiate(freshWaterPrefab, currentPos, freshWaterPrefab.transform.rotation);
                map.SetTile(pos, null);
            }
            else if (currentTile == magicSeedTile)
            {
                Vector3 currentPos = map.CellToWorld(pos);
                Instantiate(magicSeedPrefab, currentPos, magicSeedPrefab.transform.rotation);
                map.SetTile(pos,null);
            }
            
            
            // Đổi Tile thành GameObject cho Target
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
                
                Instantiate(trunkPrefab, currentPos, trunkPrefab.transform.rotation);
                Target++; 
                
                map.SetTile(pos,null);
                map.SetTile(upperTile, null);
            }
            else if (currentTile == posionLakeTile)
            {
                Vector3 currentPos = map.CellToWorld(pos);
                Instantiate(posionLakePrefab, currentPos, posionLakePrefab.transform.rotation);
                Target++;
                map.SetTile(pos,null);
            }
            else if (currentTile == holeToPlantTile)
            {
                Vector3 currentPos = map.CellToWorld(pos);
                Instantiate(holeToPlantPrefab, currentPos, holeToPlantPrefab.transform.rotation);
                Target++;
                map.SetTile(pos,null);
            }
            
            
            // Đổi Tile thành GameObject cho Shelter
            else if (currentTile == caveTile)
            {
                Vector3 currentPos = map.CellToWorld(pos);
                Instantiate(cavePrefab, currentPos, cavePrefab.transform.rotation);
                map.SetTile(pos,null);  
            }
            else if (currentTile == treeHouseTile)
            {
                Vector3 currentPos = map.CellToWorld(pos);
                Instantiate(treeHousePrefab, currentPos, treeHousePrefab.transform.rotation);
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
