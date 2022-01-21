using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreLine : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI rank;
    [SerializeField]
    private TMPro.TextMeshProUGUI name;
    [SerializeField]
    private TMPro.TextMeshProUGUI score;

    private string _playerId;

    public void Setup(string playerId, int position, string displayName, int statValue)
    {
        _playerId = playerId;
        rank.text = position.ToString();
        name.text = displayName;
        score.text = statValue.ToString();

        if (_playerId == GameObject.FindObjectOfType<GameState>().GetPlayerId())
        {
            Image background = GetComponent<Image>();
            background.color = background.color * 1.3f;
        }
    }

    public void SetDisplayName(string displayName)
    {
        name.text = displayName;
    }

    public string GetPlayerId()
    {
        return _playerId;
    }
}
