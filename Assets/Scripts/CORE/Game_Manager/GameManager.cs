using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
// Cho phép mở khoá các class và chỉnh sửa thông tin trong nó
public class MapElement
{
    // Biến string đầu tiên sẽ được lấy đặt tên cho element trong danh sách
    public string elementName;
    public TileBase tileToScan;
    public GameObject prefabToSpawn;
    public bool isTarget;
    public bool isTallObject;
    public TileBase tallTile;
}

public class GameManager : MonoBehaviour
{
    [Header("Map Setting")] public Tilemap map;

    [Header("Map Element Configuration")] public List<MapElement> mapElements;

    public int Target = 0;
    public int currentTarget = 0;
    public bool isMapReady;

    void Start()
    {
        ScanMap();
        isMapReady = true;
    }

    private void ScanMap()
    {
        BoundsInt bounds = map.cellBounds;
        foreach (var pos in bounds.allPositionsWithin)
        {
            TileBase currentTile = map.GetTile(pos);
            if (currentTile == null) continue;
            
            MapElement matchedElement = mapElements.FirstOrDefault(x => x.tileToScan == currentTile);
            if (matchedElement != null)
            {
                Vector3 currentPosition = map.CellToWorld(pos);
                Instantiate(matchedElement.prefabToSpawn, currentPosition, matchedElement.prefabToSpawn.transform.rotation);

                if (matchedElement.isTarget)
                {
                    Target++;
                }
                map.SetTile(pos,null);
                if (matchedElement.isTallObject)
                {
                    Vector3 upperPos = currentPosition + new Vector3(0, 2, 0);
                    Vector3Int upperTile = map.WorldToCell(upperPos);
                    map.SetTile(upperTile, matchedElement.tallTile);
                }
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