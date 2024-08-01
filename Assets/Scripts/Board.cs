using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;

    [SerializeField] private GameObject bgTilePrefab;
    [SerializeField] private Transform bgTileParent;
    [SerializeField] private Tile[] tiles;

    [SerializeField] private float tileSpeed;

    [SerializeField] private BoardState currentState = BoardState.move;

    [SerializeField] private Tile bomb;

    [SerializeField] private float bombChance = 2f;

    [SerializeField] private float bonusAmount = .5f;


    [HideInInspector]
    public MatchFinder matchFind;

    [HideInInspector]
    public RoundManager roundMan;

    private Tile[,] allTiles;

    private float bonusMulti;

    private BoardLayout boardLayout;
    private Tile[,] layoutStore;

    private void Awake()
    {
        matchFind = FindObjectOfType<MatchFinder>();
        roundMan = FindObjectOfType<RoundManager>();
        //  boardLayout = GetComponent<BoardLayout>();
    }
    private void Start()
    {
        allTiles = new Tile[width, height];

        layoutStore = new Tile[width, height];

        Setup();
    }
    private void Setup()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 pos = new Vector2(x, y);
                GameObject bgTile = Instantiate(bgTilePrefab, pos, Quaternion.identity);
                bgTile.transform.parent = transform;
                bgTile.name = "BG Tile - " + x + ", " + y;
                bgTile.transform.SetParent(bgTileParent, true);
                if (layoutStore[x, y] != null)
                {
                    SpawnTile(new Vector2Int(x, y), layoutStore[x, y]);
                }
              /*  else
                {

                    int tileToUse = Random.Range(0, tiles.Length);

                    int iterations = 0;
                    while (MatchesAt(new Vector2Int(x, y), tiles[tileToUse]) && iterations < 100)
                    {
                        gemToUse = Random.Range(0, tiles.Length);
                        iterations++;
                    }

                    SpawnTile(new Vector2Int(x, y), tiles[gemToUse]);
                }*/
            }
        }


    }

    private void SpawnTile(Vector2Int pos, Tile tileToSpawn)
    {
        if (Random.Range(0f, 100f) < bombChance)
        {
            tileToSpawn = bomb;
        }

        Tile tile = Instantiate(tileToSpawn, new Vector3(pos.x, pos.y + height, 0f), Quaternion.identity);
        tile.transform.parent = transform;
        tile.name = "Gem - " + pos.x + ", " + pos.y;
        allTiles[pos.x, pos.y] = tile;

     //   tile.SetupGem(pos, this);
    }
}
public enum BoardState { wait, move }
