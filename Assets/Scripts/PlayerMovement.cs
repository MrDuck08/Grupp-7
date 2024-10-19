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

    public LayerMask jumpableLayer;

    public bool grounded;

    bool facingRight = true;

    float xInput;

    float yInput;


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

        body.linearVelocity = new Vector2(direction.x * groundSpeed, body.linearVelocityY);
        CheckGround();
        HandleJump();

    }
    private void FixedUpdate(){

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

                body.AddForce(new Vector2(0, jumpSpeed) , ForceMode2D.Impulse);
            }
    }

    void Flip()
    {

        // Vänd sprite:n genom att spegla den på x-axeln
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;

        Transform weaponBase = transform.Find("PlayerWeaponAll"); // Hittar Childen PlayerWeaponAll
        if (weaponBase != null) // Kolllar Om Den Hittades
        {
            weaponBase.transform.localScale = scaler; // Vända Vapnerna
        }
    }


    private void CheckGround()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.transform.position, 0.35f, jumpableLayer);
    }

    void ApplyFriction()
    {

        if (grounded && xInput == 0 && body.linearVelocity.y <= 0)
        {
            body.linearVelocity *= groundDecay;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;


    }
}
