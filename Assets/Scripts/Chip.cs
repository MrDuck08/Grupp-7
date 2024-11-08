using UnityEngine;
using UnityEngine.Rendering;

public class Chip : MonoBehaviour
{

    float speed;
    [SerializeField] float walkRange = 3;

    bool stop = false;

    Transform player;


    private void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    private void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceFromPlayer < walkRange && !stop)
        {

            float Direction = Mathf.Sign(player.position.x - transform.position.x);

            Vector2 MovePos = new Vector2(transform.position.x + -Direction * speed * Time.deltaTime, transform.position.y);

        }

    }

}
