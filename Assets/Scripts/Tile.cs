using UnityEngine.UI;
using UnityEngine;
using System;
using System.Collections;

public class Tile : MonoBehaviour
{
    public Vector2Int posIndex;
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
        TileReposition();

        if (mousePressed && Input.GetMouseButtonUp(0))
        {
            mousePressed = false;

            if (board.CurrentState == BoardState.move && board.RoundManag.roundTime > 0)
            {
                finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                CalculateAngle();
            }
        }
    }

    private void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * Mathf.Rad2Deg;
        Debug.Log("swipe angle" + swipeAngle);
        if (Vector3.Distance(firstTouchPosition, finalTouchPosition) > .5f)
        {
            MovePieces();
        }
    }

    private void MovePieces()
    {
        previousPos = posIndex;
        if (swipeAngle < 45 && swipeAngle > -45 && posIndex.x < board.width - 1)
        {
            otherTile = board.AllTiles[posIndex.x + 1, posIndex.y];
            otherTile.posIndex.x--;
            posIndex.x++;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && posIndex.y < board.Height - 1)
        {
            otherTile = board.AllTiles[posIndex.x, posIndex.y + 1];
            otherTile.posIndex.y--;
            posIndex.y++;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && posIndex.y > 0)
        {
            otherTile = board.AllTiles[posIndex.x, posIndex.y - 1];
            otherTile.posIndex.y++;
            posIndex.y--;
        }
        else if (swipeAngle > 135 || swipeAngle < -135 && posIndex.x > 0)
        {
            otherTile = board.AllTiles[posIndex.x - 1, posIndex.y];
            otherTile.posIndex.x++;
            posIndex.x--;
        }

        board.AllTiles[posIndex.x, posIndex.y] = this;
        board.AllTiles[otherTile.posIndex.x, otherTile.posIndex.y] = otherTile;
        StartCoroutine(CheckMoveCo());
    }
    private IEnumerator CheckMoveCo()
    {
        board.CurrentState = BoardState.wait;

        yield return new WaitForSeconds(.5f);

        //  board.matchFind.FindAllMatches();
        if (otherTile != null)
        {
            if (!isMatched && !otherTile.isMatched)
            {
                otherTile.posIndex = posIndex;
                posIndex = previousPos;

                board.AllTiles[posIndex.x, posIndex.y] = this;
                board.AllTiles[otherTile.posIndex.x, otherTile.posIndex.y] = otherTile;

                yield return new WaitForSeconds(.5f);

                board.CurrentState = BoardState.move;
            }
            else
            {
               // board.DestroyMatches();
            }
        }

    }
    private void TileReposition()
    {
        if (Vector2.Distance(transform.position, posIndex) > .01f)
        {
            transform.position = Vector2.Lerp(transform.position, posIndex, board.TileSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector3(posIndex.x, posIndex.y, 0f);
            board.AllTiles[posIndex.x, posIndex.y] = this;
        }
    }

    public void SetTile(Vector2Int pos, Board theBoard)
    {
        posIndex = pos;
        board = theBoard;
    }
    private void OnMouseDown()
    {
        if (board.CurrentState == BoardState.move && board.RoundManag.roundTime > 0)
        {

            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePressed = true;
        }
    }
}
public enum TileType { Elephant, Girraf, Snake, Parrot, Panada, Penguin, Rabbit, Monkey, Hippo, Bomb }
