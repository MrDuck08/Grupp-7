using UnityEngine;
using UnityEngine.Rendering;

public class Chip : MonoBehaviour
{

    [Header("Wall & Ground check")]

    [SerializeField] Vector2 wallCheckPosition;
    [SerializeField] Vector2 groundCheckPosition;

    [SerializeField] float checkRadius = 1f;

    [SerializeField] LayerMask groundLayer;

    [Header("Movement")]

    [SerializeField] float speed = 3;
    [SerializeField] float walkRange = 3;

    bool stop = false;

    Transform player;

    Vector2 findWhereToJumpPos = new Vector2(1.5f, 0.2f);
    float findJumpY = 1;



    private void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    private void FixedUpdate()
    {

        Movement();
       

    }

    private void Update()
    {
        Jump();
    }

    private void Movement()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceFromPlayer < walkRange && !stop)
        {

            float Direction = Mathf.Sign(player.position.x - transform.position.x);

            Vector2 MovePos = new Vector2(transform.position.x + -Direction * (speed / distanceFromPlayer) * Time.deltaTime, transform.position.y);

            transform.position = MovePos;

        }
    }

    void Jump()
    {

        Vector2 relativeWallCheckPosition = (Vector2)transform.position + new Vector2(wallCheckPosition.x, wallCheckPosition.y);
        bool wallChecked = Physics2D.OverlapCircle(relativeWallCheckPosition, checkRadius, groundLayer);

        Vector2 relativeGroundCheckPosition = (Vector2)transform.position + new Vector2(groundCheckPosition.x, groundCheckPosition.y);
        bool groundChecked = Physics2D.OverlapCircle(relativeGroundCheckPosition, checkRadius, groundLayer);

        if (!groundChecked)
        {

            //stop = true;

            //findJumpY += 0.1f * Time.deltaTime;

            //findWhereToJumpPos = (Vector2)transform.position + new Vector2(wallCheckPosition.x + checkRadius, wallCheckPosition.y);
            //bool upWallChecked = Physics2D.OverlapCircle(findWhereToJumpPos, checkRadius, groundLayer);

        }

        if (wallChecked)
        {

            stop = true;

            findJumpY += 0.1f * Time.deltaTime;

            findWhereToJumpPos = (Vector2)transform.position + new Vector2(wallCheckPosition.x + checkRadius, wallCheckPosition.y + findJumpY);
            bool upWallChecked = Physics2D.OverlapCircle(findWhereToJumpPos, checkRadius, groundLayer);


            if (!upWallChecked)
            {
                Debug.Log("Move Up");
                transform.position = findWhereToJumpPos;

                findJumpY = 1;
                stop = false;

            }

        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, walkRange);

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere((Vector2)transform.position + wallCheckPosition, checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + groundCheckPosition, checkRadius);

        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(findWhereToJumpPos, checkRadius);

    }

}
