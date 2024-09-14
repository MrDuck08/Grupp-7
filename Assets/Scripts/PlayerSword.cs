using Unity.VisualScripting;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    Vector2 mousePos;
    [SerializeField] Camera cam;

    Rigidbody2D rb;

    Transform beforeAttackTransform;

    [SerializeField] Transform parentTransform;

    #region Float

    float timeUntilAttack = 0.4f;
    float maxDisatnceBetweenPlayerAndSword = 1.3f;
    float attackRange = 2;

    [SerializeField] float maxDisatnceBetweenPlayerAndSwordUnsheathed = 1.3f;

    #endregion

    [SerializeField] GameObject checkToLeaveObject;
    Collider2D checkToLeaveObjectCollider;

    #region Bool

    public bool stayUnsheathed = true;

    bool startAttack = false;

    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Fixa Atack

    void Update()
    {
        if (startAttack)
        {
            timeUntilAttack -= Time.deltaTime;

            if (timeUntilAttack <= 0)
            {
                startAttack = false;

            }
        }

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

        if (!stayUnsheathed)
        {
            maxDisatnceBetweenPlayerAndSword = 0.1f;
        }
        else
        {
            maxDisatnceBetweenPlayerAndSword = maxDisatnceBetweenPlayerAndSwordUnsheathed;
        }

        if (dis != maxDisatnceBetweenPlayerAndSword)
        {
            dis = maxDisatnceBetweenPlayerAndSword;
            transform.position = (transform.position - parentTransform.position).normalized * dis + parentTransform.position;
        }
    }

    void Attack()
    {

        startAttack = true;

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

    }

}
