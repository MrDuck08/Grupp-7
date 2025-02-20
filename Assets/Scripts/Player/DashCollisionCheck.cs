using UnityEngine;

public class DashCollisionCheck : MonoBehaviour
{

    PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Ground")
        {
            Debug.Log("Stop Due To Impact");

            

            playerMovement.TransformToDashPos(true);

        }

    }

}
