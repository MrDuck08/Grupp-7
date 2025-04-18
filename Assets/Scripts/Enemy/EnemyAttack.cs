using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyAttack : MonoBehaviour
{

    #region General Info

    [SerializeField] Transform playerLocation;

    public bool playerInRange = true;
    bool currentlyAttacking = false;

    List<GameObject> allAttacksObjects = new List<GameObject>();
    List<GameObject> weakPointList = new List<GameObject>();
    GameObject attackObject;

    List<Vector3> weakPointTransformList = new List<Vector3>();
    List<Vector3> originalWeakPointScaleList = new List<Vector3>();
    List<Vector3> originalWeakPointTransformList = new List<Vector3>();
    Vector3 originalAttackScale;
    Vector3 originalAttackPos;

    #endregion

    [Header("s")]

    [SerializeField] int[] whatAttackInt;

    EnemyFollowPlayer enemyMovment;

    [Header("Attacks")]

    #region Stun

    [Header("Stun")]

    float stunTime;
    [SerializeField] float maxStunTime = 2f;

    bool stunned = false;
    public bool completeStop;

    #endregion

    #region Attacks

    GameObject player;

    #region Stretch Attack

    [Header("Stretch Attacks")]

    [SerializeField] float stretchSpeed;

    [Header("Stretch Times")]
    [SerializeField] float maxHowFastStretchAttack = 0.2f;
    float howFastAttack;
    [SerializeField] float maxHowLongStayAfterStretchAttack = 0.2f;
    float howLongStayAfterStretchAttack;
    [SerializeField] float maxHowFastGoBack = 0.3f;
    float howFastGoBack;

    [Header("Stretch Attack Values")]

    [SerializeField] float stretchAttackRange = 2f;

    bool startStretchAttack = false;
    bool startWaitingToGoBack = false;
    bool startGoingBack = false;

    RaycastHit2D attackRayHit;

    [SerializeField] LayerMask stopLayers;

    #endregion

    #region Charge Attack



    float playerDirection;
    float distanceToPlayer;

    bool anticipateCharge = false;
    bool startCharge = false;
    bool startChargeRecovery = false;

    [Header("Charge Attack")]


    [SerializeField] float howMoreToRunAfterCharge = 1;

    [SerializeField] float chargeSpeed = 6;

    [Header("Timers")]

    [SerializeField] float maxChargeStartupTime = 1f;
    float chargeStartupTime;
    float chargeTime;
    float maxChargeTime;
    [SerializeField] float maxChargeRecoveryTime = 0.5f;
    float chargeRecoveryTime;


    #endregion

    #region Jump Attack


    bool anticipateJump = false;
    bool jumpUp = false;
    bool jumpDown = false;

    [Header("Jump Attack")]

    [SerializeField] float maxJumpSpeed = 5;
    [SerializeField] float jumpSpeed;
    [SerializeField] float downAcceration = 1f;
    float jumpAcceration = 1f;

    float xHalfWayJump;
    float distanceToJumpPos;
    float extraJumpLenghtY;

    Vector2 jumpPos;

    [Header("Timers")]

    float jumpStartupTime;
    [SerializeField] float maxJumpStartupTime = 1;
    float timeForJumpUp;
    [SerializeField] float maxTimeForJump = 1;

    #endregion

    #endregion

    #region Boss

    [SerializeField] bool amIFinalBoss = false;

    bool feintCharge = false;
    bool feintJump = false;

    #endregion

    Rigidbody2D rigidbody2D;

    void Start()
    {
        Transform Attacks = transform.Find("EnemyAttackObjects");
        foreach (Transform child in Attacks.transform)
        {
            
            allAttacksObjects.Add(child.gameObject);

        }

        rigidbody2D = GetComponent<Rigidbody2D>();
        enemyMovment = GetComponent<EnemyFollowPlayer>();

    }

    void Update()
    {

        #region Stretch Attack

        if (startStretchAttack)
        {

            howFastAttack -= Time.deltaTime;

            attackObject.transform.position += attackObject.transform.right * stretchSpeed * Time.deltaTime;

            attackObject.transform.localScale += new Vector3(stretchSpeed * Time.deltaTime, 0, 0);

            foreach (GameObject weakPoints in weakPointList)
            {

                foreach (Vector3 weakPointsVector in weakPointTransformList)
                {

                    weakPoints.transform.localScale -= new Vector3(weakPointsVector.x / attackObject.transform.localScale.x / weakPointTransformList.Count * Time.deltaTime, 0, 0);

                }

            }

            if (howFastAttack <= 0)
            {
                startStretchAttack = false;
                startWaitingToGoBack = true;

                howFastAttack = maxHowFastStretchAttack;
                howLongStayAfterStretchAttack = maxHowLongStayAfterStretchAttack;

                if (attackObject.GetComponent<EnemyFollowPlayer>() != null)
                {
                    attackObject.GetComponent<BoxCollider2D>().enabled = false;
                }

            }
        }

        if (startWaitingToGoBack)
        {
            howLongStayAfterStretchAttack -= Time.deltaTime;

            if (howLongStayAfterStretchAttack <= 0)
            {
                startWaitingToGoBack = false;
                startGoingBack = true;

                howLongStayAfterStretchAttack = maxHowLongStayAfterStretchAttack;
                howFastGoBack = maxHowFastStretchAttack;
            }
        }

        if (startGoingBack)
        {
            howFastGoBack -= Time.deltaTime;

            attackObject.transform.position -= attackObject.transform.right * stretchSpeed * Time.deltaTime;

            attackObject.transform.localScale -= new Vector3(stretchSpeed * Time.deltaTime, 0);

            foreach (GameObject weakPoints in weakPointList)
            {

                foreach (Vector3 weakPointsVector in weakPointTransformList)
                {

                    weakPoints.transform.localScale += new Vector3(weakPointsVector.x / attackObject.transform.localScale.x / weakPointTransformList.Count * Time.deltaTime, 0, 0);

                }

            }

            if (howFastGoBack <= 0)
            {
                ResetAttack();

            }
        }

        #endregion

        #region Jump Attack

        if (anticipateJump)
        {

            jumpStartupTime -= Time.deltaTime;

            if(jumpStartupTime < 0)
            {

                attackObject.SetActive(true);

                if (feintCharge)
                {
                    Debug.Log(attackObject.transform.GetChild(0).gameObject.name);
                    attackObject.transform.GetChild(0).gameObject.SetActive(false);
                }

                if (attackObject.GetComponent<BoxCollider2D>() != null)
                {
                    attackObject.GetComponent<BoxCollider2D>().enabled = true;
                }

                anticipateJump = false;

                jumpPos = player.transform.position;

                distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);

                xHalfWayJump = distanceToPlayer / 2;
                extraJumpLenghtY = distanceToPlayer;

                distanceToJumpPos = Vector2.Distance(new Vector2(jumpPos.x - xHalfWayJump * playerDirection, jumpPos.y + extraJumpLenghtY), transform.position);

                jumpSpeed = maxJumpSpeed;

                timeForJumpUp = distanceToJumpPos * 2 / jumpSpeed;

                jumpAcceration = -jumpSpeed / timeForJumpUp;

                jumpUp = true;

            }

        }

        if (jumpUp)
        {

            timeForJumpUp -= Time.deltaTime;

            rigidbody2D.gravityScale = 0;

            jumpSpeed += jumpAcceration * Time.deltaTime;


            transform.position = Vector2.MoveTowards(transform.position, new Vector2(jumpPos.x - xHalfWayJump * playerDirection, jumpPos.y + extraJumpLenghtY), jumpSpeed * Time.deltaTime);

            if(timeForJumpUp < 0)
            {

                jumpUp = false;
                jumpDown = true;

                jumpSpeed = maxJumpSpeed;


                distanceToJumpPos = Vector2.Distance(jumpPos, transform.position);

                timeForJumpUp = distanceToJumpPos * 2 / jumpSpeed;

                //maxTimeForJump = timeForJumpUp;

                jumpAcceration = downAcceration;

                if (feintJump)
                {
                    jumpPos.x -= xHalfWayJump * playerDirection;

                    distanceToJumpPos = Vector2.Distance(jumpPos, transform.position);

                    timeForJumpUp = distanceToJumpPos * 2 / jumpSpeed;

                    jumpAcceration *= 4;

                }

                jumpSpeed = 0;

            }

        }

        if (jumpDown)
        {

            timeForJumpUp -= Time.deltaTime;

            jumpSpeed += jumpAcceration * Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, jumpPos, jumpSpeed * Time.deltaTime);

            if (transform.position == new Vector3(jumpPos.x, jumpPos.y))
            {

                if (attackObject.GetComponent<BoxCollider2D>() != null)
                {
                    attackObject.GetComponent<BoxCollider2D>().enabled = false;
                }

                ResetAttack();

                if (feintJump)
                {

                    anticipateCharge = true;
                    currentlyAttacking = true;
                    enemyMovment.stop = true;

                    chargeStartupTime = 0;

                    originalWeakPointTransformList.Clear();
                    originalWeakPointScaleList.Clear();
                    weakPointTransformList.Clear();
                    weakPointList.Clear();

                    attackObject = gameObject.transform.GetChild(1).transform.Find("Charge Attack").gameObject;
                    originalAttackPos = attackObject.transform.localPosition;
                    originalAttackScale = attackObject.transform.localScale;

                    Transform AttackObject = attackObject.transform.Find("WeakPointCollection");
                    foreach (Transform child in AttackObject.transform)
                    {
                        originalWeakPointTransformList.Add(child.localPosition);
                        originalWeakPointScaleList.Add(child.localScale);
                        weakPointTransformList.Add(child.localScale);
                        weakPointList.Add(child.gameObject);
                    }

                    playerDirection = Mathf.Sign(player.transform.position.x - transform.position.x);

                }

            }

        }

        #endregion

        #region Charge Attack

        if (anticipateCharge)
        {

            transform.position -= new Vector3(0.1f * playerDirection * Time.deltaTime, 0);
            chargeStartupTime -= Time.deltaTime;


            if (chargeStartupTime < 0)
            {

                Debug.Log("Charge");
                attackObject.SetActive(true);

                if (attackObject.GetComponent<BoxCollider2D>() != null)
                {
                    attackObject.GetComponent<BoxCollider2D>().enabled = true;
                }
                if (attackObject.GetComponent<CapsuleCollider2D>() != null)
                {
                    attackObject.GetComponent<CapsuleCollider2D>().enabled = true;
                }

                distanceToPlayer = Vector3.Distance(player.transform.position, transform.position) + howMoreToRunAfterCharge;
                
                maxChargeTime = distanceToPlayer / chargeSpeed;
                chargeTime = maxChargeTime;

                anticipateCharge = false;
                startCharge = true;
            }

        }

        if (startCharge)
        {

            Vector2 MovePos = new Vector2(transform.position.x + playerDirection * chargeSpeed * Time.deltaTime, transform.position.y);

            transform.position = MovePos;

            chargeTime -= Time.deltaTime;

            if (chargeTime < 0)
            {
                Debug.Log("Charge Done");
                if (attackObject.GetComponent<BoxCollider2D>() != null)
                {
                    attackObject.GetComponent<BoxCollider2D>().enabled = false;
                }
                if (attackObject.GetComponent<CapsuleCollider2D>() != null)
                {
                    attackObject.GetComponent<CapsuleCollider2D>().enabled = false;
                }

                chargeRecoveryTime = maxChargeRecoveryTime;
                chargeRecoveryTime = 3;
                startCharge = false;
                startChargeRecovery = true;

            }
        }

        if (startChargeRecovery)
        {

            chargeRecoveryTime -= Time.deltaTime;

            if (chargeRecoveryTime < 0)
            {
                feintJump = false;

                Debug.Log("Charge Recovery");
                ResetAttack();
            }

        }

        #endregion

        #region Stun

        if (stunned)
        {

            stunTime -= Time.deltaTime;

            if(stunTime < 0)
            {

                stunned = false;

                enemyMovment.stop = false;
            }

        }

        #endregion

    }

    public void Attack()
    {

        if (completeStop) // Knockback �r aktiv
        {

            return;

        }
        Debug.Log("New Attack");
        #region General Info Gathering

        player = GameObject.FindGameObjectWithTag("Player");

        currentlyAttacking = true;
        enemyMovment.stop = true;

        int whatAttack = Random.Range(0, allAttacksObjects.Count);
        whatAttack = 0;
        attackObject = allAttacksObjects[whatAttack];
        originalAttackPos = attackObject.transform.localPosition;
        originalAttackScale = attackObject.transform.localScale;

        Transform AttackObject = attackObject.transform.Find("WeakPointCollection");
        foreach (Transform child in AttackObject.transform)
        {
            originalWeakPointTransformList.Add(child.localPosition);
            originalWeakPointScaleList.Add(child.localScale);
            weakPointTransformList.Add(child.localScale);
            weakPointList.Add(child.gameObject);
        }

        #endregion

        switch (whatAttackInt[whatAttack])
        {
            case 0: // Stretch Attack

                #region Stretch Attack

                Vector3 whereToLook = player.transform.position - attackObject.transform.position; // F�r Vart Den Ska Titta
                float angle = Mathf.Atan2(whereToLook.y, whereToLook.x) * Mathf.Rad2Deg; // F�r Vinkeln D�r Den Ska Titta
                attackObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // S�tter Rotationen


                attackRayHit = Physics2D.Raycast(attackObject.transform.position, whereToLook, Mathf.Infinity, stopLayers);

                if (attackRayHit.collider != null)
                {

                    if(attackRayHit.distance/2 <= stretchAttackRange)
                    {

                        attackObject.SetActive(true);
    
                        maxHowFastStretchAttack = (attackRayHit.distance / 2) / stretchSpeed; // Dividera Med 2 F�r Att T�nka P� Att Skalan �kas Ocks�
                        howFastAttack = maxHowFastStretchAttack;

                        if (attackObject.GetComponent<EnemyFollowPlayer>() != null)
                        {
                            attackObject.GetComponent<BoxCollider2D>().enabled = true;
                        }

                        startStretchAttack = true;

                        
                    }
                    else
                    {

                        startStretchAttack = false;
                        currentlyAttacking = false;
                        enemyMovment.stop = false;
                        // Not In Range
                        // Check If Enemy Attack Again
                    }

                }
                else
                {
                    ResetAttack();
                }

                #endregion

                break;

            case 1: // Charge Attack

                #region Charge Attack

                playerDirection = Mathf.Sign(player.transform.position.x - transform.position.x); // F�r Om Den Position i korrelation med fiendes position och sen ger 1 eller -1

                chargeStartupTime = maxChargeStartupTime;

                if (amIFinalBoss)
                {

                    int feintOrNot = Random.Range(0, 2);

                    switch (feintOrNot)
                    {

                        case 0:

                            feintCharge = false;

                            break;

                        case 1:

                            feintCharge = true;

                            break; 

                    }

                }

                feintCharge = true;

                anticipateCharge = true;

                #endregion

                break;

            case 2: // Jump Attack

                #region Jump Attack

                playerDirection = Mathf.Sign(player.transform.position.x - transform.position.x);

                jumpStartupTime = maxJumpStartupTime;

                if (amIFinalBoss)
                {

                    int feintOrNot = Random.Range(0, 2);

                    switch (feintOrNot)
                    {

                        case 0:

                            feintJump = false;

                            break;

                        case 1:

                            feintJump = true;

                            break;

                    }

                }

                feintJump = true;

                anticipateJump = true;

                #endregion

                break;
        }
    }

    #region Reset Attack

    public void ResetAttack()
    {

        for(int i = 0; i < weakPointList.Count; i++)
        {

            weakPointList[i].gameObject.transform.localPosition = originalWeakPointTransformList[i];
            weakPointList[i].gameObject.transform.localScale = originalWeakPointScaleList[i];

        }

        startGoingBack = false;

        if (!completeStop) // Om completeStop �r sann d� k�rs knockback och d� ska stop vara true
        {
            enemyMovment.stop = false;
        }
        Debug.Log("Reset " + feintJump + " Feint or not");
        rigidbody2D.gravityScale = 1;

        howFastGoBack = maxHowFastGoBack;
        howFastAttack = maxHowFastStretchAttack;
        howLongStayAfterStretchAttack = maxHowLongStayAfterStretchAttack;

        chargeStartupTime = maxChargeStartupTime;
        chargeTime = maxChargeTime;
        chargeRecoveryTime = maxChargeRecoveryTime;

        jumpStartupTime = maxJumpStartupTime;
        timeForJumpUp = maxTimeForJump;

        anticipateCharge = false;
        startCharge = false;
        startChargeRecovery = false;

        startStretchAttack = false;
        startWaitingToGoBack = false;
        startGoingBack = false;
        currentlyAttacking = false;

        anticipateJump = false;
        jumpUp = false;
        jumpDown = false;

        originalWeakPointTransformList.Clear();
        originalWeakPointScaleList.Clear();
        weakPointList.Clear();
        weakPointTransformList.Clear();

        if (attackObject != null)
        {
            attackObject.transform.localPosition = originalAttackPos;

            attackObject.transform.localScale = originalAttackScale;

            attackObject.SetActive(false);
        }

        if (!feintJump)
        {


        }

    }

    #endregion

    #region Trigger

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (startCharge && collision.gameObject.layer == 3)
        {

            ResetAttack();

            stunTime = maxStunTime;

            stunned = true;

            enemyMovment.stop = true;

        }
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 direction = playerLocation.position - transform.position;
        //Gizmos.DrawRay(transform.position, direction);
    }

}
