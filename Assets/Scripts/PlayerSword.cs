using Unity.VisualScripting;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    Vector2 mousePos;
    [SerializeField] Camera cam;

    Rigidbody2D rb;

    Transform beforeAttackTransform;

    [SerializeField] Transform parentTransform;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {


        if (Input.GetMouseButtonDown(0))
        {

            Attack();

        }
    }

    private void FixedUpdate()
    {
        //rb.AddForce(transform.right * 10 * Time.deltaTime);

        Vector3 tr = parentTransform.position - transform.position;

        float dis = Vector3.Distance(transform.position, parentTransform.position);

        if (dis != 1.3f)
        {
            dis = 1.3f;
            transform.position = (transform.position - parentTransform.position).normalized * dis + parentTransform.position;
        }
    }

    void Attack()
    {
        beforeAttackTransform = transform;

        Vector2 lookDirection = mousePos - rb.position;


        
    }
}
