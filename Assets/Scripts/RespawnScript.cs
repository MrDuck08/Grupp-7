using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    public GameObject player; 
    public GameObject respawnPoint;

    GameObject[] testGameobject;


    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        player.transform.position = respawnPoint.transform.position;

        HealthManager.health = 3;



        foreach (GameObject enemys in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemys.GetComponent<EnemyFollowPlayer>().lineOfSite = enemys.GetComponent<EnemyFollowPlayer>().baseLineOfSite;
            enemys.transform.position = enemys.GetComponent<EnemyFollowPlayer>().startTransform;
            enemys.GetComponent<EnemyAttack>().ResetAttack();

        }


    }
}
