using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "EnemyAttack")
        {
            HealthManager.health--;
            if (HealthManager.health <= 0)
            {
                //Game over, 
                gameObject.SetActive(false);
            }
            else
            {
                StartCoroutine(GetHurt());
            }
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
            //PlayerManager.isGameOver = true;
            gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(GetHurt());
        }
    }
}
