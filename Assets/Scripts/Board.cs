using System.Linq;
using UnityEngine;

public sealed class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }
    public Row[] Rows;
    public Tile[,] Tiles { get; private set; }
    public int Width=>Tiles.GetLength(0);
    public int Height=>Tiles.GetLength(1);
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Tiles = new Tile[Rows.Max(row => row.tiles.Length), Rows.Length];
        Debug.Log("Width: " + Width+ "Height"+ Height);
        for(var y=0; y<Height; y++)
        {
            for(var x=0; x<Width; x++)
            {
                var tile = Rows[y].tiles[x];
                tile.X = x;
                tile.Y = y;
                tile.Item = ItemDatabase.Items[Random.Range(0, ItemDatabase.Items.Length)];
                Tiles[x, y] = tile;
            }
        }
    }
}
