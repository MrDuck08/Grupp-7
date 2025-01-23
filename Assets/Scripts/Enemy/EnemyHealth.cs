using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;

public class EnemyHealth : MonoBehaviour
{
    
    public int maxhealth = 5;
    public int currentHealth;

    List<int> damageList = new List<int>();

    bool invincibility = false;
    bool arleadyLooking;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxhealth;
    }

    IEnumerator ActivateInvincibility()
    {
        invincibility = true;
        yield return new WaitForSeconds(0.2f);
        invincibility = false;
        arleadyLooking = false;

    }

    public void TakeDamageInfo(int amount)
    {

        if(damageList.Count >= 6)
        {
            return;
        }

        damageList.Add(amount);

        if (arleadyLooking)
        {
            return;
        }

        arleadyLooking = true;

        StartCoroutine(TakeDamage());

    }

    IEnumerator TakeDamage()
    {

        yield return new WaitForSeconds(0.2f);

        int amount = 0;

        for(int i = 0; i < damageList.Count; i++)
        {

            if(damageList[i] == 2)
            {
                amount = damageList[i];

                break;
            }
            else // Den är 1
            {
                amount = damageList[i];
            }

        }

        damageList.Clear();

        if (!invincibility)
        {

            currentHealth -= amount;
            StartCoroutine(ActivateInvincibility());

            if (currentHealth <= 0)
            {
                Destroy(gameObject);
            }

        }
    }
}
