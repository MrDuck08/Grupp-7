using UnityEngine;

public class PlayerWeaponBase : MonoBehaviour
{
    #region Mouse

    [SerializeField] Camera cam;

    Vector2 mousePos;

    Rigidbody2D rb;

    [SerializeField] Transform playerTransform;

    bool testBool = true;

    #endregion


    // Skit att fixa:

    // Ground




    void Start()
    {
        rb = GetComponent<Rigidbody2D>();



    }

    void Update()
    {

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if(Input.GetMouseButtonDown(1))
        {
            playerTransform.position += new Vector3(1,0,0);
        }


    }

    private void FixedUpdate()
    {


        if (testBool)
        {
            Vector2 lookDirection = mousePos - rb.position;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            Debug.Log("TEst");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TEST 1");
        if (collision.gameObject.tag == "Ground")
        {
            Debug.Log("TEst");
        }
    }

}
