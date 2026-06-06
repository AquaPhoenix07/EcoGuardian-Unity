using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class ChangeGreen : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject smoke;
    [Header("Map Change")]
    public Tilemap groundTilemap;
    public Tilemap wallTilemap;
    public Tilemap decoratedTilemap;
    
    [Header("TileBase ChangePairs")]
    public List<TileBase> oldGroundTiles;
    public List<TileBase> newGroundTiles;
    
    public List<TileBase> oldWallTiles;
    public List<TileBase> newWallTiles;

    public List<TileBase> oldDecoratedTiles;
    public List<TileBase> newDecoratedTiles;

    [Header("VisualEffect")] [SerializeField]
    private float spreadSpeed = 0.02f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        
        smoke = GameObject.Find("Smoke");
        StartCoroutine("SpreadGrassGround");
        StartCoroutine("SpreadGrassWall");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
// Thay TileBase cho ground
    private IEnumerator SpreadGrassGround()
    {
        yield return new WaitUntil(() => 
            gameManager.Target == gameManager.currentTarget &&
            gameManager.isMapReady);
        smoke.SetActive(false);
        List<TileChangeData> tilesToChange = new List<TileChangeData>();
        BoundsInt bounds = groundTilemap.cellBounds;

        foreach (var pos in bounds.allPositionsWithin)
        {
            TileBase currentTile = groundTilemap.GetTile(pos);
            if (currentTile == null) continue;
            for (int i = 0; i < oldGroundTiles.Count; i++)
            {
                if (currentTile == oldGroundTiles[i])
                {
                    tilesToChange.Add(new TileChangeData
                    {
                        position = pos,
                        newTile = newGroundTiles[i]
                    });
                    break;
                }  
            }
        }

        foreach (var tile in tilesToChange)
        {
            groundTilemap.SetTile(tile.position, tile.newTile);
            yield return new WaitForSeconds(spreadSpeed);
        }
    }
// Thay TileBase cho Wall 
    private IEnumerator SpreadGrassWall()
    {
        yield return new WaitUntil(() => 
            gameManager.Target == gameManager.currentTarget && 
            gameManager.isMapReady);
        
        List<TileChangeData> tilesToChange = new List<TileChangeData>();
        BoundsInt bounds = wallTilemap.cellBounds;
        
        foreach (var pos in bounds.allPositionsWithin)
        {
            TileBase currentTile = wallTilemap.GetTile(pos);
            if (currentTile == null) continue;
            for (int i = 0; i < oldWallTiles.Count; i++)
            {
                if (currentTile == oldWallTiles[i])
                {
                    tilesToChange.Add(new TileChangeData
                    {
                        position = pos,
                        newTile = newWallTiles[i]
                    });
                    break;
                }
            }
        }

        foreach (var tile in tilesToChange)
        {
            wallTilemap.SetTile(tile.position, tile.newTile);
            yield return new WaitForSeconds(spreadSpeed);
        }
        StartCoroutine("SpreadDecoration");
    }
    //Thay TileBase cho hồ nước bẩn, lửa (decoration) 
    private IEnumerator SpreadDecoration()
    {
        yield return new WaitUntil(() => 
            gameManager.Target == gameManager.currentTarget &&
            gameManager.isMapReady);
        
        List<TileChangeData> tilesToChange = new List<TileChangeData>();
        BoundsInt bounds = decoratedTilemap.cellBounds;
        foreach (var pos in bounds.allPositionsWithin)
        {
            TileBase currentTile = decoratedTilemap.GetTile(pos);
            if (currentTile == null) continue;
            for (int i = 0; i < oldDecoratedTiles.Count; i++)
            {
                if (currentTile == oldDecoratedTiles[i])
                {
                    tilesToChange.Add(new TileChangeData
                    {
                        position = pos,
                        newTile = newDecoratedTiles[i]
                    });
                    break;
                }
            }
        }
        foreach (var tile in tilesToChange)
        {
            decoratedTilemap.SetTile(tile.position, tile.newTile);
            yield return new WaitForSeconds(spreadSpeed);
        }
        
        
    }

    private class TileChangeData
    {
        public Vector3Int position;
        public TileBase newTile;
    }

    
}
