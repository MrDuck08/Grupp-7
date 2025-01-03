using UnityEditor.PackageManager.UI;
using UnityEngine;

public class MiniGamePlayer : MonoBehaviour
{

    [SerializeField] float moveSpeed = 5;

    Vector2 moveInput;

    Rigidbody2D rb2D;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        rb2D.linearVelocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * moveSpeed);

    }

}
