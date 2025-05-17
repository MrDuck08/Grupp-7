using System.Collections;
using UnityEngine;
using UnityEngine.Windows;

public class EnemyFollowPlayer : MonoBehaviour
{
    public float speed;
    public float lineOfSite;
    public float attackRange;
    public float baseLineOfSite;
    float whereToLook;
    [SerializeField] float knockBackForce = 50;

    private Transform player;

    public Vector3 startTransform;

    public bool stop = false;
    public bool attack = false;
    bool facingRight = false;

    #region Check For Ground

    [Header("Check For Ground Stuff")]

    [SerializeField] GameObject checkForGroundObject;
    [SerializeField] LayerMask groundLayer;

    #endregion

    EnemyAttack enemyAttack;
    Rigidbody2D rigidbody2D;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        enemyAttack = GetComponent<EnemyAttack>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        baseLineOfSite = lineOfSite;

        startTransform = transform.position;
    }

    private void FixedUpdate()
    {

        #region Move & Detect

        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceFromPlayer < lineOfSite && distanceFromPlayer > attackRange && !stop)
        {
            lineOfSite = baseLineOfSite * 2;

            float Direction = Mathf.Sign(player.position.x - transform.position.x);

            Vector2 MovePos = new Vector2(transform.position.x + Direction * speed * Time.deltaTime, transform.position.y);

            transform.position = MovePos;

            rigidbody2D.constraints = RigidbodyConstraints2D.None;

        }
        else if (distanceFromPlayer <= attackRange)
        {

            if (enemyAttack.anticipateFeintCharge && stop)
            {
                Debug.Log("Attack 2");

                enemyAttack.anticipateFeintCharge = false;
                enemyAttack.StopWeakpointAnimation();

                enemyAttack.Attack();

            }

            if (enemyAttack != null && !stop)
            {
                Debug.Log("Attack");

                enemyAttack.Attack();

                attack = true;
                rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;

            }

        }

        #endregion

        #region Change where looking

        whereToLook = Mathf.Sign(player.position.x - transform.position.x);

        if (distanceFromPlayer < lineOfSite)
        {

            if (whereToLook < 0 && facingRight == true)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
                facingRight = false;
            }

            if (whereToLook > 0 && facingRight == false)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y);
                facingRight = true;
            }

        }

        #endregion

        if(stop)
        {

            if(enemyAttack.jumpDown == true || enemyAttack.jumpUp == true)
            {
                return;
            }

            Vector2 GroundCheckPos = checkForGroundObject.transform.position;
            bool GroundCheckBool = Physics2D.OverlapCircle(GroundCheckPos, 0.1f, groundLayer);

            if (!GroundCheckBool)
            {

                transform.position = new Vector2(transform.position.x, transform.position.y - 2f * Time.deltaTime);

            }

        }

    }

    #region Knockback

    public IEnumerator Knockback()
    {

        if(enemyAttack != null)
        {
            enemyAttack.completeStop = true;
            enemyAttack.ResetAttack();
        }

        stop = true;

        rigidbody2D.constraints = RigidbodyConstraints2D.None;

        rigidbody2D.linearVelocity = new Vector2(knockBackForce * -whereToLook, knockBackForce/2);

        yield return new WaitForSeconds(0.5f);

        if(enemyAttack != null)
        {
            enemyAttack.completeStop = false;
            stop = false;
        }

    }

    #endregion

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(checkForGroundObject.transform.position, 0.1f);
    }
}
