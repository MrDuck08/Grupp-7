using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{

    RespawnScript respawnScript;

    private void Start()
    {
        respawnScript = FindAnyObjectByType<RespawnScript>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "EnemyAttack")
        {
            TakeDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyAttack")
        {
            TakeDamage();
        }
    }

    IEnumerator GetHurt()
    {

        Physics2D.IgnoreLayerCollision(6, 8);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreLayerCollision(6, 8, false);

    }

    public void TakeDamage()
    {
        HealthManager.health--;
        if (HealthManager.health <= 0)
        {
            //Game over, 

            respawnScript.Respawn();

        }
        else
        {
            StartCoroutine(GetHurt());
        }
    }
}
