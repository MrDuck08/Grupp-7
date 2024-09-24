using UnityEngine;

public class PlayerAxe : MonoBehaviour
{
    [Header("General")]

    Vector2 mousePos;
    [SerializeField] Camera cam;

    Rigidbody2D rb;

    Transform beforeAttackTransform;

    [SerializeField] Transform parentTransform;

    PlayerWeaponBase playerWeaponBase;

    #region Float

    [Header("Attack")]

    float maxDisatnceBetweenPlayerAndSword = 1.3f;
    float speed;
    float howFastToChangeWhereAiming;

    [SerializeField] float attackRange = 2;
    float attackDistance;

    float timeUntilAttack;
    [SerializeField] float maxtTimeUntilAttack = 1f;
    float howFastAttack;
    [SerializeField] float maxHowFastAttack = 0.2f;
    float timeToSwingDown;
    [SerializeField] float maxTimeToSwingDown = 0.2f;
    float howFastGoBack;
    [SerializeField] float maxHowFastGoBack = 0.3f;

    [SerializeField] float maxDisatnceBetweenPlayerAndSwordUnsheathed = 1.3f;

    [SerializeField] float maxDistanceToLookUpWhenAiming = 10f;
    [SerializeField] float maxDistanceToLookDownInSwing = 10f;

    #endregion

    [Header("Check If Object")]

    [SerializeField] GameObject checkToLeaveObject;
    Collider2D checkToLeaveObjectCollider;

    #region Bool

    public bool stayUnsheathed = true;

    bool startCharge = false;
    bool startAttack = false;
    bool startGoingBack = false;
    bool startSwingDown = false;
    bool attacking = false;

    public bool testBool = false;

    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        playerWeaponBase = FindFirstObjectByType<PlayerWeaponBase>();

        timeUntilAttack = maxtTimeUntilAttack;
        howFastAttack = maxHowFastAttack;
        timeToSwingDown = maxTimeToSwingDown;
        howFastGoBack = maxHowFastGoBack;
    }

    void Update()
    {
        #region Attack

        if (startCharge && stayUnsheathed)
        {
            timeUntilAttack -= Time.deltaTime;
            maxDisatnceBetweenPlayerAndSword -= speed * Time.deltaTime;
            playerWeaponBase.WhereToLookOfset += howFastToChangeWhereAiming * Time.deltaTime;

            if (timeUntilAttack <= 0)
            {
                startAttack = true;
                startCharge = false;

                timeUntilAttack = maxtTimeUntilAttack;

                attackDistance = attackRange + maxDisatnceBetweenPlayerAndSwordUnsheathed;

                speed = attackDistance / howFastAttack;


            }
        }

        if (startAttack && stayUnsheathed)
        {
            howFastAttack -= Time.deltaTime;
            maxDisatnceBetweenPlayerAndSword += speed * Time.deltaTime;


            if (howFastAttack <= 0)
            {
                startAttack = false;
                startSwingDown = true;

                attackDistance = attackRange;

                float distanceToSwingDown = maxDistanceToLookUpWhenAiming + maxDistanceToLookDownInSwing;

                howFastToChangeWhereAiming = distanceToSwingDown/ timeToSwingDown;

            }
        }

        if (startSwingDown && stayUnsheathed)
        {
            timeToSwingDown -= Time.deltaTime;
            playerWeaponBase.WhereToLookOfset -= howFastToChangeWhereAiming * Time.deltaTime;


            if (timeToSwingDown <= 0)
            {
                startGoingBack = true;
                startSwingDown = false;

                timeToSwingDown = maxTimeToSwingDown;


                speed = attackDistance / howFastGoBack;
                howFastToChangeWhereAiming = maxDistanceToLookDownInSwing / howFastGoBack;

            }
        }

        if (startGoingBack && stayUnsheathed)
        {
            howFastGoBack -= Time.deltaTime;
            maxDisatnceBetweenPlayerAndSword -= speed * Time.deltaTime;
            playerWeaponBase.WhereToLookOfset += howFastToChangeWhereAiming * Time.deltaTime;

            if (howFastGoBack <= 0)
            {
                startGoingBack = false;
                attacking = false;
                testBool = false;

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

        if (stayUnsheathed && !attacking && !testBool)
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
        testBool = true;
        howFastToChangeWhereAiming = maxDistanceToLookUpWhenAiming / timeUntilAttack;

    }
}
