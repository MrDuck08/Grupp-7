using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    bool ifEnemyInRange = true;
    bool currentlyAttacking = false;

    List<GameObject> allAttacksObjects = new List<GameObject>();

    [SerializeField] int[] whatAttackInt;

    int whatAttackForAttack;

    Transform attackRotation;

    float timeUntilAttack;
    [SerializeField] float maxtTimeUntilAttack = 1f;
    [SerializeField] float speed;

    float attackDistance;
    float howFastAttack;
    [SerializeField] float maxHowFastAttack = 0.2f;
    float howLongStayAfterAttack;
    [SerializeField] float maxHowLongStayAfterAttack = 0.2f;
    float howFastGoBack;
    [SerializeField] float maxHowFastGoBack = 0.3f;

    bool startAttack = false;
    bool startWaitingToGoBack = false;
    bool startGoingBack = false;

    RaycastHit2D attackRayHit;

    [SerializeField] LayerMask stopLayers;

    void Start()
    {
        Transform Attacks = transform.Find("EnemyAttackObjects");
        foreach (Transform child in Attacks.transform)
        {
            
            allAttacksObjects.Add(child.gameObject);

        }

        howFastAttack = maxHowFastAttack;
        howLongStayAfterAttack = maxHowLongStayAfterAttack;
        howFastGoBack = maxHowFastGoBack;

    }

    void Update()
    {

        if (ifEnemyInRange && !currentlyAttacking)
        {
            currentlyAttacking = true;


            Attack();


        }

        if (startAttack)
        {
            howFastAttack -= Time.deltaTime;

            allAttacksObjects[whatAttackForAttack].transform.position += allAttacksObjects[whatAttackForAttack].transform.right  * speed * Time.deltaTime;

            allAttacksObjects[whatAttackForAttack].transform.localScale += new Vector3(speed * Time.deltaTime, 0, 0);

            if (howFastAttack <= 0)
            {
                startAttack = false;
                startWaitingToGoBack = true;

                howFastAttack = maxHowFastAttack;
            }
        }

        if (startWaitingToGoBack)
        {
            howLongStayAfterAttack -= Time.deltaTime;

            if (howLongStayAfterAttack <= 0)
            {
                startWaitingToGoBack = false;
                startGoingBack = true;

                howLongStayAfterAttack = maxHowLongStayAfterAttack;
                howFastGoBack = maxHowFastAttack;
            }
        }

        if (startGoingBack)
        {
            howFastGoBack -= Time.deltaTime;

            allAttacksObjects[whatAttackForAttack].transform.position -= allAttacksObjects[whatAttackForAttack].transform.right * speed * Time.deltaTime;

            allAttacksObjects[whatAttackForAttack].transform.localScale -= new Vector3(speed * Time.deltaTime, 0);

            if (howFastGoBack <= 0)
            {
                startGoingBack = false;

                howFastGoBack = maxHowFastGoBack;

            }
        }
    }

    void Attack()
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");

        int whatAttack = Random.Range(0, allAttacksObjects.Count);
        whatAttackForAttack = whatAttack;
        allAttacksObjects[whatAttack].SetActive(true);

        attackRotation = allAttacksObjects[whatAttack].transform;

        switch (whatAttackInt[whatAttack])
        {
            case 0:
                // Stretch Attack

                Vector3 whereToLook = Player.transform.position - allAttacksObjects[whatAttack].transform.position;
                float angle = Mathf.Atan2(whereToLook.y, whereToLook.x) * Mathf.Rad2Deg;
                allAttacksObjects[whatAttack].transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                attackRotation.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                attackDistance = Vector2.Distance(Player.transform.position, allAttacksObjects[whatAttack].transform.position);

                attackRayHit = Physics2D.Raycast(allAttacksObjects[whatAttack].transform.position, Player.transform.position, 1000f, stopLayers);

                if (attackRayHit.collider != null)
                {
                    Debug.Log(attackRayHit.distance);
                    maxHowFastAttack = attackRayHit.distance/speed;
                    howFastAttack = maxHowFastAttack;
                }

                startAttack = true;



                break;

            case 1:

            break;
        }
    }

}
