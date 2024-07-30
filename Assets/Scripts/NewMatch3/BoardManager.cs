using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;
    public int width;
    public int height;
    public GameObject tilePrefab;
    public TileBehaviour selectedTile;

    private TileBehaviour[,] tiles;
    private TileBehaviour spawnedTile;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        tiles = new TileBehaviour[width, height];
        SetupBoard();
    }

    void SetupBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (tiles[x, y] == null)
                {
                    Vector2 pos = new Vector2(x, y);
                    GameObject tileObj = Instantiate(tilePrefab, pos, Quaternion.identity);

                    tileObj.transform.SetParent(transform, true);
                    tileObj.name = $"Tile ({x},{y})";
                    spawnedTile = tileObj.GetComponent<TileBehaviour>();
                    spawnedTile.X = x;
                    spawnedTile.Y = y;
                    spawnedTile.TileItem = TileDatabase.Items[UnityEngine.Random.Range(0, ItemDatabase.Items.Length)];
                    tiles[x, y] = spawnedTile;
                }
            }
        }
        if (CheckForMatches())
        {
            ClearMatches();
            StartCoroutine(FillBoard());
        }

    }

    public void SwapTiles(TileBehaviour tile1, TileBehaviour tile2)
    {
        Vector2 tempPos = tile1.transform.position;
        tile1.transform.position = tile2.transform.position;
        tile2.transform.position = tempPos;

        int tempX = tile1.X;
        int tempY = tile1.Y;

        tile1.X = tile2.X;
        tile1.Y = tile2.Y;

        tile2.X = tempX;
        tile2.Y = tempY;

        tiles[tile1.X, tile1.Y] = tile1;
        tiles[tile2.X, tile2.Y] = tile2;

        if (CheckForMatches())
        {
            ClearMatches();
            StartCoroutine(FillBoard());
        }
        else
        {
            // Swap back if no matches found
            SwapTiles(tile1, tile2);
        }
    }

    bool CheckForMatches()
    {
        bool matchFound = false;

        // Check for horizontal matches
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width - 2; x++)
            {
                if (tiles[x, y].TileItem == tiles[x + 1, y].TileItem && tiles[x + 1, y].TileItem == tiles[x + 2, y].TileItem)
                {
                    matchFound = true;
                    tiles[x, y].IsMatched = true;
                    tiles[x + 1, y].IsMatched = true;
                    tiles[x + 2, y].IsMatched = true;
                }
            }
        }

        // Check for vertical matches
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height - 2; y++)
            {
                if (tiles[x, y].TileItem == tiles[x, y + 1].TileItem && tiles[x, y + 1].TileItem == tiles[x, y + 2].TileItem)
                {
                    matchFound = true;
                    tiles[x, y].IsMatched = true;
                    tiles[x, y + 1].IsMatched = true;
                    tiles[x, y + 2].IsMatched = true;
                }
            }
        }

        return matchFound;
    }
    void ClearMatches()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (tiles[x, y].IsMatched)
                {
                    Destroy(tiles[x, y].gameObject);
                    tiles[x, y] = null;
                }
            }
        }
        StartCoroutine(FillBoard());
    }
    IEnumerator FillBoard()
    {
        yield return new WaitForSeconds(0.5f);
        SetupBoard();
    }
}
