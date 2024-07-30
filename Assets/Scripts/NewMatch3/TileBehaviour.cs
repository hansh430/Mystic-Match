using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class TileBehaviour : MonoBehaviour
{
    private BoardManager board;
    private TileBehaviour selectedTile;
    private Button button;
    public int X;
    public int Y;
    public Image Icon;
    private Item tileItem;
    public bool IsMatched { get; set; }
    public Item TileItem
    {
        get => tileItem;
        set
        {
            if (tileItem == value) return;
            tileItem = value;
            Icon.sprite = tileItem.sprite;
        }
    }
    void Start()
    {
        board = FindObjectOfType<BoardManager>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnTileClick);
    }

    void OnTileClick()
    {
        if (board.selectedTile == null)
        {
            board.selectedTile = this;
        }
        else
        {
            if (IsAdjacent(board.selectedTile))
            {
                board.SwapTiles(board.selectedTile, this);
                board.selectedTile = null;
            }
            else
            {
                board.selectedTile = this;
            }
        }
    }

    bool IsAdjacent(TileBehaviour otherTile)
    {
        Debug.Log($"other pos x {otherTile.transform.position.x} other pos y {otherTile.transform.position.y}");
        Debug.Log($"this pos x {transform.position.x} this pos y {transform.position.y}");
        return (Mathf.Abs(otherTile.transform.position.x - transform.position.x) == 110 && otherTile.transform.position.y == transform.position.y) ||
               (Mathf.Abs(otherTile.transform.position.y - transform.position.y) == 110 && otherTile.transform.position.x == transform.position.x);
    }
}
