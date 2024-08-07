using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    private Board board;
    public List<Tile> CurrentMatches = new List<Tile>();
    private void Awake()
    {
        board = FindObjectOfType<Board>();
    }
    public void FindAllMatches()
    {
        CurrentMatches.Clear();
        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                Tile currentTile = board.AllTiles[x, y];
                if (currentTile != null)
                {
                    HorizontalMatch(x, y, currentTile);
                    VerticalMatch(x,y, currentTile);
                }
            }
        }
        if (CurrentMatches.Count > 0)
        {
            CurrentMatches = CurrentMatches.Distinct().ToList();
        }
        CheckForBombs();
    }

    private void HorizontalMatch(int x, int y, Tile currentTile)
    {
        if (x > 0 && x < board.Width - 1)
        {
            Tile leftTile = board.AllTiles[x - 1, y];
            Tile rightTile = board.AllTiles[x + 1, y];
            SetTileMatch(currentTile, leftTile, rightTile);
        }
    }

    private void VerticalMatch(int x, int y, Tile currentTile)
    {
        if (y> 0 && y < board.Height - 1)
        {
            Tile upperTile = board.AllTiles[x, y+1];
            Tile lowerTile = board.AllTiles[x, y-1];
            SetTileMatch(currentTile, upperTile, lowerTile);
        }
    }
    private void SetTileMatch(Tile currentTile, Tile firstTile, Tile secondTile)
    {
        if (firstTile != null && secondTile != null)
        {

            if (firstTile.type == currentTile.type && secondTile.type == currentTile.type)
            {
                currentTile.isMatched = true;
                firstTile.isMatched = true;
                secondTile.isMatched = true;

                CurrentMatches.Add(currentTile);
                CurrentMatches.Add(firstTile);
                CurrentMatches.Add(secondTile);
            }
        }
    }
    private void CheckForBombs()
    {
        for(int i=0; i<CurrentMatches.Count; i++)
        {
            Tile currentTile = CurrentMatches[i];
            int x = currentTile.posIndex.x;
            int y = currentTile.posIndex.y;
          
            if (x > 0)
            {
                Tile leftTile = board.AllTiles[x - 1, y];
                CheckForMarkingBombArea(leftTile);
            }
            if(x<board.Width-1)
            {
                Tile rightTile = board.AllTiles[x + 1, y];
                CheckForMarkingBombArea(rightTile);
            }
            if(y>0)
            {
                Tile lowerTile = board.AllTiles[x, y - 1];
                CheckForMarkingBombArea(lowerTile);
            }
            if(y<board.Height-1)
            {
                Tile upperTile = board.AllTiles[x, y + 1];
                CheckForMarkingBombArea(upperTile);
            }
        }
    }

    private void CheckForMarkingBombArea(Tile tile)
    {
        if (tile != null)
        {
            if (tile.type == TileType.Bomb)
            {
                MarkBombArea(tile.posIndex, tile);
            }
        }
    }
    private void MarkBombArea(Vector2Int bombPos, Tile theBomb)
    {
        for (int x = bombPos.x - theBomb.blastSize; x <= bombPos.x + theBomb.blastSize; x++)
        {
            for (int y = bombPos.y - theBomb.blastSize; y <= bombPos.y + theBomb.blastSize; y++)
            {
                if (x >= 0 && x < board.Width && y >= 0 && y < board.Height)
                {
                    if (board.AllTiles[x, y] != null)
                    {
                        board.AllTiles[x, y].isMatched = true;
                        CurrentMatches.Add(board.AllTiles[x, y]);
                    }
                }
            }
        }
        CurrentMatches = CurrentMatches.Distinct().ToList();
    }
}


