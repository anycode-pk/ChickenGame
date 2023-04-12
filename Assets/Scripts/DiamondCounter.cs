using UnityEngine;
using TMPro;


public class DiamondCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counter;
    private int count;
    private int maxAmount;
    private void Start()
    {
        maxAmount = GameObject.FindGameObjectsWithTag("Diamond").Length;
        count = 0;
        counter.text = count + "/" + maxAmount;
    }
    public void AddDiamond()
    {
        count++;
        counter.text = count + "/" + maxAmount;
    }
}
