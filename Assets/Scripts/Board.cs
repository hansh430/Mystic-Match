using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board Instance;
    public int width;
    public int Height;

    [SerializeField] private GameObject bgTilePrefab;
    [SerializeField] private Transform bgTileParent;
    [SerializeField] private Tile[] tiles;
    [SerializeField] private Transform tileParent;


    [SerializeField] private Tile bomb;

    [SerializeField] private float bombChance = 2f;

    [SerializeField] private float bonusAmount = .5f;


    [HideInInspector]
    public MatchFinder matchFind;

    [HideInInspector]
    public RoundManager RoundManag;

    public BoardState CurrentState = BoardState.move;

    public float TileSpeed=7f;

    public Tile[,] AllTiles;

    private float bonusMulti;

    private Tile[,] layoutStore;

    private void Awake()
    {
        Instance = this;
        matchFind = FindObjectOfType<MatchFinder>();
        RoundManag = FindObjectOfType<RoundManager>();
    }
    private void Start()
    {
        AllTiles = new Tile[width, Height];

      //  layoutStore = new Tile[width, height];

        Setup();
    }
    private void Setup()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Vector2 pos = new Vector2(x, y);
                GameObject bgTile = Instantiate(bgTilePrefab, pos, Quaternion.identity, bgTileParent);
                bgTile.name = "BG Tile - " + x + ", " + y;
              
                    int tileToUse = Random.Range(0, tiles.Length);

                    int iterations = 0;
                    while (MatchesAt(new Vector2Int(x, y), tiles[tileToUse]) && iterations < 100)
                    {
                        tileToUse = Random.Range(0, tiles.Length);
                        iterations++;
                    }

                    SpawnTile(new Vector2Int(x, y), tiles[tileToUse]);
            }
        }


    }

    private bool MatchesAt(Vector2Int currentTilePos, Tile tile)
    {
        if (currentTilePos.x > 1)
        {
            if (AllTiles[currentTilePos.x - 1, currentTilePos.y].type == tile.type && AllTiles[currentTilePos.x - 2, currentTilePos.y].type == tile.type)
            {
                return true;
            }
        }
        if (currentTilePos.y > 1)
        {
            if (AllTiles[currentTilePos.x, currentTilePos.y - 1].type == tile.type && AllTiles[currentTilePos.x, currentTilePos.y - 2].type == tile.type)
            {
                return true;
            }
        }
        return false;
    }

    private void SpawnTile(Vector2Int pos, Tile tileToSpawn)
    {
        if (Random.Range(0f, 100f) < bombChance)
        {
            tileToSpawn = bomb;
        }

        Tile tile = Instantiate(tileToSpawn, new Vector3(pos.x, pos.y, 0f), Quaternion.identity, tileParent);
        tile.name = "Gem - " + pos.x + ", " + pos.y;
        AllTiles[pos.x, pos.y] = tile;

        tile.SetTile(pos, this);
    }
}
public enum BoardState { wait, move }
