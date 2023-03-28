using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HealthController
{
    private int maxHealth = 3, currentHealth;
    public Image[] lives;
    public GameObject resume;
    public bool takeable = false;
    public float knockBackForce=5f;

    [Header("Components")]  
    PauseMenu pauseMenu;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] public Transform transform;

    [Header("Audio")]
    [SerializeField] private AudioClip heartSound;
    [SerializeField] private AudioSource audioSource;



    public void SetHp()
    {
        currentHealth = maxHealth;
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<PauseMenu>();
        TakeDamage(1);
    }

    public void TakeDamage(int amount)
    {
        for (int i = 2; i < 2 + amount; i++)
        {
            currentHealth--;
            lives[currentHealth].enabled = false;
            takeable = true;
        }
        //push from source
        if (currentHealth <= 0)
        {
            pauseMenu.PauseGame();
            resume.SetActive(false);
        }
    }

    public void TakeDamage(int amount, Transform source)
    {
        for (int i = 0; i < amount; i++)
        {
            if (currentHealth <= 0)
            {
                Debug.Log("zgon");
                //pauseMenu.PauseGame();
                //resume.SetActive(false);
                break;
            }
            currentHealth--;
            lives[currentHealth].enabled = false;
            takeable = true;
        }
        Vector3 direction = (rb.transform.position -  source.position).normalized;
        rb.AddForce(direction * knockBackForce);
       
    }
    public void AddHeart(int amount)
    {
        if (takeable)
        {
            for (int i = 1; i <= amount; i++)
            {               
                lives[currentHealth].enabled = true;
                currentHealth++;              
            }
            if (currentHealth >= 3)
                takeable = false;
        }
    }
    public GameObject PickUpHeart(Collider2D other)
    {
        if (other.gameObject.CompareTag("Heart"))
        {
            if (currentHealth < 3)
            {
                AddHeart(1);
                audioSource.PlayOneShot(heartSound);
                return other.gameObject;
            }
        }
        return null;
    }
}