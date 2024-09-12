using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    Vector2 mousePos;
    [SerializeField] Camera cam;

    Rigidbody2D rb;

    Transform beforeAttackTransform;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);



        //Vector3 ss = transform.position - mousePos;

        //transform.LookAt(ss);

        if (Input.GetMouseButtonDown(0))
        {

            Attack();

        }
    }

    void Attack()
    {
        beforeAttackTransform = transform;

        Vector2 lookDirection = mousePos - rb.position;
        transform.position = lookDirection * 100 * Time.deltaTime;
        
    }
}
