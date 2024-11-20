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
    bool findNewGroundRight = false;

    Transform player;

    Vector2 findWhereToJumpUp = new Vector2(1.5f, 0.2f);
    float findJumpYUp = 0.1f;

    Vector2 findGroundToJumpDown;
    float findJumpYDown = 0.1f;

    Vector2 findGroundRight;
    float findJumpYRight = 0.1f;
    float findJumpXRight = 0.1f;
    Vector2 findGroundToJumpRight;

    public GameObject TestObject;

    #region Jump

    [SerializeField] float jumpSpeed = 5;
    [SerializeField] float extraJumpLenght = 5;
    float jumpAcceration = 0.1f;
    float distanceToJumpPos;
    float timeForJumpUp;

    Vector2 jumpPos;

    bool startJumpUp = false;
    bool startJumpingDown = false;
    bool jumping = false;

    #endregion

    Rigidbody2D rb2D;



    private void Start()
    {

        rb2D = GetComponent<Rigidbody2D>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    private void FixedUpdate()
    {

        Movement();

    }

    private void Update()
    {

        //jumpSpeed -= jumpAcceration * Time.deltaTime;

        //if(jumpSpeed < 0)
        //{
        //    jumpSpeed = 0;
        //}

        //TestObject.transform.position = Vector2.MoveTowards(TestObject.transform.position, new Vector2(1333, 133), jumpSpeed);

        if(!jumping)
        {
            JumpCheck();
        }

        if(startJumpUp)
        {

            rb2D.gravityScale = 0;

            jumpSpeed -= jumpAcceration * Time.deltaTime;


            transform.position = Vector2.MoveTowards(transform.position, jumpPos, jumpSpeed);

            if(transform.position == new Vector3(jumpPos.x, jumpPos.y))
            {
                Debug.Log(jumpPos + " Up Pos");
                startJumpUp = false;
                startJumpingDown = true;

                jumpPos.x *= 2;
                jumpPos.y -= extraJumpLenght;

                distanceToJumpPos = Vector3.Distance(new Vector2(jumpPos.x, jumpPos.y), transform.position);

                timeForJumpUp = distanceToJumpPos / jumpSpeed;

                jumpAcceration = jumpSpeed / timeForJumpUp;

                jumpSpeed = 0;

                Debug.Log(jumpPos + " Down Pos");

            }

        }

        if (startJumpingDown)
        {

            jumpSpeed += jumpAcceration * Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, jumpPos, jumpSpeed);

            if (transform.position == new Vector3(jumpPos.x, jumpPos.y))
            {

                startJumpingDown = false;
                jumping = false;



                rb2D.gravityScale = 1;
            }

        }
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

    void JumpCheck()
    {

        Vector2 relativeWallCheckPosition = (Vector2)transform.position + new Vector2(wallCheckPosition.x, wallCheckPosition.y);
        bool wallChecked = Physics2D.OverlapCircle(relativeWallCheckPosition, checkRadius, groundLayer);

        Vector2 relativeGroundCheckPosition = (Vector2)transform.position + new Vector2(groundCheckPosition.x, groundCheckPosition.y);
        bool groundChecked = Physics2D.OverlapCircle(relativeGroundCheckPosition, checkRadius, groundLayer);

        #region Ground Search

        if (!groundChecked)
        {

            #region Right Ground Search

            if (!findNewGroundRight)
            {

                stop = true;

                findJumpXRight += 1f * Time.deltaTime;

                findGroundRight = (Vector2)transform.position + new Vector2(groundCheckPosition.x + findJumpXRight, groundCheckPosition.y);
                findNewGroundRight = Physics2D.OverlapCircle(findGroundRight, checkRadius, groundLayer);


            }

            #endregion

            #region Down Ground Check

            stop = true;

            findJumpYDown += 1f * Time.deltaTime;

            findGroundToJumpDown = (Vector2)transform.position + new Vector2(groundCheckPosition.x + 1, groundCheckPosition.y - findJumpYDown);
            bool findNewGroundDown = Physics2D.OverlapCircle(findGroundToJumpDown, checkRadius, groundLayer);

            if (findNewGroundDown)
            {

                transform.position = findGroundToJumpDown;

                ResetCheckValues();

            }

            #endregion

        }
        if (findNewGroundRight)
        {

            findJumpYRight += 1f * Time.deltaTime;

            findGroundToJumpRight = new Vector2(findGroundRight.x + checkRadius * 2, findGroundRight.y + findJumpYRight);
            bool findWhereToJump = Physics2D.OverlapCircle(findGroundToJumpRight, checkRadius, groundLayer);

            if (!findWhereToJump)
            {

                transform.position = findGroundToJumpRight;

                ResetCheckValues();

            }

        }

        #endregion

        #region Wall Up Check

        if (wallChecked)
        {

            stop = true;

            findJumpYUp += 2f * Time.deltaTime;

            findWhereToJumpUp = (Vector2)transform.position + new Vector2(wallCheckPosition.x + checkRadius/2, wallCheckPosition.y * findJumpYUp);
            bool upWallChecked = Physics2D.OverlapCircle(findWhereToJumpUp, checkRadius, groundLayer);


            if (!upWallChecked)
            {

                Jump(findWhereToJumpUp);

                ResetCheckValues();
            }

        }

        #endregion

        if (groundChecked && !wallChecked)
        {
            stop = false;
        }

    }

    void Jump(Vector2 posToJumpTo)
    {
        jumping = true;


        jumpPos = posToJumpTo;

        Debug.Log(jumpPos + " 1");

        float distanceX = jumpPos.x - transform.position.x;

        distanceX /= 2;

        float x = jumpPos.x - distanceX;

        jumpPos.y += extraJumpLenght;

        Debug.Log(jumpPos + " 2");
        distanceToJumpPos = Vector3.Distance(new Vector2(x, jumpPos.y), transform.position);

        timeForJumpUp = distanceToJumpPos/jumpSpeed;

        jumpAcceration = jumpSpeed / timeForJumpUp;

        startJumpUp = true;

    }

    private void ResetCheckValues()
    {

        findJumpYUp = 0.1f;
        findJumpYDown = 0.1f;
        findJumpXRight = 0.1f;
        findJumpYRight = 0.1f;

        findNewGroundRight = false;

        stop = false;

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, walkRange);

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere((Vector2)transform.position + wallCheckPosition, checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + groundCheckPosition, checkRadius);

        Gizmos.color = Color.blue;


        Gizmos.DrawWireSphere(findGroundToJumpRight, checkRadius);
        Gizmos.DrawWireSphere(findWhereToJumpUp, checkRadius);
        Gizmos.DrawWireSphere(findGroundToJumpDown, checkRadius);
        Gizmos.DrawWireSphere(findGroundRight, checkRadius);

    }

}
