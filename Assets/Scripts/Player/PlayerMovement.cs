using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float acceleration;

    public float groundSpeed;
    public float jumpSpeed;
    [Range(0f, 1f)]

    public float groundDecay;
    public Rigidbody2D body;
    public Transform groundCheck;

    public LayerMask groundMask;

    public bool grounded;

    bool facingRight = true;

    int isDeadCheck = 1;

    float xInput;
    float yInput;
    bool isKnockbackActive = false;
    float knockbackDuration = 0.5f;
    float knockbackTimer = 0f;

    float direction;

    Animator myAnimator;

    #region Dash

    [Header("Dash")]

    [SerializeField] GameObject dashCheck;

    bool searchingForDashLocation = false;

    float searchSpeed = 50f;
    float maxDashSearchLenght = 3;

    Vector2 playerPosOnSearch;

    #endregion

    private void Start()
    {
        myAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        GetInput();

        Vector2 inputDirection = new Vector2(xInput, 0).normalized;

        #region What direction facing

        if (xInput < 0 && facingRight == true)
        {
            Flip();
            direction = -1;
            facingRight = false;
        }

        if (xInput > 0 && facingRight == false)
        {
            Flip();
            direction = 1;
            facingRight = true;
        }

        #endregion

        #region Knockback

        if (isKnockbackActive)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0)
            {
                isKnockbackActive = false; // Avsluta knockback
            }
            return; // Hoppa �ver r�relselogiken
        }

        #endregion

        body.linearVelocity = new Vector2(inputDirection.x * groundSpeed, body.linearVelocityY);

        #region Handle Stuff

        CheckGround();
        HandleJump();
        DashHandler();

        #endregion

        #region animation

        if (xInput == 0)
        {

            myAnimator.SetBool("IsRunning", false);


        }
        else
        {
            myAnimator.SetBool("IsRunning", true);

        }

        #endregion

        #region Dash search

        if (searchingForDashLocation)
        {

            playerPosOnSearch = transform.position;

            dashCheck.transform.position += new Vector3(direction * searchSpeed * Time.deltaTime , 0, 0);

            dashCheck.transform.localScale += new Vector3(searchSpeed * Time.deltaTime, 0, 0);

         

            if (Vector2.Distance(dashCheck.transform.position, playerPosOnSearch) >= Vector2.Distance(playerPosOnSearch, new Vector2(playerPosOnSearch.x + maxDashSearchLenght, playerPosOnSearch.y)))
            {

                TransformToDashPos(false);

            }

        }

        #endregion

    }
    private void FixedUpdate()
    {

        ApplyFriction();
    }

    void GetInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
    }

    #region General Movement

    void MoveWithInput()
    {
        if (Mathf.Abs(xInput) > 0)
        {

            float increment = xInput * acceleration;
            float newSpeed = body.linearVelocity.x + increment;

            body.linearVelocity = new Vector2(newSpeed, body.linearVelocity.y);

            float direction = Mathf.Sign(xInput);
            Vector3 currentScale = transform.localScale;
            transform.localScale = new Vector3(Mathf.Abs(currentScale.x) * direction, currentScale.y, currentScale.z);
        }

    }

    void FaceInput()
    {
        float moveInput = Input.GetAxis("Horizontal");
        transform.localScale = new Vector3(body.linearVelocity.x, jumpSpeed);
    }

    void HandleJump()
    {

        if (Input.GetButtonDown("Jump") && grounded)
        {

            body.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
        }
    }

    void Flip()
    {

        // V�nd sprite:n genom att spegla den p� x-axeln
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;

        Transform weaponBase = transform.Find("PlayerWeaponAll"); // Hittar Childen PlayerWeaponAll
        if (weaponBase != null) // Kollar Om Den Hittades
        {
            weaponBase.transform.localScale = scaler; // V�nda Vapnerna
        }
    }


    private void CheckGround()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.transform.position, 0.35f, groundMask);
    }

    void ApplyFriction()
    {

        if (grounded && xInput == 0 && body.linearVelocity.y <= 0)
        {
            body.linearVelocity *= groundDecay;
        }
    }

    #endregion

    #region Knockback

    public void knockback(GameObject objectThatCollidedWithMe)
    {

        //float direction = Mathf.Sign(transform.localScale.x); // F�r 1 eller -1 fr�n vilket h�ll den kollar

        //body.linearVelocity = new Vector2(250 * -direction, -10);

        Rigidbody2D body = GetComponent<Rigidbody2D>();

        // Riktning för knockback (vänster eller höger)
        float direction = Mathf.Sign(transform.localScale.x);

        // Applicera en kraft endast i X-led
        body.AddForce(new Vector2(10000 * -direction, 3), ForceMode2D.Impulse);
        //body.linearVelocity = new Vector2(200 * -direction, body.linearVelocity.y);

        isKnockbackActive = true;
        knockbackTimer = knockbackDuration;

        float knockBackDirection = Mathf.Sign(objectThatCollidedWithMe.transform.localScale.x);

        //float direction = Mathf.Sign(transform.localScale.x);
        body.linearVelocity = new Vector2(10f * -knockBackDirection * isDeadCheck, body.linearVelocity.y * isDeadCheck);

    }

    #endregion

    public IEnumerator Died()
    {

        isDeadCheck = 0;

        yield return new WaitForSeconds(0.1f);

        isDeadCheck = 1;

    }

    #region Dash

    void DashHandler()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {

            searchingForDashLocation = true;

        }

    }

    public void TransformToDashPos(bool WallHit)
    {

        float wallDistance = 0;

        if (WallHit)
        {

            wallDistance = 1f;



        }

        searchingForDashLocation = false;


        transform.position = new Vector3(transform.position.x + Vector2.Distance(dashCheck.transform.position, playerPosOnSearch) * direction - wallDistance * direction, transform.position.y, transform.position.z);
        Debug.Log(dashCheck.transform.position.x + " Dash Pos x");

        dashCheck.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        dashCheck.transform.localScale = new Vector3(0.28111f, 0.28111f, 0.28111f);

    }

    #endregion
}