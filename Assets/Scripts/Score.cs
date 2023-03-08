using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    TextMeshProUGUI text;

    void Start()
    {      
        text = GetComponent<TextMeshProUGUI>();
    }
    public static int count;
    void Update()
    {
        text.text = count.ToString();
    }  
}