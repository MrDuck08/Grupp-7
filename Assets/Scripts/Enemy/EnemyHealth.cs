using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    
    public int maxhealth = 3;
    public int currentHealth;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxhealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        Debug.Log("take damage");
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
