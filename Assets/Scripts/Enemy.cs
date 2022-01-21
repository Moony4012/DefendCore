using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [Header("Enemy")]

    [SerializeField]
    protected int _Price = 10;

    [SerializeField]
    protected int _Score = 10;

    protected float _startTime = 0;

    protected override void OnDead(GameObject killer)
    {
        if (killer.GetComponent<Bullets>() != null)
        {
            GameState gameState = GameObject.FindObjectOfType<GameState>();

            gameState.AddCoins(_Price);

            float time = Time.time - _startTime;
            float percent = 1.0f - (time / 10.0f);
            if (percent > 0.0f)
            {
                gameState.AddScore((int)((float)_Score * percent));
            }
        }

        GameObject.FindObjectOfType<GameState>().CheckForWin();
    }
}
