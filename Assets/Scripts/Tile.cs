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
}
