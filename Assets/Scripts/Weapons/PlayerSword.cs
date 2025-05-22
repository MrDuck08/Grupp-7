using Unity.VisualScripting;
using UnityEngine;

public class PlayerSword : WeaponBase
{
    [Header("General")]

    Vector2 mousePos;
    [SerializeField] Camera cam;

    Rigidbody2D rb;

    Transform beforeAttackTransform;

    [SerializeField] Transform parentTransform;

    EnemyHealth enemyHealth;
    AudioManager audioManager;
    PlayerMovement playerMovement;

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
    [SerializeField] float disToAttack = 0.35f;

    #endregion

    [Header("Check If Object")]

    [SerializeField] GameObject checkToLeaveObject;
    Collider2D attackCollider;

    #region Bool

    public bool stayUnsheathed = true;

    bool startCharge = false;
    bool startAttack = false;
    bool startGoingBack = false;
    bool attacking = false;

    #endregion

    public override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        attackCollider = GetComponent<Collider2D>();

        enemyHealth = FindFirstObjectByType<EnemyHealth>();
        audioManager = FindFirstObjectByType<AudioManager>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();

        attackCollider.enabled = false;

        timeUntilAttack = maxtTimeUntilAttack;
        howFastAttack = maxHowFastAttack;
        howFastGoBack = maxHowFastGoBack;
    }

    public override void Update()
    {
        base.Update();
        #region Attack

        if (startCharge && stayUnsheathed)
        {
            timeUntilAttack -= Time.deltaTime;
            maxDisatnceBetweenPlayerAndSword -= speed * Time.deltaTime;

            if (timeUntilAttack <= 0)
            {
                attackCollider.enabled = true;

                startAttack = true;
                startCharge = false;
                attacking = true;
                timeUntilAttack = maxtTimeUntilAttack;

                attackDistance = attackRange + (maxDisatnceBetweenPlayerAndSwordUnsheathed - disToAttack);

                speed = attackDistance/ howFastAttack;
                audioManager.SwordSound();

                // Aktivera Attack Hitbox

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

                attackCollider.enabled = false;

                // Stäng Av Attack Hitbox

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


        float dis = Vector3.Distance(transform.position, parentTransform.position);

        if (!stayUnsheathed)
        {
            maxDisatnceBetweenPlayerAndSword = 0.1f;

            ResetAttack();
        }

        if (stayUnsheathed && !attacking)
        {
            maxDisatnceBetweenPlayerAndSword = maxDisatnceBetweenPlayerAndSwordUnsheathed;
            Debug.Log("Distance = MaxDistance");
        }

        Debug.Log(stayUnsheathed + " StayYncheated " + attacking + " Are Attacking " + dis + " dis " + (transform.position - parentTransform.position).normalized + " What dis?");

        if (dis != maxDisatnceBetweenPlayerAndSword)
        {
            dis = maxDisatnceBetweenPlayerAndSword;
            transform.position = (transform.position - parentTransform.position).normalized * dis + parentTransform.position;
        }

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();


    }

    void Attack()
    {
        speed = (maxDisatnceBetweenPlayerAndSwordUnsheathed - disToAttack) / timeUntilAttack;
        attacking = true;
        startCharge = true;

    }

    public void ResetAttack()
    {

        startGoingBack = false;
        startAttack = false;
        startCharge = false;
        attacking = false;
        stayUnsheathed = true;



        timeUntilAttack = maxtTimeUntilAttack;
        howFastAttack = maxHowFastAttack;
        howFastGoBack = maxHowFastGoBack;
        howFastAttack = maxHowFastAttack;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        switch (collision.transform.tag)
        {
            case "WeakPoint":

                if (collision.transform.parent.transform.parent.transform.parent.transform.parent.gameObject != null)
                {
                    collision.transform.parent.transform.parent.transform.parent.transform.parent.GetComponent<EnemyHealth>().TakeDamageInfo(2);
                }

                break;

            case "Weakpoint2":

                if (collision.transform.parent.transform.parent != null) 
                {
                    collision.transform.parent.transform.parent.GetComponent<EnemyHealth>().TakeDamageInfo(2);
                }

                break;

        case "Enemy":

            collision.GetComponent<EnemyHealth>().TakeDamageInfo(1);

            break;

        case "EnemyAttack":

            collision.transform.parent.transform.parent.GetComponent<EnemyHealth>().TakeDamageInfo(1);

            break;

        case "TutorialWeakPoint":

            playerMovement.dashHasReset = true;

            break;

        }
    }


}
