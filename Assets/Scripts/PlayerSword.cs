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
    float howFastGoBack;
    [SerializeField] float maxHowFastGoBack = 0.3f;

    [SerializeField] float maxDisatnceBetweenPlayerAndSwordUnsheathed = 1.3f;

    #endregion

    [Header("Check If Object")]

    [SerializeField] GameObject checkToLeaveObject;
    Collider2D checkToLeaveObjectCollider;

    #region Bool

    public bool stayUnsheathed = true;

    bool startCharge = false;
    bool startAttack = false;
    bool startGoingBack = false;
    bool attacking = false;

    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        timeUntilAttack = maxtTimeUntilAttack;
        howFastAttack = maxHowFastAttack;
        howFastGoBack = maxHowFastGoBack;
    }

    void Update()
    {
        #region Attack

        if (startCharge && stayUnsheathed)
        {
            timeUntilAttack -= Time.deltaTime;
            maxDisatnceBetweenPlayerAndSword -= speed * Time.deltaTime;

            if (timeUntilAttack <= 0)
            {
                startAttack = true;
                startCharge = false;
                attacking = true;
                timeUntilAttack = maxtTimeUntilAttack;

                attackDistance = attackRange + maxDisatnceBetweenPlayerAndSwordUnsheathed;

                speed = attackDistance/ howFastAttack;

            }
        }

        if (startAttack && stayUnsheathed)
        {
            howFastAttack -= Time.deltaTime;
            maxDisatnceBetweenPlayerAndSword += speed * Time.deltaTime;

            if (howFastAttack <= 0)
            {
                startAttack = false;
                startGoingBack = true;

                howFastAttack = maxHowFastAttack;

                attackDistance = attackRange;

                speed = attackDistance / howFastGoBack;

            }
        }

        if (startGoingBack && stayUnsheathed)
        {
            howFastGoBack -= Time.deltaTime;
            maxDisatnceBetweenPlayerAndSword -= speed * Time.deltaTime;

            if (howFastGoBack <= 0)
            {
                startGoingBack = false;
                attacking = false;

                howFastGoBack = maxHowFastGoBack;

            }
        }

        #endregion

        if (Input.GetMouseButtonDown(0) && stayUnsheathed && !attacking)
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

            startGoingBack = false;
            startAttack = false;    
            startCharge = false;
            attacking = false;

            timeUntilAttack = maxtTimeUntilAttack;
            howFastAttack = maxHowFastAttack;
            howFastGoBack = maxHowFastGoBack;
        }

        if(stayUnsheathed && !attacking)
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
        attacking = true;
        startCharge = true;

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

    }

}
