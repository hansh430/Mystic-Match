using UnityEngine.UI;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Item itemType;
    public Image Icon;
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
