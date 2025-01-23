using UnityEngine;
using UnityEngine.Windows;

public class EnemyFollowPlayer : MonoBehaviour
{
    public float speed;
    public float lineOfSite;
    public float attackRange;
    public float baseLineOfSite;

    private Transform player;

    public Vector3 startTransform;

    public bool stop = false;
    bool facingRight = false;

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
            enemyAttack.Attack();

            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        #region Change where looking

        if (distanceFromPlayer < lineOfSite && !stop)
        {

            float whereToLook = Mathf.Sign(player.position.x - transform.position.x);

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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
