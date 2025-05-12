using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class PlayerCollision : MonoBehaviour
{
    public float invincibilityDuration = 1.5f; // Hur l�nge i-frames p�g�r
    public bool isInvincible = false; // Kontroll f�r om spelaren �r os�rbar
    private float invincibilityTimer = 0f; // Timer f�r att h�lla koll p� tiden

    RespawnScript respawnScript;
    PlayerMovement playerMovement;
    GameManager gameManager;

    private void Start()
    {

        respawnScript = FindAnyObjectByType<RespawnScript>();
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        gameManager = FindAnyObjectByType<GameManager>();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        switch (collision.transform.tag)
        {

            case "EnemyAttack":

                if (!isInvincible)
                {
                    playerMovement.knockback(collision.gameObject.GetComponentInParent<GameObject>().GetComponentInParent<GameObject>());

                }
                TakeDamage();

                break;

            case "MinigameHinder":

                Debug.Log(gameObject.transform.position);
                gameManager.StartMinigame(transform.position, collision.gameObject.name);

                break;

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyAttack")
        {
            if (!isInvincible)
            {
                playerMovement.knockback(collision.transform.parent.gameObject.transform.parent.gameObject);

            }
            TakeDamage();

        }

    }

    IEnumerator GetHurt()
    {
        isInvincible = true;
        yield return new WaitForSeconds(0.5f);
        isInvincible = false;

    }

    public void TakeDamage()
    {

        if (isInvincible == false)
        {

            HealthManager.health--;
            if (HealthManager.health <= 0)
            {
                //Game over, 
                //PlayerManager.isGameOver = true;
                respawnScript.Respawn();
            }
            else
            {
                StartCoroutine(GetHurt());
            }

        }
    }
}
