using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI yourScore;
    [SerializeField]
    private TMPro.TMP_InputField customName;

    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private GameObject scoreLinePrefab;

    private int _score;

    public void ShowLeadernboard(int Score) 
    {
        _score = Score;

        this.gameObject.SetActive(true);
        customName.text = PlayerPrefs.GetString("CustomName");
        yourScore.text = "Score : " + Score.ToString();

        RectTransform content = scrollRect.content;
        for (int index = 0; index < content.childCount; index++)
        {
            GameObject.Destroy(content.GetChild(index).gameObject);
        }

        PlayFab.ClientModels.GetLeaderboardAroundPlayerRequest getLeaderboardAroundPlayerRequest = new PlayFab.ClientModels.GetLeaderboardAroundPlayerRequest();
        getLeaderboardAroundPlayerRequest.PlayFabId = null;
        getLeaderboardAroundPlayerRequest.StatisticName = "TopScore";

        PlayFab.PlayFabClientAPI.GetLeaderboardAroundPlayer(getLeaderboardAroundPlayerRequest, OnLeaderboardRecevied, null);
    }

    private void OnLeaderboardRecevied(GetLeaderboardAroundPlayerResult obj)
    {
        for (int index = 0; index < obj.Leaderboard.Count; index++)
        {
            PlayerLeaderboardEntry entry = obj.Leaderboard[index];

            if (entry.PlayFabId == GameObject.FindObjectOfType<GameState>().GetPlayerId())
            {
                entry.StatValue = Math.Max(entry.StatValue, _score);
                break;
            }
        }

        obj.Leaderboard.Sort((item1, item2) => item2.StatValue - item1.StatValue);

        for (int index = 0; index < obj.Leaderboard.Count; index++)
        {
            PlayerLeaderboardEntry entry = obj.Leaderboard[index];

            GameObject line = GameObject.Instantiate(scoreLinePrefab, scrollRect.content);
            line.GetComponent<ScoreLine>().Setup(entry.PlayFabId, index + 1, entry.DisplayName, entry.StatValue);
        }
    }

    public void OnUsernameChanged()
    {
        PlayerPrefs.SetString("CustomName", customName.text);

        PlayFab.ClientModels.UpdateUserTitleDisplayNameRequest updateUserTitleDisplayNameRequest = new PlayFab.ClientModels.UpdateUserTitleDisplayNameRequest();
        updateUserTitleDisplayNameRequest.DisplayName = customName.text;

        PlayFab.PlayFabClientAPI.UpdateUserTitleDisplayName(updateUserTitleDisplayNameRequest, null, null);

        int lines = scrollRect.content.childCount;
        for (int index = 0; index < lines; ++index)
        {
            ScoreLine scoreLine = scrollRect.content.GetChild(index).GetComponent<ScoreLine>();
            if (scoreLine != null)
            {
                if (scoreLine.GetPlayerId() == GameObject.FindObjectOfType<GameState>().GetPlayerId())
                {
                    scoreLine.SetDisplayName(customName.text);
                }
            }
        }
    }
}
