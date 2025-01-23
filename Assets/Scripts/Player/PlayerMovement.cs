using System.Collections;
using System.Data.Common;
using UnityEditor;
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


    void Update()
    {
        GetInput();

        Vector2 direction = new Vector2(xInput, 0).normalized;

        if (xInput < 0 && facingRight == true)
        {
            Flip();
            facingRight = false;
        }

        if (xInput > 0 && facingRight == false)
        {
            Flip();
            facingRight = true;
        }

        if (isKnockbackActive)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0)
            {
                isKnockbackActive = false; // Avsluta knockback
            }
            return; // Hoppa �ver r�relselogiken
        }
        body.linearVelocity = new Vector2(direction.x * groundSpeed, body.linearVelocityY);
        CheckGround();
        HandleJump();


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

    public IEnumerator Died()
    {

        isDeadCheck = 0;

        yield return new WaitForSeconds(0.1f);

        isDeadCheck = 1;

    }
}