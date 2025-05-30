using UnityEngine;

public class MiniGamePlayer : MonoBehaviour
{

    [SerializeField] float moveSpeed = 5;

    Vector2 moveInput;

    Rigidbody2D rb2D;

    SceneLoader loader;
    GameManager gameManager;

    int health = 3;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {

        loader = FindAnyObjectByType<SceneLoader>();

        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        rb2D.linearVelocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * moveSpeed);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.transform.tag == "EnemyAttack")
        {

            health--;

            switch (health)
            {

                case 0:

                    loader.ReloadScene();

                    break;

                case 1:

                    gameObject.GetComponent<SpriteRenderer>().color = Color.red;

                    break;

                case 2:

                    gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;

                    break;

            }


        }

    }

}
