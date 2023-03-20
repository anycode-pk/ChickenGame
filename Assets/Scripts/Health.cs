using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private int maxHealth=3, currentHealth;
    public Image[] lives; 
    PauseMenu pauseMenu;
    void Start()
    {
        currentHealth = maxHealth;
        pauseMenu= GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<PauseMenu>();
    }
    
    // Update is called once per frame
    public void TakeDamage(int amount)
    {
        for(int i = 2;i<2+amount;i++) 
        {
            currentHealth--;
            lives[currentHealth].enabled = false;
        }
       //SubtractHeart(currentHealth);
        if (currentHealth <= 0) 
        {
            //Debug.Log("xd");
            pauseMenu.PauseGame();           
        }
    }
    //private void SubtractHeart(int live) 
    //{
    //    lives[live].enabled = false;//gameObject.SetActive(false);
    //}
}
