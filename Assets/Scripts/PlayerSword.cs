using Unity.VisualScripting;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    [Header("General")]

    Vector2 mousePos;
    [SerializeField] Camera cam;

    Rigidbody2D rb;

    Transform beforeAttackTransform;

    [SerializeField] Transform parentTransform;

    #region Float

    [Header("Attack")]

    float timeUntilAttack;
    [SerializeField] float maxtTimeUntilAttack = 1f;
    float maxDisatnceBetweenPlayerAndSword = 1.3f;
    float speed;

    [SerializeField] float attackRange = 2;
    float attackDistance;
    float howFastAttack;
    [SerializeField] float maxHowFastAttack = 0.2f;

    [SerializeField] float maxDisatnceBetweenPlayerAndSwordUnsheathed = 1.3f;

    #endregion

    [Header("Check If Object")]

    [SerializeField] GameObject checkToLeaveObject;
    Collider2D checkToLeaveObjectCollider;

    #region Bool

    public bool stayUnsheathed = true;

    bool startCharge = false;
    bool startAttack = false;

    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        timeUntilAttack = maxtTimeUntilAttack;
        howFastAttack = maxHowFastAttack;
    }

    void Update()
    {
        if (startCharge)
        {
            timeUntilAttack -= Time.deltaTime;
            maxDisatnceBetweenPlayerAndSword -= speed * Time.deltaTime;

            if (timeUntilAttack <= 0)
            {
                startAttack = true;
                startCharge = false;
                timeUntilAttack = maxtTimeUntilAttack;

                attackDistance = attackRange + maxDisatnceBetweenPlayerAndSwordUnsheathed;

                speed = attackDistance/ howFastAttack;

            }
        }

        if (startAttack)
        {
            howFastAttack -= Time.deltaTime;
            maxDisatnceBetweenPlayerAndSword += speed * Time.deltaTime;

            if (howFastAttack <= 0)
            {
                startAttack = false;

                howFastAttack = maxtTimeUntilAttack;
                howFastAttack = maxHowFastAttack;

            }
        }

        if (Input.GetMouseButtonDown(0))
        {

            Attack();

        }
    }

    private void FixedUpdate()
    {

        Vector3 tr = parentTransform.position - transform.position;

        float dis = Vector3.Distance(transform.position, parentTransform.position);

        if (!stayUnsheathed)
        {
            maxDisatnceBetweenPlayerAndSword = 0.1f;
        }

        if(stayUnsheathed && !startCharge && !startAttack)
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
        speed = maxDisatnceBetweenPlayerAndSwordUnsheathed / timeUntilAttack;

        startCharge = true;

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

    }

}
