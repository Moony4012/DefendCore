using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public int _coins = 5;
    //public int gain = 5;
    public int _Score = 0;
    public List<TurretsData> _turretDatas;

    private string _playfabId;

    void Start()
    {
        if (Jammer.PlayerPrefs.HasKey("CustomId") == false)
        {
            Jammer.PlayerPrefs.SetString("CustomId", Guid.NewGuid().ToString());
            Jammer.PlayerPrefs.Save();
        }

        PlayFab.ClientModels.LoginWithCustomIDRequest loginRequest = new PlayFab.ClientModels.LoginWithCustomIDRequest();
        loginRequest.CustomId = Jammer.PlayerPrefs.GetString("CustomId");
        loginRequest.CreateAccount = true;

        PlayFab.PlayFabClientAPI.LoginWithCustomID(loginRequest, OnLoginSuccess, OnLoginFailure);

        SetCoins(30);
    }

    public void GameWin()
    {
        AddScore((int)((float)_coins * 0.1f));
        AddScore(GameObject.FindObjectOfType<Core>().GetHp() * 10);

        PlayFab.ClientModels.StatisticUpdate leaderboard = new PlayFab.ClientModels.StatisticUpdate();
        leaderboard.StatisticName = "TopScore";
        leaderboard.Value = _Score;

        PlayFab.ClientModels.UpdatePlayerStatisticsRequest updateLeaderboardRequest = new PlayFab.ClientModels.UpdatePlayerStatisticsRequest();
        updateLeaderboardRequest.Statistics = new List<PlayFab.ClientModels.StatisticUpdate>();
        updateLeaderboardRequest.Statistics.Add(leaderboard);

        PlayFab.PlayFabClientAPI.UpdatePlayerStatistics(updateLeaderboardRequest, OnLeaderboardUpdateSuccess, OnLeaderboardUpdateFailure);
    }

    public void CheckForWin()
    {
        if (GameObject.FindObjectOfType<Spawner>().HasFinish() == false)
        {
            return;
        }

        if (GameObject.FindObjectsOfType<Enemy>().Length > 1)
        {
            return;
        }

        GameWin();
    }

    public void Gameover()
    {
        GameObject.FindObjectOfType<HUD>().Gameover();
    }

    private void OnLeaderboardUpdateFailure(PlayFabError obj)
    {
        Debug.LogError("Si t'as internet, t'as pas de leaderboard parce que : " + obj.ErrorMessage);
    }

    private void OnLeaderboardUpdateSuccess(UpdatePlayerStatisticsResult obj)
    {
        Debug.Log("Leaderboard updated");

        GameObject.FindObjectOfType<HUD>().ShowLeadernboard(_Score);
    }

    private void OnLoginFailure(PlayFabError obj)
    {
        Debug.LogError("C'est pas de bol...");
    }

    private void OnLoginSuccess(LoginResult obj)
    {
        if (obj.NewlyCreated == true)
        {
            Debug.Log("Compte PlayFab created");
        }
        else
        {
            Debug.Log("Connexion au Compte PlayFab");
        }
        _playfabId = obj.PlayFabId;
    }

    public string GetPlayerId()
    {
        return _playfabId;
    }

    void Update()
    {

       //if (Input.GetKeyDown(KeyCode.Space))
       //{
       //    SetCoins(_coins+gain);
       //}
    }
    public void SetCoins(int coins)
    {
        _coins = coins;
        GameObject.FindObjectOfType<HUD>().OnCoinsChanged(_coins);
    }
    public int GetCoins()
    {
        return _coins;
    }

    public int AddCoins(int _Price)
    {
        _coins += _Price;
        GameObject.FindObjectOfType<HUD>().OnCoinsChanged(_coins);
        return _coins;
    }

    public int AddScore(int Score)
    {
        _Score += Score;
        GameObject.FindObjectOfType<HUD>().OnScoreChanged(_Score);
        return _Score;
    }

    public List<TurretsData> GetTurretDatas()
    {
        return _turretDatas;
    }
}
