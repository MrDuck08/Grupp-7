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

    #region Search

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

    float searchSpeed = 5;

    #endregion

    #region Jump

    [Header("Jump")]

    [SerializeField] float maxJumpSpeed = 5;
    float jumpSpeed;
    [SerializeField] float extraJumpLenght = 5;
    float jumpAcceration = 1f;
    float distanceToJumpPos;
    float timeForJumpUp;
    float maxTimeForJump;
    float xHalfWayJump;

    Vector2 downJumpPos;
    Vector2 jumpPos;

    bool startJumpUp = false;
    bool startJumpingDown = false;
    bool jumping = false;

    bool jumpDown = false;

    #endregion

    Rigidbody2D rb2D;



    private void Start()
    {

        rb2D = GetComponent<Rigidbody2D>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        jumpSpeed = maxJumpSpeed;

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

        #region Jump

        if (startJumpUp)
        {

            timeForJumpUp -= Time.deltaTime;

            rb2D.gravityScale = 0;

            jumpSpeed += jumpAcceration * Time.deltaTime;


            transform.position = Vector2.MoveTowards(transform.position, new Vector2(jumpPos.x - xHalfWayJump, jumpPos.y), jumpSpeed * Time.deltaTime);

            if(timeForJumpUp <= 0)
            {

                startJumpUp = false;
                startJumpingDown = true;

                jumpPos.x += xHalfWayJump;
                jumpPos.y -= extraJumpLenght;
                if (jumpDown)
                {
                    jumpPos.y = downJumpPos.y;

                    jumpDown = false;
                }

                distanceToJumpPos = Vector3.Distance(new Vector2(jumpPos.x, jumpPos.y), transform.position);

                jumpSpeed = maxJumpSpeed;

                timeForJumpUp = distanceToJumpPos * 2 / jumpSpeed;

                timeForJumpUp = maxTimeForJump;

                jumpAcceration = jumpSpeed / timeForJumpUp;

                jumpSpeed = 0;

            }

        }

        if (startJumpingDown)
        {
            timeForJumpUp -= Time.deltaTime;

            jumpSpeed += jumpAcceration * Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, new Vector2(jumpPos.x - xHalfWayJump, jumpPos.y), jumpSpeed * Time.deltaTime);

            if (timeForJumpUp <= 0)
            {

                startJumpingDown = false;
                jumping = false;

                stop = false;

                Debug.Log("DOne");
                rb2D.gravityScale = 1;
            }

        }

        #endregion
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

        #region Wall Up Check

        if (wallChecked)
        {

            stop = true;

            findJumpYUp += searchSpeed * Time.deltaTime;

            findWhereToJumpUp = (Vector2)transform.position + new Vector2(wallCheckPosition.x + checkRadius, wallCheckPosition.y * findJumpYUp);
            bool upWallChecked = Physics2D.OverlapCircle(findWhereToJumpUp, checkRadius, groundLayer);


            if (!upWallChecked)
            {

                ResetCheckValues();

                Jump(findWhereToJumpUp, 0);

            }

        }

        #endregion

        #region Ground Search

        if (!groundChecked)
        {

            #region Right Ground Search

            if (!findNewGroundRight)
            {

                stop = true;

                findJumpXRight += searchSpeed * Time.deltaTime;

                findGroundRight = (Vector2)transform.position + new Vector2(groundCheckPosition.x + findJumpXRight, groundCheckPosition.y);
                findNewGroundRight = Physics2D.OverlapCircle(findGroundRight, checkRadius, groundLayer);


            }

            #endregion

            #region Down Ground Check

            stop = true;

            findJumpYDown += searchSpeed * Time.deltaTime;

            findGroundToJumpDown = (Vector2)transform.position + new Vector2(groundCheckPosition.x + 1, groundCheckPosition.y - findJumpYDown);
            bool findNewGroundDown = Physics2D.OverlapCircle(findGroundToJumpDown, checkRadius, groundLayer);

            if (findNewGroundDown)
            {

                ResetCheckValues();

                Jump(findGroundToJumpDown, 1);

            }

            #endregion

        }
        if (findNewGroundRight)
        {

            findJumpYRight += searchSpeed * Time.deltaTime;

            findGroundToJumpRight = new Vector2(findGroundRight.x + checkRadius * 2, findGroundRight.y + findJumpYRight);
            bool findWhereToJump = Physics2D.OverlapCircle(findGroundToJumpRight, checkRadius, groundLayer);

            if (!findWhereToJump)
            {

                transform.position = findGroundToJumpRight;

                ResetCheckValues();

            }

        }

        #endregion


        if (groundChecked && !wallChecked && !jumping)
        {
            stop = false;
        }

    }

    void Jump(Vector2 posToJumpTo, int whatTypeOfJump)
    {

        // 0 = Wall Jump
        // 1 = Down Jump
        // 2 = Hole Jump

        jumping = true;
        stop = true;

        if(whatTypeOfJump == 0)

        {
            jumpPos = posToJumpTo;

        }

        if(whatTypeOfJump == 1)
        {

            jumpPos = posToJumpTo;
            downJumpPos = posToJumpTo;
            Debug.Log("Jump Down");
            jumpPos.y = transform.position.y;

            jumpDown = true;

        }



        xHalfWayJump = jumpPos.x - transform.position.x;

        xHalfWayJump /= 2;

        jumpPos.y += extraJumpLenght;

        distanceToJumpPos = Vector2.Distance(new Vector2(jumpPos.x - xHalfWayJump, jumpPos.y), transform.position);

        timeForJumpUp = distanceToJumpPos * 2 / jumpSpeed;
        maxTimeForJump = timeForJumpUp;

        jumpAcceration = -jumpSpeed / timeForJumpUp;

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