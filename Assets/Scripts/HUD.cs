using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI coinsText;
    [SerializeField]
    private TMPro.TextMeshProUGUI GameOver;
    [SerializeField]
    private TMPro.TextMeshProUGUI _Score;
    [SerializeField]
    private RectTransform buttomConteneur;
    [SerializeField]
    private GameObject buttonPrefab;

    [SerializeField]
    GameObject fullBar;
    [SerializeField]
    Image progressBar;

    [SerializeField]
    private Leaderboard _Leaderboard;

    private void Awake()
    {
        GameOver.gameObject.SetActive(false);
        _Leaderboard.gameObject.SetActive(false);
        progressBar.fillAmount = 0.0f;

        while (buttomConteneur.childCount != 0)
        {
            GameObject.DestroyImmediate(buttomConteneur.GetChild(0).gameObject);
        }

        List<TurretsData> turretDatas = GameObject.FindObjectOfType<GameState>().GetTurretDatas();
        foreach (TurretsData data in turretDatas)
        {
            TurretsButton turretButton = GameObject.Instantiate(buttonPrefab, buttomConteneur).GetComponent<TurretsButton>();
            turretButton.Setup(data);
            turretButton.GetComponent<Button>().onClick.AddListener(delegate { OnTurretButtonClicked(data); });
        }
    }

    public void Gameover()
    {
        GameOver.gameObject.SetActive(true);
    }

    public void ShowLeadernboard(int Score)
    {
        _Leaderboard.ShowLeadernboard(Score);
    }

    void Update()
    {
        
    }

    public void OnBarChanged(float time)
    {
        progressBar.fillAmount = time / (Spawner._waveCount * Spawner._waveDuration);
    }

    public void OnCoinsChanged(int coins)
    {
        coinsText.text = coins.ToString();

        for (int buttom = 0; buttom < buttomConteneur.childCount; buttom++)
        {
            buttomConteneur.GetChild(buttom).GetComponent<TurretsButton>().Refresh(coins);
        }
    }

    public void OnScoreChanged(int Score)
    {
        _Score.text = Score.ToString();
    }

    public void OnButtonRetryClicked()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void OnTurretButtonClicked(TurretsData data)
    {
        int coins = GameObject.FindObjectOfType<GameState>().GetCoins();

        if (coins >= data._price)
        {
            GameObject.FindObjectOfType<TorretsPlacement>().BeginPlace(data);
        }
    }
}
