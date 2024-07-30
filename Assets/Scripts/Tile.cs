using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class Tile : MonoBehaviour
{
    public int X;
    public int Y;
    public Image Icon;
    public Button TileButton;
    private Item item;

    public Item Item
    {
        get => item;
        set
        {
            if (item == value) return;
            item = value;
            Icon.sprite = item.sprite;
        }
    }
    private Tile left => X > 0 ? Board.Instance.Tiles[X - 1, Y] : null;
    private Tile top => Y > 0 ? Board.Instance.Tiles[X, Y - 1] : null;
    private Tile right => X < Board.Instance.Width - 1 ? Board.Instance.Tiles[X + 1, Y] : null;
    private Tile bottom => Y < Board.Instance.Height - 1 ? Board.Instance.Tiles[X, Y + 1] : null;
    public Tile[] Neighbours => new[]
    {
        left, top, right, bottom
    };

    private void Start()
    {
        TileButton.onClick.AddListener(() => Board.Instance.SelectTile(this));
    }

    public List<Tile> GetConnectedTiles(List<Tile> exclude = null)
    {
        var result = new List<Tile> { this, };
        if (exclude == null)
        {
            exclude = new List<Tile> { this, };
        }
        else
        {
            exclude.Add(this);
        }
        foreach (var neighbour in Neighbours)
        {
            if (neighbour == null || exclude.Contains(neighbour) || neighbour.item != item) continue;
            result.AddRange(neighbour.GetConnectedTiles(exclude));
        }
        return result;
    }

}
