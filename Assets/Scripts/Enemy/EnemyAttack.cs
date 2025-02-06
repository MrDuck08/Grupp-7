using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
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

    [Header("s")]

    [SerializeField] int[] whatAttackInt;

    EnemyFollowPlayer enemyMovment;

    [Header("Attacks")]

    #region Stun

    [Header("Stun")]

    float stunTime;
    [SerializeField] float maxStunTime = 2f;

    bool stunned = false;

    #endregion

    #region Attacks

    GameObject player;

    #region Stretch Attack

    [Header("Stretch Attacks")]

    [SerializeField] float stretchSpeed;

    [Header("Stretch Times")]
    float howFastAttack;
    [SerializeField] float maxHowFastStretchAttack = 0.2f;
    float howLongStayAfterStretchAttack;
    [SerializeField] float maxHowLongStayAfterStretchAttack = 0.2f;
    float howFastGoBack;
    [SerializeField] float maxHowFastGoBack = 0.3f;

    [SerializeField] float stretchAttackRange = 2f;

    bool startStretchAttack = false;
    bool startWaitingToGoBack = false;
    bool startGoingBack = false;

    RaycastHit2D attackRayHit;

    [SerializeField] LayerMask stopLayers;

    #endregion

    #region Charge Attack

    [Header("Charge Attack")]

    float playerDirection;
    float distanceToPlayer;

    bool anticipateCharge = false;
    bool startCharge = false;
    bool startChargeRecovery = false;

    [Header("Attack Values")]

    [SerializeField] float howMoreToRunAfterCharge = 1;

    [SerializeField] float chargeSpeed = 6;

    [Header("Timers")]

    float chargeStartupTime;
    [SerializeField] float maxChargeStartupTime = 1f;
    float chargeTime;
    float maxChargeTime;
    float chargeRecoveryTime;
    [SerializeField] float maxChargeRecoveryTime = 0.5f;


    #endregion

    #endregion




    void Start()
    {
        Transform Attacks = transform.Find("EnemyAttackObjects");
        foreach (Transform child in Attacks.transform)
        {
            
            allAttacksObjects.Add(child.gameObject);

        }

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

                foreach(Vector3 weakPointsVector in weakPointTransformList)
                {

                    weakPoints.transform.localScale -= new Vector3(weakPointsVector.x / attackObject.transform.localScale.x/ weakPointTransformList.Count * Time.deltaTime, 0, 0);

                }

            }

            if (howFastAttack <= 0)
            {
                startStretchAttack = false;
                startWaitingToGoBack = true;

                howFastAttack = maxHowFastStretchAttack;
                howLongStayAfterStretchAttack = maxHowLongStayAfterStretchAttack;

                if(attackObject.GetComponent<EnemyFollowPlayer>() != null)
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

        #region Charge Attack

        if (anticipateCharge)
        {

            chargeStartupTime -= Time.deltaTime;


            if(chargeStartupTime < 0)
            {

                attackObject.SetActive(true);

                if(attackObject.GetComponent<BoxCollider2D>() != null)
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

                if (attackObject.GetComponent<BoxCollider2D>() != null)
                {
                    attackObject.GetComponent<BoxCollider2D>().enabled = false;
                }
                if (attackObject.GetComponent<CapsuleCollider2D>() != null)
                {
                    attackObject.GetComponent<CapsuleCollider2D>().enabled = false;
                }

                chargeRecoveryTime = maxChargeRecoveryTime;

                startCharge = false;
                startChargeRecovery = true;

            }
        }

        if (startChargeRecovery)
        {

            chargeRecoveryTime -= Time.deltaTime;

            if(chargeRecoveryTime < 0)
            {
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

        #region General Info Gathering

        player = GameObject.FindGameObjectWithTag("Player");

        currentlyAttacking = true;
        enemyMovment.stop = true;

        int whatAttack = Random.Range(0, allAttacksObjects.Count);

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

                Vector3 whereToLook = player.transform.position - attackObject.transform.position; // Får Vart Den Ska Titta
                float angle = Mathf.Atan2(whereToLook.y, whereToLook.x) * Mathf.Rad2Deg; // Får Vinkeln Där Den Ska Titta
                attackObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // Sätter Rotationen


                attackRayHit = Physics2D.Raycast(attackObject.transform.position, whereToLook, Mathf.Infinity, stopLayers);

                if (attackRayHit.collider != null)
                {

                    if(attackRayHit.distance/2 <= stretchAttackRange)
                    {

                        attackObject.SetActive(true);
    
                        maxHowFastStretchAttack = (attackRayHit.distance / 2) / stretchSpeed; // Dividera Med 2 För Att Tänka På Att Skalan Ökas Också
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

                playerDirection = Mathf.Sign(player.transform.position.x - transform.position.x); // Får Om Den Position i korrelation med fiendes position och sen ger 1 eller -1

                chargeStartupTime = maxChargeStartupTime;

                anticipateCharge = true;

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

        enemyMovment.stop = false;

        howFastGoBack = maxHowFastGoBack;
        howFastAttack = maxHowFastStretchAttack;
        howLongStayAfterStretchAttack = maxHowLongStayAfterStretchAttack;

        chargeStartupTime = maxChargeStartupTime;
        chargeTime = maxChargeTime;
        chargeRecoveryTime = maxChargeRecoveryTime;

        anticipateCharge = false;
        startCharge = false;
        startChargeRecovery = false;

        startStretchAttack = false;
        startWaitingToGoBack = false;
        startGoingBack = false;
        currentlyAttacking = false;

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
