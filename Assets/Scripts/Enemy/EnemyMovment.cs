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
    bool facingRight = false;

    EnemyAttack enemyAttack;
    FinalBoss finalBoss;
    float mmk = 12;
    Rigidbody2D rigidbody2D;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        enemyAttack = GetComponent<EnemyAttack>();
        finalBoss = GetComponent<FinalBoss>();
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
        else if (distanceFromPlayer <= attackRange && !stop)
        {
            if (enemyAttack != null)
            {
                enemyAttack.Attack();
            }
            if(finalBoss != null)
            {

            }

            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        #endregion

        #region Change where looking

        whereToLook = Mathf.Sign(player.position.x - transform.position.x);

        if (distanceFromPlayer < lineOfSite && !stop)
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
    }
}
