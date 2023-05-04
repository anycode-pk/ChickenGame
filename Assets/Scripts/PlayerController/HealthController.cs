using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HealthController
{
    [SerializeField] private int maxHealth = 3, currentHealth;
    public Image[] lives;
    public GameObject resume;
    public bool takeable = false;
    public float knockBackForce=5f;

    [Header("Components")]  
    PauseMenu pauseMenu;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] public Transform transform;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider2D col;

    [Header("Audio")]
    [SerializeField] private AudioClip heartSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip deathSound;

    [Header("StringToHash")]
    private static readonly int Death = Animator.StringToHash("Death");


    public void SetHp()
    {
        currentHealth = maxHealth;
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<PauseMenu>();
        TakeDamage(1);
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("Taking damage amount: " + amount);
        for (int i = 2; i < 2 + amount; i++)
        {
            currentHealth--;
            lives[currentHealth].enabled = false;
            takeable = true;
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int amount, Vector2 contactPoint)
    {
        Debug.Log("Taking damage amount: " + amount);
        for (int i = 0; i < amount; i++)
        {
            currentHealth--;
            if (currentHealth <= 0)
            {              
                Die();
                return;
            }
            lives[currentHealth].enabled = false;
            takeable = true;
        }
        Vector3 direction = (rb.position - contactPoint).normalized;
        Knockback(direction, knockBackForce);
        
       
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

    private void Die()
    {
        audioSource.PlayOneShot(deathSound);
        CinemachineVirtualCamera vCam = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        vCam.Follow = null;
        col.enabled = false;
        animator.SetTrigger(Death);
        pauseMenu.RestartGame();
    }

    public void CheckForDamage(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            TakeDamage(3, other.GetContact(0).point);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            TakeDamage(1, other.GetContact(0).point);
        }
    }

    public void Knockback(Vector3 direction, float force)
    {           
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }
}