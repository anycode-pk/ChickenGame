using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private int maxHealth=3, currentHealth;
    public Image[] lives;
    public GameObject resume;
    PauseMenu pauseMenu;
    void Start()
    {
        currentHealth = maxHealth;
        pauseMenu= GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<PauseMenu>();
        TakeDamage(1);
    }
    
    // Update is called once per frame
    public void TakeDamage(int amount)
    {
        for(int i = 2;i<2+amount;i++) 
        {
            currentHealth--;
            lives[currentHealth].enabled = false;
        }
       //push from source
        if (currentHealth <= 0) 
        {           
            pauseMenu.PauseGame();  
            resume.SetActive(false);
        }
    }

    public void AddHeart(int amount) 
    {
        for (int i = 1; i <= amount; i++)
        {
            if (currentHealth <= 3)
            {
                lives[currentHealth].enabled = true;
                currentHealth++;
            }
        }          
    }
}
