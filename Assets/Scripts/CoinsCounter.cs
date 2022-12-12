using UnityEngine;
using TMPro;

public class CoinsCounter : MonoBehaviour
{
    public TextMeshProUGUI coinsDisplay;
    private int count;

    public void AddCoinsCount(int numberOfCoins)
    {
        numberOfCoins += numberOfCoins;
        coinsDisplay.text = "Coins: " + count;
    }
}
