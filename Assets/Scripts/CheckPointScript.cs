using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
    private RespawnScript respawn;
    private BoxCollider2D checkPointCollider; 

    void Awake()
    {
        checkPointCollider = GetComponent<BoxCollider2D>();
        respawn = GameObject.FindGameObjectWithTag("RespawnManager").GetComponent<RespawnScript>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if(other.gameObject.CompareTag("Player"))
        {

            respawn.respawnPoint = this.gameObject;
            checkPointCollider.enabled = false;
        }
    }
}
