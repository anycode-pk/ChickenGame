using UnityEngine;
using TMPro;

public class CoinsCounter : MonoBehaviour
{
    public TextMeshProUGUI coinsDisplay;
    private int count;

    public void AddCoinsCount(int numberOfCoins)
    {
        count += numberOfCoins;
        coinsDisplay.text = "" + count;
    }
}
