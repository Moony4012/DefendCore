using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : Entity
{    
    void Start()
    {
        base.Start();
    }

    protected override void OnDead(GameObject killer)
    {
        GameObject.FindObjectOfType<GameState>().Gameover();
    }
}
