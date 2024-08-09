using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void ScoreCheck(Tile tileToCheck, RoundManager roundManager, float bonusMultiplier,float bonusAmount )
    {
        roundManager.CurrentScore += tileToCheck.scoreValue;

        if (bonusMultiplier > 0)
        {
            float bonusToAdd = tileToCheck.scoreValue * bonusMultiplier * bonusAmount;
            roundManager.CurrentScore += Mathf.RoundToInt(bonusToAdd);
        }
    }
}
