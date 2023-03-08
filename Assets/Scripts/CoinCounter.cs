using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI count;
    
    public void AddCoin()
    {
        int currentCount = int.Parse(count.text);
        currentCount++;
        count.text = currentCount.ToString();
    }
}
