using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncName : MonoBehaviour
{
    float _time = Mathf.Infinity;

    public void Update()
    {
        if (Time.time - _time >= 2.0f)
        {
            _time = Mathf.Infinity;
            GameObject.FindObjectOfType<Leaderboard>().OnUsernameChanged();
        }
    }

    public void OnCharactereChange(string Username)
    {
        _time = Time.time;
    }
}
