using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    
    public int maxhealth = 5;
    public int currentHealth;

    List<int> damageList = new List<int>();

    bool invincibility = false;
    bool arleadyLooking;

    public bool dead = false;

    List<GameObject> children = new List<GameObject>();
    List<GameObject> checkChildren = new List<GameObject>();

    PlayerMovement playerMovement;
    EnemyFollowPlayer enemyMovment;
    EnemyAttack enemyAttack;
    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxhealth;

        enemyAttack = GetComponent<EnemyAttack>();

        Transform visualls = gameObject.transform.GetChild(2);
        animator = visualls.GetComponent<Animator>();

        if(visualls.GetComponent<SpriteRenderer>() != null )
        {

            children.Add(visualls.gameObject);

        }

        foreach (Transform child in visualls.transform)
        {

            children.Add(child.gameObject);
            checkChildren.Add(child.gameObject);

        }

        for (int i = 0; i < checkChildren.Count; i++)
        {

            if(checkChildren[i].transform.childCount != 0)
            {

                for (int j = 0; j < checkChildren[i].transform.childCount; j++)
                {

                    checkChildren.Add(checkChildren[i].transform.GetChild(j).gameObject);
                    children.Add(checkChildren[i].transform.GetChild(j).gameObject);


                }

            }

        }

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

            if(damageList[i] == 2) // Weakpoint hit, 2 skada
            {
                amount = damageList[i];

                playerMovement = FindObjectOfType<PlayerMovement>();

                playerMovement.dashHasReset = true;

                enemyMovment = GetComponent<EnemyFollowPlayer>();

                StartCoroutine(enemyMovment.Knockback());

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
            StartCoroutine(FlashRed());
            StartCoroutine(ActivateInvincibility());

            if (currentHealth <= 0)
            {
                StartCoroutine(DeathAnimation());
            }

        }
    }

    IEnumerator DeathAnimation()
    {

        animator.SetTrigger("Death");

        dead = true;

        yield return new WaitForSeconds(1.2f);

        if (enemyAttack.amIFinalBoss)
        {

            SceneLoader loader = FindObjectOfType<SceneLoader>();

            loader.LoadNextScene();

        }

        Destroy(gameObject);

    }

    #region Take Damage Red

    private IEnumerator FlashRed()
    {
        if(gameObject.GetComponent<SpriteRenderer>() != null)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }

        foreach(GameObject visualls in children)
        {

            if(visualls.GetComponent<SpriteRenderer>() != null)
            {
                visualls.GetComponent<SpriteRenderer>().color = Color.red;

            }

        }

        yield return new WaitForSeconds(0.1f);

        if (gameObject.GetComponent<SpriteRenderer>() != null)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }

        foreach (GameObject visualls in children)
        {

            if (visualls.GetComponent<SpriteRenderer>() != null)
            {
                visualls.GetComponent<SpriteRenderer>().color = Color.white;

            }

        }
    }

    #endregion

}
