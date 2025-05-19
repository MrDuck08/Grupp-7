using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float acceleration;

    public float groundSpeed;
    public float jumpSpeed;
    float coyoteTimer;
    float coyoteTime = 0.15f;

    bool jumped = false;
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


    float direction = 1;

    Animator myAnimator;

    #region Animation For Next Scene

    bool animationForNextScene = false;

    float walkSpeedForAnimation = 5f;
    float accerationForAnimation;
    float timeForAnimation = 5f;

    #endregion

    #region Dash

    [Header("Dash")]

    [SerializeField] GameObject DashVisuals;

    float maxDashSearchLenght = 3;

    Vector2 playerPosOnSearch;

    public bool dashHasReset = true;

    #endregion

    bool playingSOund;

    AudioManager audioManager;

    private void Start()
    {
        myAnimator = GetComponentInChildren<Animator>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {

        if (animationForNextScene) 
        {

            walkSpeedForAnimation += accerationForAnimation * Time.deltaTime;

            transform.position += new Vector3(walkSpeedForAnimation * direction * Time.deltaTime, 0);

            if(walkSpeedForAnimation <= 0)
            {

                animationForNextScene = false;

            }

            return; 
        }

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

        if (grounded)
        {
            if (!jumped)
            {
                coyoteTimer = coyoteTime;
            }
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        #region Handle Stuff

        CheckGround();
        HandleJump();
        DashHandler();

        #endregion

        #region animation

        if (xInput == 0)
        {

            myAnimator.SetBool("IsRunning", false);
            if (playingSOund)
            {
                audioManager.RunningSoundStop();
                playingSOund = false;
            }
        }
        else
        {

            myAnimator.SetBool("IsRunning", true);

        }

        #endregion




        if (dashHasReset == true)
        {
            DashVisuals.gameObject.SetActive(true);
        }
        else
        {
            DashVisuals.gameObject.SetActive(false);
        }



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

        if (Input.GetButtonDown("Jump") && !jumped)
        {
            if (grounded)
            {
                audioManager.JumpSound();
                body.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
                StartCoroutine(ResetJump());
            }
            else if(coyoteTimer > 0)
            {

                audioManager.JumpSound();
                body.linearVelocity = new Vector2(body.linearVelocity.x, 0);
                body.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
                StartCoroutine(ResetJump());

            }

        }
    }

    IEnumerator ResetJump()
    {
        jumped = true;

        yield return new WaitForSeconds(0.2f);

        jumped = false;

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

    public void NextScene()
    {
        body.linearVelocity = Vector2.zero;
        myAnimator.SetTrigger("NextScene");
        animationForNextScene = true;

        accerationForAnimation = -walkSpeedForAnimation / timeForAnimation;

    }

    #region Dash

    void DashHandler()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashHasReset)
        {

            playerPosOnSearch = transform.position;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(direction, 0), maxDashSearchLenght, groundMask);
            audioManager.DashSound();
            if (hit)
            {

                transform.position = new Vector3(transform.position.x + Vector2.Distance(hit.point, playerPosOnSearch) * direction - 0.2f * direction, transform.position.y, transform.position.z);

                dashHasReset = false;

            }
            else
            {

                transform.position = new Vector3(transform.position.x + maxDashSearchLenght * direction, transform.position.y, transform.position.z);

                dashHasReset = false;

            }

        }

    }

    #endregion
}