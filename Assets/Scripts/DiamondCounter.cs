using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DiamondCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI count;
    [SerializeField] private TextMeshProUGUI max;
    private int maxAmount=1;
    private void Start()
    {
        maxAmount = GameObject.FindGameObjectsWithTag("Diamond").Length;
        max.text = "/"+maxAmount.ToString();
    }
    public void AddDiamond()
    {
        int currentCount = int.Parse(count.text);
        currentCount++;
        count.text = currentCount.ToString();
    }
}
