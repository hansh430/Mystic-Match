using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    }

    private void HorizontalMatch(int x, int y, Tile currentTile)
    {
        if (x > 0 && x < board.Width - 1)
        {
            Tile leftTile = board.AllTiles[x - 1, y];
            Tile rightTile = board.AllTiles[x + 1, y];
            if (leftTile != null && rightTile != null)
            {

                if (leftTile.type == currentTile.type && rightTile.type == currentTile.type)
                {
                    currentTile.isMatched = true;
                    leftTile.isMatched = true;
                    rightTile.isMatched = true;

                    CurrentMatches.Add(currentTile);
                    CurrentMatches.Add(leftTile);
                    CurrentMatches.Add(rightTile);
                }
            }
        }
    }
    private void VerticalMatch(int x, int y, Tile currentTile)
    {
        if (y> 0 && y < board.Height - 1)
        {
            Tile upperTile = board.AllTiles[x, y+1];
            Tile lowerTile = board.AllTiles[x, y-1];
            if (upperTile != null && lowerTile != null)
            {

                if (upperTile.type == currentTile.type && lowerTile.type == currentTile.type)
                {
                    currentTile.isMatched = true;
                    upperTile.isMatched = true;
                    lowerTile.isMatched = true;

                    CurrentMatches.Add(currentTile);
                    CurrentMatches.Add(upperTile);
                    CurrentMatches.Add(lowerTile);
                }
            }
        }
    }
}


