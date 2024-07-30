using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public sealed class Board : MonoBehaviour
{
    [SerializeField] private AudioClip collectAudio;
    [SerializeField] private Button refreshButton;
    public static Board Instance { get; private set; }
    public Row[] Rows;
    public Tile[,] Tiles { get; private set; }
    public int Width => Tiles.GetLength(0);
    public int Height => Tiles.GetLength(1);
    private readonly List<Tile> selection = new List<Tile>();
    private const float TWEENDURATION = 0.25F;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        refreshButton.onClick.AddListener(GenerateTiles);
        GenerateTiles();
        Pop();

    }
    private void OnDestroy()
    {
        refreshButton?.onClick.RemoveListener(GenerateTiles);
    }
    private void GenerateTiles()
    {
        Tiles = new Tile[Rows.Max(row => row.tiles.Length), Rows.Length];
        Debug.Log("Width: " + Width + "Height" + Height);
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var tile = Rows[y].tiles[x];
                tile.X = x;
                tile.Y = y;
                tile.Item = ItemDatabase.Items[UnityEngine.Random.Range(0, ItemDatabase.Items.Length)];
                Tiles[x, y] = tile;
            }
        }
    }

    public async void SelectTile(Tile tile)
    {
        if (!selection.Contains(tile))
        {
            if (selection.Count > 0)
            {
                if (Array.IndexOf(selection[0].Neighbours, tile) != -1) selection.Add(tile);
            }
            else
            {
                selection.Add(tile);
            }
        }

        if (selection.Count < 2) return;
        Debug.Log($"selected tile at {selection[0].X}, {selection[0].Y} and {selection[1].X}, {selection[1].Y}");

        await SwapTile(selection[0], selection[1]);

        if (CanPop())
            Pop();
        else
        {
            if (selection.Count > 0)
            {
                await SwapTile(selection[0], selection[1]);

            }

        }

        selection.Clear();
    }
    public async Task SwapTile(Tile tile1, Tile tile2)
    {
        var icon1 = tile1.Icon;
        var icon2 = tile2.Icon;
        var icon1Transform = icon1.transform;
        var icon2Transform = icon2.transform;
        var sequence = DOTween.Sequence();
        sequence.Join(icon1Transform.DOMove(icon2Transform.position, TWEENDURATION))
            .Join(icon2Transform.DOMove(icon1Transform.position, TWEENDURATION));

        await sequence.Play().AsyncWaitForCompletion();
        icon1Transform.SetParent(tile2.transform);
        icon2Transform.SetParent(tile1.transform);

        tile1.Icon = icon2;
        tile2.Icon = icon1;

        var tileItem = tile1.Item;
        tile1.Item = tile2.Item;
        tile2.Item = tileItem;
    }

    private bool CanPop()
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                if (Tiles[x, y].GetConnectedTiles().Skip(1).Count() >= 2) return true;
            }
        }
        return false;
    }
    private async void Pop()
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                var tile = Tiles[x, y];
                var connectedTiles = tile.GetConnectedTiles();
                if (connectedTiles.Skip(1).Count() < 2) continue;
                var deflateSequence = DOTween.Sequence();
                foreach (var connectedTile in connectedTiles) deflateSequence.Join(connectedTile.Icon.transform.DOScale(Vector3.zero, TWEENDURATION));
                await deflateSequence.Play().AsyncWaitForCompletion();

                AudioManager.Instance.PlayAudio(collectAudio);
                ScoreHandler.Instance.Score += tile.Item.value * connectedTiles.Count;

                var inflateSequence = DOTween.Sequence();
                foreach (var connectedTile in connectedTiles)
                {
                    connectedTile.Item = ItemDatabase.Items[UnityEngine.Random.Range(0, ItemDatabase.Items.Length)];
                    inflateSequence.Join(connectedTile.Icon.transform.DOScale(Vector3.one, TWEENDURATION));
                }
                await inflateSequence.Play().AsyncWaitForCompletion();
                x = 0;
                y = 0;
            }
        }
    }
}
