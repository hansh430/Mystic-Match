using UnityEngine.UI;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int posIndex { get; set; }
    public TileType type;
    public bool isMatched;
    public int blastSize = 2;
    public int scoreValue = 10;
    public GameObject destroyEffect;

    [SerializeField] private Board board;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private bool mousePressed;
    private float swipeAngle = 0;
    private Tile otherTile;
    private Vector2Int previousPos;

    private void Update()
    {
        
    }
    public void SetTile(Vector2Int pos, Board theBoard)
    {
        posIndex = pos;
        board = theBoard;
    }

}
    public enum TileType { Elephant, Girraf, Snake, Parrot, Panada, Penguin, Rabbit,Monkey,Hippo,Bomb }
