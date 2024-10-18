using UnityEditor.Rendering;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    public float speed;
    public float lineOfSite;
    public float attackRange;

    private Transform player;

    public bool stop = false;

    EnemyAttack enemyAttack;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        enemyAttack = GetComponent<EnemyAttack>();
    }


    private void FixedUpdate()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceFromPlayer < lineOfSite && distanceFromPlayer > attackRange && !stop)
        {
            
            float Direction = Mathf.Sign(player.position.x - transform.position.x);

            Vector2 MovePos = new Vector2(transform.position.x + Direction * speed * Time.deltaTime, transform.position.y);

            transform.position = MovePos;


        }
        else if (distanceFromPlayer <= attackRange && !stop)
        {
            enemyAttack.Attack();
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
