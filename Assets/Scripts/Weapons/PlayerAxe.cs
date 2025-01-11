using UnityEngine;

public class PlayerAxe : WeaponBase
{
    [Header("General")]

    [SerializeField] Transform parentTransform;

    PlayerWeaponBase playerWeaponBase;
    EnemyHealth enemyHealth;

    BoxCollider2D axeHeadCollider;

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

    #region Bool

    public bool stayUnsheathed = true;

    bool startCharge = false;
    bool startAttack = false;
    bool startGoingBack = false;
    bool startSwingDown = false;
    bool attacking = false;
    bool switchedToRight = false;
    bool switchedToLeft = false;
    bool firstTimeSwitching = true;

    public bool rightSideAxe = true;


    #endregion

    public override void Start()
    {
        base.Start();

        playerWeaponBase = FindFirstObjectByType<PlayerWeaponBase>();
        enemyHealth = FindFirstObjectByType<EnemyHealth>();

        axeHeadCollider = GetComponentInChildren<BoxCollider2D>();

        axeHeadCollider.enabled = false;

        timeUntilAttack = maxtTimeUntilAttack;
        howFastAttack = maxHowFastAttack;
        timeToSwingDown = maxTimeToSwingDown;
        howFastGoBack = maxHowFastGoBack;
    }

    public override void Update()
    {
        base.Update();


        #region Attack

        if (startCharge && stayUnsheathed && attacking)
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

                axeHeadCollider.enabled = true;


            }
        }

        if (startAttack && stayUnsheathed && attacking)
        {
            howFastAttack -= Time.deltaTime;
            maxDisatnceBetweenPlayerAndSword += speed * Time.deltaTime;


            if (howFastAttack <= 0)
            {
                startAttack = false;
                startSwingDown = true;

                howFastAttack = maxHowFastAttack;

                float distanceToSwingDown = maxDistanceToLookUpWhenAiming + maxDistanceToLookDownInSwing;

                howFastToChangeWhereAiming = distanceToSwingDown/ timeToSwingDown;

            }
        }

        if (startSwingDown && stayUnsheathed && attacking)
        {
            timeToSwingDown -= Time.deltaTime;
            playerWeaponBase.WhereToLookOfset -= howFastToChangeWhereAiming * Time.deltaTime;


            if (timeToSwingDown <= 0)
            {
                startGoingBack = true;
                startSwingDown = false;

                timeToSwingDown = maxTimeToSwingDown;

                attackDistance = attackRange;

                speed = attackDistance / howFastGoBack;
                howFastToChangeWhereAiming = maxDistanceToLookDownInSwing / howFastGoBack;

                axeHeadCollider.enabled = false;

            }
        }

        if (startGoingBack && stayUnsheathed && attacking)
        {
            howFastGoBack -= Time.deltaTime;
            maxDisatnceBetweenPlayerAndSword -= speed * Time.deltaTime;
            playerWeaponBase.WhereToLookOfset += howFastToChangeWhereAiming * Time.deltaTime;

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

        stopAttacking = false;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        float dis = Vector3.Distance(transform.position, parentTransform.position);

        if (!stayUnsheathed)
        {
            maxDisatnceBetweenPlayerAndSword = 0.1f;

            ResetAttack();
        }

        if (stayUnsheathed && !attacking)
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

        howFastToChangeWhereAiming = maxDistanceToLookUpWhenAiming / timeUntilAttack;

    }

    #region Side Switch

    public void SideSwitch()
    {
        if (!attacking)
        {
            if (rightSideAxe && !switchedToRight)
            {

                switchedToRight = true;
                switchedToLeft = false;

                if (!firstTimeSwitching)
                {

                    maxDistanceToLookUpWhenAiming *= -1;
                    maxDistanceToLookDownInSwing *= -1;

                    Vector3 scaler = transform.localScale; // Vänd Grafiken
                    scaler.y *= -1; // Vänd Grafiken
                    transform.localScale = scaler; // Vänd Grafiken
                }

                firstTimeSwitching = false;
            }

            if(!rightSideAxe && !switchedToLeft)
            {
                 
                switchedToLeft = true;
                switchedToRight = false;
                maxDistanceToLookUpWhenAiming *= -1;
                maxDistanceToLookDownInSwing *= -1;

                Vector3 scaler = transform.localScale; // Vänd Grafiken
                scaler.y *= -1; // Vänd Grafiken
                transform.localScale = scaler; // Vänd Grafiken

                firstTimeSwitching = false;

            }
        }
    }

    #endregion

    public void ResetAttack()
    {

        startGoingBack = false;
        startAttack = false;
        startSwingDown = false;
        startCharge = false;
        attacking = false;



        timeUntilAttack = maxtTimeUntilAttack;
        howFastAttack = maxHowFastAttack;
        timeToSwingDown = maxTimeToSwingDown;
        howFastGoBack = maxHowFastGoBack;
        howFastAttack = maxHowFastAttack;

        playerWeaponBase = FindFirstObjectByType<PlayerWeaponBase>();

        playerWeaponBase.WhereToLookOfset = 0;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Weak point")
        {
            enemyHealth.TakeDamage(2);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            enemyHealth.TakeDamage(1);
        }
        if (collision.gameObject.tag == "EnemyAttack")
        {
            enemyHealth.TakeDamage(1);
        }
    }
}
