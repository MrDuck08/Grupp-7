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

    [SerializeField] int[] whatAttackInt;
    int whatAttackForAttack;

    EnemyFollowPlayer enemyMovment;

    [Header("Attacks")]

    #region Attacks

    #region Stretch Attack

    [Header("Stretch Attacks")]

    [SerializeField] float speed;

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

            attackObject.transform.position += attackObject.transform.right  * speed * Time.deltaTime;

            attackObject.transform.localScale += new Vector3(speed * Time.deltaTime, 0, 0);

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

            attackObject.transform.position -= attackObject.transform.right * speed * Time.deltaTime;

            attackObject.transform.localScale -= new Vector3(speed * Time.deltaTime, 0);

            foreach (GameObject weakPoints in weakPointList)
            {

                foreach (Vector3 weakPointsVector in weakPointTransformList)
                {

                    weakPoints.transform.localScale += new Vector3(weakPointsVector.x / attackObject.transform.localScale.x / weakPointTransformList.Count * Time.deltaTime, 0, 0);

                }

            }

            if (howFastGoBack <= 0)
            {
                startGoingBack = false;

                enemyMovment.stop = false;

                howFastGoBack = maxHowFastGoBack;

                startStretchAttack = false;
                currentlyAttacking = false;

                weakPointList.Clear();
                weakPointTransformList.Clear();

                attackObject.SetActive(false);

            }
        }

        #endregion

    }

    public void Attack()
    {

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        currentlyAttacking = true;
        enemyMovment.stop = true;

        int whatAttack = Random.Range(0, allAttacksObjects.Count);
        whatAttackForAttack = whatAttack;

        attackObject = allAttacksObjects[whatAttack];

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

                        maxHowFastStretchAttack = (attackRayHit.distance / 2) / speed; // Dividera Med 2 För Att Tänka På Att Skalan Ökas Också
                        howFastAttack = maxHowFastStretchAttack;

                        Transform AttackObject = attackObject.transform.Find("WeakPointCollection");
                        foreach (Transform child in AttackObject.transform)
                        {
                            weakPointTransformList.Add(child.localScale);
                            weakPointList.Add(child.gameObject);
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

                #endregion

                break;

            case 1:

            break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 direction = playerLocation.position - transform.position;
        //Gizmos.DrawRay(transform.position, direction);
    }

}
