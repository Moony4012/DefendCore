
using UnityEngine;
using UnityEngine.UI;

public class TurretsButton : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI _labelText;
    [SerializeField]
    private TMPro.TextMeshProUGUI _priceText;

    private TurretsData _data;

    public void Setup(TurretsData data)
    {
        _data = data;
        _labelText.text = _data._label;
        _priceText.text = _data._price.ToString();
    }

    public void Refresh(int coins)
    {
        GetComponent<Button>().interactable = coins >= _data._price;
    }
}