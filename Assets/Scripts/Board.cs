using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board Instance;
    public int Width;
    public int Height;

    [SerializeField] private GameObject bgTilePrefab;
    [SerializeField] private Transform bgTileParent;
    [SerializeField] private Tile[] tiles;
    [SerializeField] private Transform tileParent;


    [SerializeField] private Tile bomb;

    [SerializeField] private float bombChance = 2f;

    [SerializeField] private float bonusAmount = .5f;


    [HideInInspector]
    public MatchManager MatchFind;

    [HideInInspector]
    public RoundManager RoundManag;

    public BoardState CurrentState = BoardState.move;

    public float TileSpeed = 7f;

    public Tile[,] AllTiles;

    private float bonusMulti;

    private Tile[,] layoutStore;

    private void Awake()
    {
        Instance = this;
        MatchFind = FindObjectOfType<MatchManager>();
        RoundManag = FindObjectOfType<RoundManager>();
    }
    private void Start()
    {
        AllTiles = new Tile[Width, Height];

        //  layoutStore = new Tile[width, height];

        Setup();
    }
    private void Setup()
    {
        for (int x = 0; x < Width; x++)
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

    public void DestroyMatches()
    {
        for (int i = 0; i < MatchFind.CurrentMatches.Count; i++)
        {
            if (MatchFind.CurrentMatches[i] != null)
            {
                ScoreManager.Instance.ScoreCheck(MatchFind.CurrentMatches[i], RoundManag, bonusMulti, bonusAmount);
                DestroyMatchedTileAt(MatchFind.CurrentMatches[i].posIndex);
            }
        }

        StartCoroutine(DecreaseRowCo());
    }
    private IEnumerator DecreaseRowCo()
    {
        yield return new WaitForSeconds(.2f);

        int nullCounter = 0;

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (AllTiles[x, y] == null)
                {
                    nullCounter++;
                }
                else if (nullCounter > 0)
                {
                    AllTiles[x, y].posIndex.y -= nullCounter;
                    AllTiles[x, y - nullCounter] = AllTiles[x, y];
                    AllTiles[x, y] = null;
                }

            }

            nullCounter = 0;
        }

        StartCoroutine(FillBoardCo());
    }

    private IEnumerator FillBoardCo()
    {
        yield return new WaitForSeconds(.5f);
        RefillBoard();

        yield return new WaitForSeconds(.5f);

        MatchFind.FindAllMatches();

        if (MatchFind.CurrentMatches.Count > 0)
        {
            bonusMulti++;

            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
        else
        {
            yield return new WaitForSeconds(.5f);
            CurrentState = BoardState.move;

            bonusMulti = 0f;
        }
    }
    private void RefillBoard()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                if (AllTiles[x, y] == null)
                {
                    int gemToUse = Random.Range(0, tiles.Length);

                    SpawnTile(new Vector2Int(x, y), tiles[gemToUse]);
                }
            }
        }
    }


    private void DestroyMatchedTileAt(Vector2Int pos)
    {
        Tile tile = AllTiles[pos.x, pos.y];
        if (tile != null && tile.isMatched)
        {
            switch (tile.type)
            {
                case TileType.Bomb:
                    AudioManager.Instance.PlayAudio(AudioSourceType.ExplodeSoundAudioSource);
                    break;
                case TileType.Stone:
                    AudioManager.Instance.PlayAudio(AudioSourceType.StoneSoundAudioSource);
                    break;
                default:
                    AudioManager.Instance.PlayAudio(AudioSourceType.TileSoundAudioSource);
                    break;
            }
            Instantiate(tile.destroyEffect, new Vector2(tile.posIndex.x, tile.posIndex.y), Quaternion.identity);
            Destroy(tile.gameObject);
            AllTiles[pos.x, pos.y] = null;
        }
    }

    public void ShuffleBoard()
    {
        if (CurrentState != BoardState.wait)
        {
            CurrentState= BoardState.wait;
            List<Tile> tileFromBoard = new List<Tile>();
            for(int x=0; x< Width; x++)
            {
                for(int y=0; y< Height; y++)
                {
                    tileFromBoard.Add(AllTiles[x, y]);
                    AllTiles[x, y] = null;
                }
            }

            for(int x=0; x< Width; x++)
            {
                for (int y=0; y< Height; y++)
                {
                    int tileToUse = Random.Range(0, tileFromBoard.Count);
                    int iteration = 0;
                    while(MatchesAt(new Vector2Int(x, y), tileFromBoard[tileToUse])&& iteration<100 && tileFromBoard.Count > 1)
                    {
                        tileToUse = Random.Range(0,tileFromBoard.Count);    
                        iteration++;
                    }
                    tileFromBoard[tileToUse].SetTile(new Vector2Int(x, y), this);
                    AllTiles[x,y]= tileFromBoard[tileToUse];
                    tileFromBoard.RemoveAt(tileToUse);
                }
            }
            StartCoroutine(FillBoardCo());
        }
    }
}
public enum BoardState { wait, move }
